#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

#pragma warning disable CA1416

namespace PSnaps.Tests.Cmdlets;

/// <remarks>
///   This test fixture should not be run in parallel, since it messes with global state and could potentially cause random test
///   failures if run in parallel with other test fixtures.
/// </remarks>
[TestFixture]
[TestOf ( typeof( SetSnapdClientCommand ) )]
[Category ( "Commandlets" )]
[Category ( "Module State" )]
[NonParallelizable]
public class SetSnapdClientCommandTests
{
  private readonly System.Collections.Concurrent.ConcurrentStack<ISnapdRestClient> _clientStack = [ ];

  [OneTimeTearDown]
  public void AfterAllTests_EnsureModuleStateHasSnapdClient ( )
  {
    if ( !_clientStack.TryPop ( out ISnapdRestClient? originalClient ) )
    {
      return;
    }

    Assert.That ( originalClient, Is.Not.Null );
    PSnapsModuleState.ExchangeSnapdClient ( originalClient )?.Dispose ( );
  }

  [TearDown]
  public void AfterEachTest ( )
  {
    // Pop the stack and dispose the client from the last test.
    if ( _clientStack.TryPop ( out ISnapdRestClient? topOfStack ) )
    {
      ISnapdRestClient? clientFromLastTest = PSnapsModuleState.ExchangeSnapdClient ( topOfStack );
      clientFromLastTest?.Dispose ( );
    }
  }

  [OneTimeSetUp]
  public void BeforeAllTests ( )
  {
    // Starting the bottom of the stack with the original client and putting a fresh one in state so disposal during tests is safe.
    Assume.That ( PSnapsModuleState.Client, Is.Not.Null );
    _clientStack.Push ( PSnapsModuleState.Client );

    // ReSharper disable once NotDisposedResource
    PSnapsModuleState.ExchangeSnapdClient ( new SnapdClient ( ) );
    Assume.That ( PSnapsModuleState.Client, Is.InstanceOf<SnapdClient> ( ) );
  }

  [SetUp]
  public void BeforeEachTest ( )
  {
    // Push the current client to the stack if not null, and swap in a new MockSnapdClient.
    MockSnapdClient   mockSnapdClientForTest = new ( );
    ISnapdRestClient? clientFromBeforeTest   = PSnapsModuleState.ExchangeSnapdClient ( mockSnapdClientForTest );

    if ( clientFromBeforeTest is not null )
    {
      _clientStack.Push ( clientFromBeforeTest );
    }
    else
    {
      // If the previous client was null, dispose the new one and fail.
      mockSnapdClientForTest.Dispose ( );
      throw new InvalidOperationException ( $"Module state contained a null client before test run {TestContext.CurrentContext.Test.DisplayName}." );
    }
  }

  [Test]
  [NonParallelizable]
  [RequiresThread]
  public void SwapClient_DisposesOldClient ( )
  {
    ISnapdRestClient? oldClient = PSnapsModuleState.Client;
    ISnapdRestClient  newClient = new MockSnapdClient ( );
    Assume.That ( oldClient, Is.Not.Null );
    Assume.That ( oldClient, Is.SameAs ( PSnapsModuleState.Client ) );
    Assume.That ( newClient, Is.Not.Null );
    Assume.That ( newClient, Is.Not.SameAs ( oldClient ) );

    Assume.That ( ( ) => SetSnapdClientCommand.SwapClient ( newClient ), Throws.Nothing );

    // Call any method other than Dispose() and the exception should be thrown.
    // The Client type is responsible for proving that behavior for all methods in its own tests, so we don't need to be exhaustive here.
    Assert.That ( ( ) => oldClient.GetAllSnapsAsync ( ), Throws.TypeOf<ObjectDisposedException> ( ) );
  }

  [Test]
  [NonParallelizable]
  [RequiresThread]
  public void SwapClient_DoesNotDisposeOldClientWhenToldNotTo ( )
  {
    ISnapdRestClient? oldClient = PSnapsModuleState.Client;
    ISnapdRestClient  newClient = new MockSnapdClient ( );
    Assume.That ( oldClient, Is.Not.Null );
    Assume.That ( oldClient, Is.SameAs ( PSnapsModuleState.Client ) );
    Assume.That ( newClient, Is.Not.Null );
    Assume.That ( newClient, Is.Not.SameAs ( oldClient ) );

    Assume.That ( ( ) => SetSnapdClientCommand.SwapClient ( newClient, true ), Throws.Nothing );

    Assert.That ( ( ) => oldClient.GetAllSnapsAsync ( ), Throws.Nothing );
    Assert.That ( ( ) => oldClient.Dispose ( ),          Throws.Nothing );
  }

  [Test]
  [NonParallelizable]
  [RequiresThread]
  public void SwapClient_SetsNewClient ( )
  {
    ISnapdRestClient? oldClient = PSnapsModuleState.Client;
    ISnapdRestClient  newClient = new MockSnapdClient ( );
    Assume.That ( oldClient, Is.Not.Null );
    Assume.That ( oldClient, Is.SameAs ( PSnapsModuleState.Client ) );
    Assume.That ( newClient, Is.Not.Null );
    Assume.That ( newClient, Is.Not.SameAs ( oldClient ) );

    SetSnapdClientCommand.SwapClient ( newClient );

    using ( Assert.EnterMultipleScope ( ) )
    {
      Assert.That ( PSnapsModuleState.Client, Is.Not.Null );

      Assert.That ( ReferenceEquals ( oldClient, newClient ),                Is.False );
      Assert.That ( ReferenceEquals ( oldClient, PSnapsModuleState.Client ), Is.False );
    }

    Assert.That ( PSnapsModuleState.Client, Is.SameAs ( newClient ) );
  }
}
