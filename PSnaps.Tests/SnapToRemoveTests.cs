#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using System.Globalization;
using PSnaps.Cmdlets;

namespace PSnaps.Tests;

[TestFixture]
[TestOf ( typeof( SnapToRemove ) )]
[Parallelizable ( ParallelScope.All )]
public class SnapToRemoveTests
{
  private static IEnumerable<string> BadInputStringsForParseTests
  {
    get
    {
      yield return @"\*+";
      yield return @"N\1\2";
      yield return "N/1.2.3/321";
      yield return "Name/1.2.3/InvalidRevision";
    }
  }

  private static IEnumerable<TestCaseData<string>> GoodInputStringsAndExpectedResultsForParseTests
  {
    get
    {
      yield return new ( "AllComponentsValid/1.2.3/321" ) { ExpectedResult                  = new SnapToRemove ( "AllComponentsValid",              "1.2.3", "321" ) };
      yield return new ( "AllComponentsValid_RevisionStar/1.2.3/*" ) { ExpectedResult       = new SnapToRemove ( "AllComponentsValid_RevisionStar", "1.2.3" ) };
      yield return new ( "AllComponentsValid_VersionStar/*/321" ) { ExpectedResult          = new SnapToRemove ( "AllComponentsValid_VersionStar",  "*", "321" ) };
      yield return new ( "AllComponentsValid_VersionAndRevisionStar/*/*" ) { ExpectedResult = new SnapToRemove ( "AllComponentsValid_VersionAndRevisionStar" ) };
    }
  }

  private static IEnumerable<TestCaseData<string?, string?, string?>> ToString_CorrectOutput_Cases
  {
    get
    {
      yield return new ( "AllComponentsValid", "1.2.3", "321" ) { ExpectedResult                            = "AllComponentsValid/1.2.3/321" };
      yield return new ( "NameAndVersionValid_RevisionNull", "1.2.3", null ) { ExpectedResult               = "NameAndVersionValid_RevisionNull/1.2.3/*" };
      yield return new ( "NameAndRevisionValid_VersionNull", null, "321" ) { ExpectedResult                 = "NameAndRevisionValid_VersionNull/*/321" };
      yield return new ( "NameValid_VersionAndRevisionNull", null, null ) { ExpectedResult                  = "NameValid_VersionAndRevisionNull/*/*" };
      yield return new ( "NameAndVersionValid_RevisionEmpty", "1.2.3", string.Empty ) { ExpectedResult      = "NameAndVersionValid_RevisionEmpty/1.2.3/*" };
      yield return new ( "NameAndRevisionValid_VersionEmpty", string.Empty, "321" ) { ExpectedResult        = "NameAndRevisionValid_VersionEmpty/*/321" };
      yield return new ( "NameValid_VersionAndRevisionEmpty", string.Empty, string.Empty ) { ExpectedResult = "NameValid_VersionAndRevisionEmpty/*/*" };
      yield return new ( "NameAndVersionValid_RevisionWhitespace", "1.2.3", " " ) { ExpectedResult          = "NameAndVersionValid_RevisionWhitespace/1.2.3/*" };
      yield return new ( "NameAndRevisionValid_VersionWhitespace", " ", "321" ) { ExpectedResult            = "NameAndRevisionValid_VersionWhitespace/*/321" };
      yield return new ( "NameValid_VersionAndRevisionWhitespace", " ", " " ) { ExpectedResult              = "NameValid_VersionAndRevisionWhitespace/*/*" };
    }
  }

  [Test]
  public void Constructor_NullNameThrowsArgumentNullException ( )
  {
    Assert.That ( static ( ) => new SnapToRemove ( null! ), Throws.ArgumentNullException );
  }

  [Test]
  public void Constructor_WhitespaceOrEmptyNameThrowsArgumentException ( [Values ( "", " ", "\t", "\n", "\xa0", "\x2002", "\x2003", "\x2009" )] string name )
  {
    Assert.That ( ( ) => new SnapToRemove ( name ), Throws.ArgumentException );
  }

  [Test]
  public void Deconstruct_AllComponentsCorrect ( )
  {
    const string snapName     = "SnapName";
    const string snapVersion  = "1.2.3";
    const string snapRevision = "321";
    SnapToRemove testObject   = new ( snapName, snapVersion, snapRevision );
    ( string name, string version, string revision ) = testObject;
    Assert.That ( name,     Is.EqualTo ( snapName ) );
    Assert.That ( version,  Is.EqualTo ( snapVersion ) );
    Assert.That ( revision, Is.EqualTo ( snapRevision ) );
  }

  [Test]
  public void Parse_BadInputThrowsFormatException ( [ValueSource ( nameof (BadInputStringsForParseTests) )] string input )
  {
    Assert.That ( ( ) => SnapToRemove.Parse ( input ), Throws.TypeOf<FormatException> ( ) );
  }

  [Test]
  [TestCaseSource ( nameof (GoodInputStringsAndExpectedResultsForParseTests) )]
  public SnapToRemove Parse_ReturnsExpectedObject ( string input )
  {
    return SnapToRemove.Parse ( input );
  }

  [Test]
  public void Parse_WhitespaceInputThrowsArgumentException ( [Values ( "", " ", "\t", "\n", "\xa0", "\x2002", "\x2003", "\x2009" )] string input )
  {
    Assert.That ( ( ) => SnapToRemove.Parse ( input ), Throws.TypeOf<ArgumentException> ( ) );
  }

  [Test]
  [TestCaseSource ( nameof (ToString_CorrectOutput_Cases) )]
  public string ToString_CorrectOutput ( string name, string? version, string? revision )
  {
    return new SnapToRemove ( name, version!, revision! ).ToString ( );
  }

  [Test]
  public void TryParse_FalseAndResultNullForBadInput ( [ValueSource ( nameof (BadInputStringsForParseTests) )] string input )
  {
    Assert.That ( SnapToRemove.TryParse ( input, CultureInfo.CurrentCulture, out SnapToRemove? result ), Is.False );
    Assert.That ( result,                                                                                Is.Null );
  }

  [Test]
  [TestCaseSource ( nameof (GoodInputStringsAndExpectedResultsForParseTests) )]
  public SnapToRemove? TryParse_TrueAndExpectedObjectForGoodInput ( string input )
  {
    Assert.That ( SnapToRemove.TryParse ( input, null, out SnapToRemove? result ), Is.True );
    return result;
  }
}
