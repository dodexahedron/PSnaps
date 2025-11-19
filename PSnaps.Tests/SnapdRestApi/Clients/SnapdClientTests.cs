#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.Tests.SnapdRestApi.Clients;

[TestFixture]
[TestOf ( typeof( SnapdClient ) )]
[Category ( "Snapd Integration" )]
public class SnapdClientTests
{
  [Test]
#pragma warning disable CA1416
  public void Dispose_CanBeCalledMultipleTimesWithoutError ( [Values ( 2, 10 )] int timesCalled )
  {
    SnapdClient client = new ( );

    IDisposable disposeLoopScope = Assert.EnterMultipleScope ( );

    for ( ; timesCalled > 0; timesCalled-- )
    {
      Assert.That ( ( ) => client.Dispose ( ), Throws.Nothing );
    }

    disposeLoopScope.Dispose ( );
#pragma warning restore CA1416
  }
}
