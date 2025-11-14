#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using PSnaps.SnapCore;

namespace PSnaps.Tests.SnapCore;

[TestFixture]
[TestOf ( typeof( SnapPackage ) )]
[Parallelizable ( ParallelScope.All )]
public class SnapPackageTests
{
  private static IEnumerable<SnapPackage> BasicPackageCollection
  {
    get
    {
      yield return new ( )
                   {
                     Channel         = "Channel",
                     Description     = "Description",
                     Id              = "Id1",
                     Name            = "Package1",
                     Revision        = "0",
                     TrackingChannel = "stable",
                     Version         = "1.0.0"
                   };
      yield return new ( )
                   {
                     Channel         = "Channel",
                     Description     = "Description",
                     Id              = "Id2",
                     Name            = "Package2",
                     Revision        = "0",
                     TrackingChannel = "stable",
                     Version         = "1.0.0"
                   };
      yield return new ( )
                   {
                     Channel         = "Channel",
                     Description     = "Description",
                     Id              = "Id3",
                     Name            = "Package3",
                     Revision        = "0",
                     TrackingChannel = "stable",
                     Version         = "1.0.0"
                   };
    }
  }

  private static IEnumerable<TestCaseData<SnapPackage?, SnapPackage?>> CompareTo_ExpectedOrdering_Cases
  {
    get
    {
      SnapPackage package1A = new ( )
                              {
                                Channel         = "Channel",
                                Description     = "Description",
                                Id              = "Id",
                                Name            = "Package1",
                                Revision        = "0",
                                TrackingChannel = "stable",
                                Version         = "1.0.0"
                              };
      yield return new ( package1A, package1A ) { ExpectedResult = 0, TestName = "Same Object" };

      SnapPackage package1B = new ( )
                              {
                                Channel         = "Channel",
                                Description     = "Description",
                                Id              = "Id",
                                Name            = "Package1",
                                Revision        = "0",
                                TrackingChannel = "stable",
                                Version         = "1.0.0"
                              };
      yield return new ( package1A, package1B ) { ExpectedResult = 0, TestName = "Identical Objects" };
      yield return new ( package1A, null ) { ExpectedResult      = 1, TestName = "Null Comes First" };

      SnapPackage package2 = new ( )
                             {
                               Channel         = "Channel",
                               Description     = "Description",
                               Id              = "Id",
                               Name            = "Package2",
                               Revision        = "0",
                               TrackingChannel = "stable",
                               Version         = "1.0.0"
                             };
      yield return new ( package1A, package2 ) { ExpectedResult = -1, TestName = "Only Name Differs - Left First" };
      yield return new ( package2, package1A ) { ExpectedResult = 1, TestName  = "Only Name Differs - Right First" };
    }
  }

  private static IEnumerable<TestCaseData<SnapPackage?, SnapPackage?>> LessThanOperator_ExpectedResult_Cases
  {
    get
    {
      SnapPackage package1A = new ( )
                              {
                                Channel         = "Channel",
                                Description     = "Description",
                                Id              = "Id",
                                Name            = "Package1",
                                Revision        = "0",
                                TrackingChannel = "stable",
                                Version         = "1.0.0"
                              };
      yield return new ( package1A, package1A ) { ExpectedResult = false, TestName = "Same Object" };

      SnapPackage package1B = new ( )
                              {
                                Channel         = "Channel",
                                Description     = "Description",
                                Id              = "Id",
                                Name            = "Package1",
                                Revision        = "0",
                                TrackingChannel = "stable",
                                Version         = "1.0.0"
                              };
      yield return new ( package1A, package1B ) { ExpectedResult = false, TestName = "Identical Objects" };
      yield return new ( package1A, null ) { ExpectedResult      = false, TestName = "Null Comes First" };
      yield return new ( null, null ) { ExpectedResult           = false, TestName = "Null Not Less Than Null" };

      SnapPackage package2 = new ( )
                             {
                               Channel         = "Channel",
                               Description     = "Description",
                               Id              = "Id",
                               Name            = "Package2",
                               Revision        = "0",
                               TrackingChannel = "stable",
                               Version         = "1.0.0"
                             };
      yield return new ( package1A, package2 ) { ExpectedResult = true, TestName  = "Only Name Differs - Left < Right" };
      yield return new ( package2, package1A ) { ExpectedResult = false, TestName = "Only Name Differs - Left > Right" };
    }
  }

  private static IEnumerable<TestCaseData<SnapPackage?, SnapPackage?>> LessThanOrEqualOperator_ExpectedResult_Cases
  {
    get
    {
      SnapPackage package1A = new ( )
                              {
                                Channel         = "Channel",
                                Description     = "Description",
                                Id              = "Id",
                                Name            = "Package1",
                                Revision        = "0",
                                TrackingChannel = "stable",
                                Version         = "1.0.0"
                              };
      yield return new ( package1A, package1A ) { ExpectedResult = true, TestName = "Same Object" };

      SnapPackage package1B = new ( )
                              {
                                Channel         = "Channel",
                                Description     = "Description",
                                Id              = "Id",
                                Name            = "Package1",
                                Revision        = "0",
                                TrackingChannel = "stable",
                                Version         = "1.0.0"
                              };
      yield return new ( package1A, package1B ) { ExpectedResult = true, TestName  = "Identical Objects" };
      yield return new ( package1A, null ) { ExpectedResult      = false, TestName = "NotNull > Null" };
      yield return new ( null, null ) { ExpectedResult           = true, TestName  = "Null <= Null" };

      SnapPackage package2 = new ( )
                             {
                               Channel         = "Channel",
                               Description     = "Description",
                               Id              = "Id",
                               Name            = "Package2",
                               Revision        = "0",
                               TrackingChannel = "stable",
                               Version         = "1.0.0"
                             };
      yield return new ( package1A, package2 ) { ExpectedResult = true, TestName  = "Only Name Differs - Left <= Right" };
      yield return new ( package2, package1A ) { ExpectedResult = false, TestName = "Only Name Differs - Left >= Right" };
    }
  }

  [Test]
  [TestCaseSource ( nameof (CompareTo_ExpectedOrdering_Cases) )]
  public int CompareTo_ExpectedOrdering ( SnapPackage left, SnapPackage? right )
  {
    return left.CompareTo ( right );
  }

  [Test]
  public void CompareTo_OtherType_ThrowsArgumentException ( )
  {
    SnapPackage testPackage = new ( )
                              {
                                Channel         = "Channel",
                                Description     = "Description",
                                Id              = "Id",
                                Name            = "Package2",
                                Revision        = "0",
                                TrackingChannel = "stable",
                                Version         = "1.0.0"
                              };
    Assert.That ( ( ) => testPackage.CompareTo ( 0xDEADBEEF ), Throws.ArgumentException );
  }

  [Test ( ExpectedResult = 0 )]
  public int CompareTo_SnapPackageAsObject_DoesNotThrow ( )
  {
    SnapPackage testPackage = new ( )
                              {
                                Channel         = "Channel",
                                Description     = "Description",
                                Id              = "Id",
                                Name            = "Package2",
                                Revision        = "0",
                                TrackingChannel = "stable",
                                Version         = "1.0.0"
                              };
    object testPackageAsObject = testPackage;
    return testPackage.CompareTo ( testPackageAsObject );
  }

  [Test]
  public void GetHashCode_ReturnsHashCodeOfIdProperty ( [ValueSource ( nameof (BasicPackageCollection) )] SnapPackage snap )
  {
    Assume.That ( snap,    Is.Not.Null );
    Assume.That ( snap.Id, Is.Not.Null );
    Assume.That ( snap.Id, Is.Not.Empty );
    Assume.That ( snap.Id, Is.Not.WhiteSpace );

    Assert.That ( snap.GetHashCode ( ), Is.EqualTo ( snap.Id.GetHashCode ( ) ) );
  }

  [Test]
  [TestCaseSource ( nameof (LessThanOperator_ExpectedResult_Cases) )]
  public bool GreaterThanOperator_ExpectedResult ( SnapPackage? right, SnapPackage? left )
  {
    return left > right;
  }

  [Test]
  [TestCaseSource ( nameof (LessThanOrEqualOperator_ExpectedResult_Cases) )]
  public bool GreaterThanOrEqualOperator_ExpectedResult ( SnapPackage? right, SnapPackage? left )
  {
    return left >= right;
  }

  [Test]
  [TestCaseSource ( nameof (LessThanOperator_ExpectedResult_Cases) )]
  public bool LessThanOperator_ExpectedResult ( SnapPackage? left, SnapPackage? right )
  {
    return left < right;
  }

  [Test]
  [TestCaseSource ( nameof (LessThanOrEqualOperator_ExpectedResult_Cases) )]
  public bool LessThanOrEqualOperator_ExpectedResult ( SnapPackage? left, SnapPackage? right )
  {
    return left <= right;
  }
}
