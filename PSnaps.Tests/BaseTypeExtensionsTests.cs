#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.Tests;

[TestFixture]
public class BaseTypeExtensionsTests
{
  public static string[][] InputStrings = [ [ "String 1", "String 2", "String C", "Fourth String", "String 5 has \'single quotes\', and also a comma." ] ];

  [Test]
  public void StringArray_WrapAndJoin_AllOutputComponentsInCorrectPositions ( [ValueSource ( nameof (InputStrings) )] string[] input, [Values ( ',', ':', ' ', ';' )] char separator, [Values ( '\'', '"', ':', ' ' ),] char wrapper )
  {
    Assume.That ( input,                          Is.Not.Null );
    Assume.That ( input,                          Is.Not.Empty );
    Assume.That ( input,                          Has.All.Length.Positive );
    Assume.That ( char.IsSurrogate ( separator ), Is.False );
    Assume.That ( char.IsSurrogate ( wrapper ),   Is.False );

    string output = input.WrapAndJoin ( separator, wrapper );

    using ( Assert.EnterMultipleScope ( ) )
    {
      Assert.That ( output, Is.Not.Null );
      Assert.That ( output, Is.Not.Empty );
      Assert.That ( output, Is.Not.WhiteSpace );
    }

    // Starting before the beginning of the string to cut out one of the first-iteration conditional checks in the loop body that wasn't getting unrolled.
    int outputStringIndex = -2;

    for (
      int inputIndex = 0;
      inputIndex < input.Length && outputStringIndex < output.Length;
      // Every loop iteration advances the input index by one and moves two characters right in the output string to accommodate the end wrapper for the previous element and the separator.
      inputIndex++ )
    {
      string inputElement = input [ inputIndex ];

      // For visibility and simple proof that we know exactly what we are expecting and where we are expecting it, go ahead and pre-compute these values:
      int separatorIndex    = outputStringIndex + 1;
      int startWrapperIndex = separatorIndex    + 1;
      int elementStartIndex = startWrapperIndex + 1;
      int elementLength     = inputElement.Length;
      int endWrapperIndex   = elementStartIndex + elementLength;

      // Pre-validate the test isn't going to overflow the string buffer.
      Assume.That ( endWrapperIndex, Is.LessThan ( output.Length ) );

      // Check that the new characters are in the right places.
      using ( Assert.EnterMultipleScope ( ) )
      {
        if ( separatorIndex > 0 )
        {
          Assert.That ( output [ separatorIndex ], Is.EqualTo ( separator ) );
        }

        char startWrapper = output [ startWrapperIndex ];
        Assert.That ( startWrapper, Is.EqualTo ( wrapper ) );
        char endWrapper = output [ endWrapperIndex ];
        Assert.That ( endWrapper,   Is.EqualTo ( wrapper ) );
      }

      // Check that the current input element exists verbatim at the expected location.
      Range  expectedSlice            = new ( elementStartIndex, elementStartIndex + elementLength );
      string textWhereElementShouldBe = output [ expectedSlice ];
      Assert.That ( textWhereElementShouldBe, Is.EqualTo ( inputElement ).Using ( StringComparer.Ordinal ) );

      textWhereElementShouldBe = output.Substring ( elementStartIndex, elementLength );
      Assert.That ( textWhereElementShouldBe, Is.EqualTo ( inputElement ).Using ( StringComparer.Ordinal ) );

      textWhereElementShouldBe = output [ elementStartIndex..endWrapperIndex ];
      Assert.That ( textWhereElementShouldBe, Is.EqualTo ( inputElement ).Using ( StringComparer.Ordinal ) );

      // Move to the last "consumed" character index to set up for next iteration.
      outputStringIndex = endWrapperIndex;
    }

    // Assume that we have run to the exact end of the output string to ensure the test logic isn't broken.
    Assume.That ( outputStringIndex, Is.EqualTo ( output.Length - 1 ) );
  }

}