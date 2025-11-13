#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps;

using StringWrapperTuple = (string Str, char C);
using StringsWrapAndJoinTuple = (string[] Strings, char Separator, char Wrapper);

/// <summary>
///   Miscellaneous extensions on built-in types.
/// </summary>
public static class BaseTypeExtensions
{
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
  ///   This method is similar to <see cref="string.Join(char, object?[])" />, but also wraps each element as well.
  /// </remarks>
  public static string WrapAndJoin ( this string[] items, char separator = ',', char wrapper = '\'' )
  {
    int spanLength = items.Sum ( static i => i.Length + 3 ) - 1;
    return string.Create (
                          spanLength,
                          new StringsWrapAndJoinTuple ( items, separator, wrapper ),
                          static ( span, tuple ) =>
                          {
                            int position  = 0;
                            int itemIndex = 0;
                            span [ position ] = tuple.Wrapper;
                            tuple.Strings [ itemIndex ].AsSpan ( ).CopyTo ( span [ ++position.. ] );
                            span [ position += tuple.Strings [ itemIndex ].Length ] = tuple.Wrapper;

                            for ( position++, itemIndex++; itemIndex < tuple.Strings.Length && position < span.Length; itemIndex++, position++ )
                            {
                              span [ position ] = tuple.Separator;
                              ReadOnlySpan<char> itemSpan = tuple.Strings [ itemIndex ].AsSpan ( );
                              span [ ++position ] = tuple.Wrapper;
                              itemSpan.CopyTo ( span [ ++position.. ] );
                              span [ position += itemSpan.Length ] = tuple.Wrapper;
                            }
                          }
                         );
  }
}
