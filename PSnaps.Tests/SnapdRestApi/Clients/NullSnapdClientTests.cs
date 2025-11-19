#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.Tests.SnapdRestApi.Clients;

[TestFixture]
[TestOf ( typeof( NullSnapdClient ) )]
[Category ( "Snapd Integration" )]
public class NullSnapdClientTests
{
  private static string[][] SnapNameArrays => [ [ "core22" ], [ "core22", "snapd" ] ];

  [Test]
  public async Task GetAllSnaps_ReturnsNull ( )
  {
    using NullSnapdClient client = new ( );
    Assert.That ( await client.GetAllSnapsAsync ( ), Is.Null );
  }

  [Test]
  public async Task GetChangesAsync_ReturnsNull ( )
  {
    using NullSnapdClient client = new ( );
    Assert.That ( await client.GetChangesAsync ( "1" ), Is.Null );
  }

  [Test]
  public async Task GetSnapAsync_ReturnsNull ( )
  {
    using NullSnapdClient client = new ( );
    Assert.That ( await client.GetSingleSnapAsync ( "core22" ), Is.Null );
  }

  [Test]
  public async Task GetSnapsAsync_ReturnsNull ( [ValueSource ( nameof (SnapNameArrays) )] string[] snaps, [Values] bool includeInactive )
  {
    using NullSnapdClient client = new ( );
    Assert.That ( await client.GetSnapsAsync ( snaps, includeInactive ), Is.Null );
  }

  [Test]
  public async Task InstallMultipleSnapsAsync_ReturnsNull ( [ValueSource ( nameof (SnapNameArrays) )] string[] snaps )
  {
    using NullSnapdClient client = new ( );
    Assert.That ( await client.InstallMultipleSnapsAsync ( snaps ), Is.Null );
  }

  [Test]
  public async Task RemoveMultipleSnapsAsync_ReturnsNull ( [ValueSource ( nameof (SnapNameArrays) )] string[] snaps, [Values] bool purge )
  {
    using NullSnapdClient client = new ( );
    Assert.That ( await client.RemoveMultipleSnapsAsync ( snaps, purge ), Is.Null );
  }

  [Test]
  public async Task RemoveSnapAsync_ReturnsNull ( [Values] bool purge )
  {
    using NullSnapdClient client = new ( );
    Assert.That ( await client.RemoveSnapAsync ( "core22", "226", purge ), Is.Null );
  }

  [Test]
  public void SnapdUnixSocketUri_ReturnsNull ( )
  {
    using NullSnapdClient client = new ( );
    Assert.That ( client.SnapdUnixSocketUri, Is.Null );
  }
}
