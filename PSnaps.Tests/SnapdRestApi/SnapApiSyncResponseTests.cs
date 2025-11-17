#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using PSnaps.SnapCore;
using PSnaps.SnapdRestApi;
using PSnaps.SnapdRestApi.Responses;
using System.Text.Json;
using ExpectedSnapTypeCounts = (int AppSnaps, int BaseSnaps, int SnapdSnaps);

namespace PSnaps.Tests.SnapdRestApi;

[TestFixture]
[TestOf ( typeof( SnapApiSyncResponse<> ) )]
[TestOf ( typeof( SnapApiJsonSerializerContext ) )]
[TestOf ( typeof( AppSnap ) )]
[TestOf ( typeof( BaseSnap ) )]
[TestOf ( typeof( SnapdSnap ) )]
[TestOf ( typeof( SnapPackage ) )]
[Category ( "Serialization" )]
public class SnapApiSyncResponseTests
{
  private static IEnumerable<TestCaseData<SnapApiSyncResponse<SnapPackage[]?>, ExpectedSnapTypeCounts>> ResponsesWithMultipleSnapPackages_DifferentTypes
  {
    get
    {
      BaseSnap baseSnap1 = new ( )
                           {
                             Channel         = "BaseSnapChannel",
                             Description     = "A base snap",
                             Id              = "BaseSnap1",
                             Name            = "BaseSnap1",
                             Revision        = "1",
                             TrackingChannel = "BaseSnapTrackingChannel",
                             Version         = "1.0.0"
                           };
      BaseSnap baseSnap2 = new ( )
                           {
                             Channel         = "BaseSnapChannel",
                             Description     = "A base snap",
                             Id              = "BaseSnap2",
                             Name            = "BaseSnap2",
                             Revision        = "1",
                             TrackingChannel = "BaseSnapTrackingChannel",
                             Version         = "1.0.0"
                           };
      AppSnap appSnap1 = new ( )
                         {
                           Base            = baseSnap1.Name,
                           Channel         = "AppSnapChannel",
                           Description     = "An app snap",
                           Id              = "AppSnap1",
                           Name            = "AppSnap1",
                           Revision        = "1",
                           TrackingChannel = "AppSnapTrackingChannel",
                           Version         = "1.0.0"
                         };
      AppSnap appSnap2 = new ( )
                         {
                           Base            = baseSnap1.Name,
                           Channel         = "AppSnapChannel",
                           Description     = "An app snap",
                           Id              = "AppSnap2",
                           Name            = "AppSnap2",
                           Revision        = "1",
                           TrackingChannel = "AppSnapTrackingChannel",
                           Version         = "1.0.0"
                         };
      AppSnap appSnap3 = new ( )
                         {
                           Base            = baseSnap2.Name,
                           Channel         = "AppSnapChannel",
                           Description     = "An app snap",
                           Id              = "AppSnap3",
                           Name            = "AppSnap3",
                           Revision        = "1",
                           TrackingChannel = "AppSnapTrackingChannel",
                           Version         = "1.0.0"
                         };
      SnapdSnap snapdSnap1 = new ( )
                             {
                               Channel         = "SnapdSnapChannel",
                               Description     = "A Snapd snap",
                               Id              = "SnapdSnap1",
                               Name            = "SnapdSnap1",
                               Revision        = "1",
                               TrackingChannel = "SnapdSnapTrackingChannel",
                               Version         = "1.0.0"
                             };
      yield return new (
                        new ( )
                        {
                          Status     = "OK",
                          StatusCode = 200,
                          Result =
                          [
                            appSnap1,
                            baseSnap1,
                            snapdSnap1
                          ]
                        },
                        ( 1, 1, 1 )
                       );
      yield return new (
                        new ( )
                        {
                          Status     = "OK",
                          StatusCode = 200,
                          Result =
                          [
                            appSnap2,
                            appSnap3,
                            baseSnap1,
                            baseSnap2
                          ]
                        },
                        ( 2, 2, 0 )
                       );
      yield return new (
                        new ( )
                        {
                          Status     = "OK",
                          StatusCode = 200,
                          Result =
                          [
                            appSnap1,
                            appSnap2,
                            appSnap3,
                            baseSnap1,
                            baseSnap2,
                            snapdSnap1
                          ]
                        },
                        ( 3, 2, 1 )
                       );
    }
  }

  private static IEnumerable<TestCaseData<SnapApiSyncResponse<SnapPackage[]?>, Type>> SingleElementResponses_OneOfEachPackageType
  {
    get
    {
      yield return new (
                        new ( )
                        {
                          StatusCode = 200,
                          Status     = "OK",
                          Result =
                          [
                            new AppSnap
                            {
                              Base            = "AppSnapBase",
                              Channel         = "AppSnapChannel",
                              Description     = "An app snap",
                              Id              = "AppSnap1",
                              Name            = "AppSnap1",
                              Revision        = "1",
                              TrackingChannel = "AppSnapTrackingChannel",
                              Version         = "1.0.0"
                            }
                          ]
                        },
                        typeof( AppSnap )
                       );
      yield return new (
                        new ( )
                        {
                          StatusCode = 200,
                          Status     = "OK",
                          Result =
                          [
                            new BaseSnap
                            {
                              Channel         = "BaseSnapChannel",
                              Description     = "A base snap",
                              Id              = "BaseSnap1",
                              Name            = "BaseSnap1",
                              Revision        = "1",
                              TrackingChannel = "BaseSnapTrackingChannel",
                              Version         = "1.0.0"
                            }
                          ]
                        },
                        typeof( BaseSnap )
                       );
      yield return new (
                        new ( )
                        {
                          StatusCode = 200,
                          Status     = "OK",
                          Result =
                          [
                            new SnapdSnap
                            {
                              Channel         = "SnapdSnapChannel",
                              Description     = "A Snapd snap",
                              Id              = "SnapdSnap1",
                              Name            = "SnapdSnap",
                              Revision        = "1",
                              TrackingChannel = "SnapdSnapTrackingChannel",
                              Version         = "1.0.0"
                            }
                          ]
                        },
                        typeof( SnapdSnap )
                       );
    }
  }

  [Test]
  [TestCaseSource ( nameof (ResponsesWithMultipleSnapPackages_DifferentTypes) )]
  public void CorrectPolymorphicDeserializationOfMultipleFromSameResponse ( SnapApiSyncResponse<SnapPackage[]?> response, ExpectedSnapTypeCounts expectedCounts )
  {
    Assume.That ( response,        Is.Not.Null );
    Assume.That ( response.Result, Is.Not.Null );
    Assume.That ( response.Result, Is.Not.Empty );
    Assume.That ( response.Result, Has.Length.AtLeast ( 2 ) );

    // Use reflection-based serializer.
    string json = JsonSerializer.Serialize ( response );
    Assume.That ( json, Is.Not.Null );
    Assume.That ( json, Is.Not.Empty );
    Assume.That ( json, Is.Not.WhiteSpace );

    // Deserialize with JsonTypeInfo<T> as is done in the module.
    SnapApiSyncResponse<SnapPackage[]?>? deserializedResponse = JsonSerializer.Deserialize<SnapApiSyncResponse<SnapPackage[]?>> ( json, SnapApiJsonSerializerContext.Default.SnapApiSyncResponseSnapPackageArray );

    Assert.That ( deserializedResponse,        Is.Not.Null );
    Assert.That ( deserializedResponse.Result, Is.Not.Null );
    Assert.That ( deserializedResponse.Result, Is.Not.Empty );
    Assert.That ( deserializedResponse.Result, Has.Exactly ( expectedCounts.AppSnaps ).InstanceOf<AppSnap> ( ) );
    Assert.That ( deserializedResponse.Result, Has.Exactly ( expectedCounts.BaseSnaps ).InstanceOf<BaseSnap> ( ) );
    Assert.That ( deserializedResponse.Result, Has.Exactly ( expectedCounts.SnapdSnaps ).InstanceOf<SnapdSnap> ( ) );
    Assert.That ( deserializedResponse.Result, Is.EquivalentTo ( response.Result ) );

    // Also make sure that they aren't somehow just copies of the original objects, for test validity.
    foreach ( SnapPackage originalPackage in response.Result )
    {
      Assume.That ( deserializedResponse.Result, Has.None.SameAs ( originalPackage ) );
    }
  }

  [Test]
  [TestCaseSource ( nameof (SingleElementResponses_OneOfEachPackageType) )]
  public void CorrectPolymorphicDeserializationOfSingleFromResponse ( SnapApiSyncResponse<SnapPackage[]?> response, Type expectedType )
  {
    Assume.That ( response,        Is.Not.Null );
    Assume.That ( response.Result, Is.Not.Null );
    Assume.That ( response.Result, Is.Not.Empty );
    Assume.That ( response.Result, Has.Exactly ( 1 ).InstanceOf ( expectedType ) );

    // Use reflection-based serializer.
    string json = JsonSerializer.Serialize ( response );
    Assume.That ( json, Is.Not.Null );
    Assume.That ( json, Is.Not.Empty );
    Assume.That ( json, Is.Not.WhiteSpace );

    // Deserialize with JsonTypeInfo<T> as is done in the module.
    SnapApiSyncResponse<SnapPackage[]?>? deserializedResponse = JsonSerializer.Deserialize<SnapApiSyncResponse<SnapPackage[]?>> ( json, SnapApiJsonSerializerContext.Default.SnapApiSyncResponseSnapPackageArray );

    Assert.That ( deserializedResponse,        Is.Not.Null );
    Assert.That ( deserializedResponse.Result, Is.Not.Null );
    Assert.That ( deserializedResponse.Result, Is.Not.Empty );
    Assert.That ( deserializedResponse.Result, Has.Exactly ( 1 ).InstanceOf ( expectedType ) );
    Assert.That ( deserializedResponse.Result, Is.EquivalentTo ( response.Result ) );
    Assume.That ( deserializedResponse.Result [ 0 ], Is.Not.SameAs ( response.Result [ 0 ] ) );
  }
}
