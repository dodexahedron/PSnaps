#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using StringsWrapAndJoinTuple = (string[] Strings, char Separator, char Wrapper);

namespace PSnaps;

/// <summary>
///   Miscellaneous extensions on built-in types.
/// </summary>
public static class BaseTypeExtensions
{
  /// <summary>
  ///   Maximum length of a <see langword="string" /> in .net (2^30 <see langword="char" />s).
  /// </summary>
  private const int String_Max_Length = 0x40000000;

  /// <summary>
  ///   Without intermediate string allocations, wraps each element of the input collection with <paramref name="wrapper" />, and
  ///   separates each element with <paramref name="separator" />.
  /// </summary>
  /// <param name="items">The input string array to wrap and join.</param>
  /// <param name="separator">The separator character to use between each element in the output string.</param>
  /// <param name="wrapper">
  ///   The character that will be inserted immediately before and after each element of the input array.
  /// </param>
  /// <returns>
  ///   A single string containing all elements of the input array, with each one wrapped with <paramref name="wrapper" /> and
  ///   separated by <paramref name="separator" />.
  /// </returns>
  /// <remarks>
  ///   This method is similar to <see cref="string.Join(char, object?[])" />, but also wraps each element as well.<br />
  ///   Time and memory complexity is linear.<br />
  ///   The length of the output string is calculated by first performing FMA(3,items.Length,-1) and, if that value is already too
  ///   large for a string, throws before bothering to sum up the individual element lengths.
  /// </remarks>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   (With an inner exception of type <see cref="InsufficientMemoryException" />)<br />
  ///   If the input array is too large for the output to fit in a single string.
  /// </exception>
  /// <exception cref="OverflowException">
  ///   If sum of inserted characters and lengths of all elements of the input array overflow an <see langword="int" />.
  /// </exception>
  public static string WrapAndJoin ( this string[] items, char separator = ',', char wrapper = '\'' )
  {
    // We need this value as part of the total length anyway, but we can bail out before summing up the lengths of the elements if
    // this value is already too large to hold in a string, since clearly it's impossible to fit the input as well at that point,
    // even if they're all empty strings.
    int wrappersAndSeparatorsCount = (int)float.FusedMultiplyAdd ( 3f, items.Length, -1 );

    if ( wrappersAndSeparatorsCount >= String_Max_Length )
    {
      throw new ArgumentOutOfRangeException ( "Input array is too large for output to fit in a single string.", new InsufficientMemoryException ( $"Total inserted characters ({wrappersAndSeparatorsCount}) is larger than maximum string length of {String_Max_Length} chars." ) );
    }

    // Memory safety: Force this one to be checked so overflow throws regardless of compiler options, so we don't under-allocate.
    int totalOutputLength = checked ( wrappersAndSeparatorsCount + items.Sum ( static s => s.Length ) );

    // One last check on the total before creating the string.
    if ( totalOutputLength > String_Max_Length )
    {
      throw new ArgumentOutOfRangeException ( "Input array is too large for output to fit in a single string.", new InsufficientMemoryException ( $"Sum of element lengths plus inserted characters ({totalOutputLength}) exceeds {String_Max_Length} characters." ) );
    }

    // Now create the string in one pass, inserting the wrappers, separators, and input elements directly into the
    // destination span, which .net will then directly use for the output string without allocating or copying again.
    return string.Create (
                          totalOutputLength,
                          new StringsWrapAndJoinTuple ( items, separator, wrapper ),
                          static ( span, tuple ) =>
                          {
                            int position  = 0;
                            int itemIndex = 0;
                            // Either a first-element check is needed in the loop to elide one separator,
                            // or we can just unroll the first element and use branchless code in the loop.
                            // The compiler wasn't unrolling it in the loop with check version, so the first
                            // element has been manually unrolled here, before the loop starts (which it of
                            // course won't, if there's only one element in the input).
                            span [ position ] = tuple.Wrapper;
                            tuple.Strings [ itemIndex ].AsSpan ( ).CopyTo ( span [ ++position.. ] );
                            span [ position += tuple.Strings [ itemIndex ].Length ] = tuple.Wrapper;

                            for ( position++, itemIndex++; itemIndex < tuple.Strings.Length && position < span.Length; itemIndex++, position++ )
                            {
                              span [ position ]   = tuple.Separator;
                              span [ ++position ] = tuple.Wrapper;
                              ReadOnlySpan<char> itemSpan = tuple.Strings [ itemIndex ].AsSpan ( );
                              itemSpan.CopyTo ( span [ ++position.. ] );
                              span [ position += itemSpan.Length ] = tuple.Wrapper;
                            }
                          }
                         );
  }
}
