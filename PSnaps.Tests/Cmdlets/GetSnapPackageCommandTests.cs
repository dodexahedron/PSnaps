#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.Tests.Cmdlets;

[TestFixture]
[Category ( "Commandlets" )]
[TestOf ( typeof( GetSnapPackageCommand ) )]
public class GetSnapPackageCommandTests
{
  private readonly MockSnapdClient _mockClient = new ( );

  private static IEnumerable<string[]?> NullAndEmptyStringArrays
  {
    get
    {
      yield return null;
      yield return [ ];
    }
  }

  private static IEnumerable<string[]?> ValidSnapNameArrays
  {
    get
    {
      yield return [ "core22" ];
      yield return [ "core22", "snapd" ];
    }
  }

  [OneTimeTearDown]
  public void DisposeMockClient ( )
  {
    _mockClient.Dispose ( );
  }

  [Test]
  public void GetSnapPackage_AlreadyCanceledToken_TaskCanceled ( [Values] bool all, [ValueSource ( nameof (NullAndEmptyStringArrays) )] [ValueSource ( nameof (ValidSnapNameArrays) )] string[]? name )
  {
    using CancellationTokenSource cts = new ( );
    GetSnapPackageCommand command = new ( )
                                    {
                                      All  = new ( all ),
                                      Name = name
                                    };
    cts.Cancel ( );
    CancellationToken cancelledToken = cts.Token;

    Assume.That ( cancelledToken.IsCancellationRequested, Is.True );

    Assert.That ( ( ) => command.GetSnapPackages ( cancelledToken ), Throws.TypeOf<OperationCanceledException> ( ) );
  }

  [Test]
  public void GetSnapPackage_EmptyNameArrayAndNoAllParameter_ReturnsNull ( )
  {
    GetSnapPackageCommand command = new ( )
                                    {
                                      All  = new ( false ),
                                      Name = [ ]
                                    };

    Assume.That ( command.All.IsPresent, Is.False );
    Assume.That ( command.Name,          Is.Empty );

    Assert.That ( command.GetSnapPackages ( ), Is.Null );
  }

  [Test]
  public void GetSnapPackage_NullNameAndAllParameterPresent_ReturnsPackages ( )
  {
    GetSnapPackageCommand command = new ( )
                                    {
                                      All  = new ( true ),
                                      Name = null
                                    };

    Assume.That ( command.All.IsPresent, Is.True );
    Assume.That ( command.Name,          Is.Null );

    List<SnapPackage>? snapPackages = command.GetSnapPackages ( );
    IDisposable        scope        = Assert.EnterMultipleScope ( );
    Assert.That ( snapPackages, Is.Not.Null );
    Assert.That ( snapPackages, Has.Count.GreaterThan ( 0 ) );
    Assert.That ( snapPackages, Has.All.InstanceOf<SnapPackage> ( ).And.Not.Null );
    scope.Dispose ( );
  }

  [Test]
  public void GetSnapPackage_NullOrEmptyNameAndNoAllParameter_ReturnsNull ( [ValueSource ( nameof (NullAndEmptyStringArrays) )] string[]? name )
  {
    GetSnapPackageCommand command = new ( )
                                    {
                                      All  = new ( false ),
                                      Name = name
                                    };

    Assume.That ( command.All.IsPresent, Is.False );
    Assume.That ( command.Name,          Is.Null.Or.Empty );

    Assert.That ( command.GetSnapPackages ( ), Is.Null );
  }

  [Test]
  public void GetSnapPackage_OneName_ExpectedResults ( [Values] bool all )
  {
    using CancellationTokenSource cts = new ( );
    GetSnapPackageCommand command = new ( )
                                    {
                                      All  = new ( all ),
                                      Name = [ "core22" ]
                                    };

    Assume.That ( cts.Token.IsCancellationRequested, Is.False );

    List<SnapPackage>? result = command.GetSnapPackages ( cts.Token );

    Assert.That ( result, Is.Not.Null );
    Assert.That ( result, Is.Not.Empty );
    Assert.That ( result, Has.Count.GreaterThanOrEqualTo ( 1 ) );
    Assert.That ( result, Has.All.Not.Null );
    Assert.That ( result, Has.All.InstanceOf<SnapPackage> ( ) );
  }

  [OneTimeSetUp]
  public void SetMockClient ( )
  {
    ISnapdRestClient? normalClient = PSnapsModuleState.ExchangeSnapdClient ( _mockClient );
    normalClient?.Dispose ( );
  }
}
