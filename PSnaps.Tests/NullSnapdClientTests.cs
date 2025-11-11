#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using PSnaps.SnapdRestApi.Clients;

namespace PSnaps.Tests;

[TestFixture]
[TestOf ( typeof( NullSnapdClient ) )]
[Category ( "Snapd Integration" )]
public class NullSnapdClientTests
{
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
  public async Task InstallMultipleSnapsAsync_ReturnsNull ( )
  {
    using NullSnapdClient client = new ( );
    Assert.That ( await client.InstallMultipleSnapsAsync ( [ "core22", "snapd" ] ), Is.Null );
  }

  [Test]
  public async Task RemoveSnapAsync_ReturnsNull ( [Values] bool purge )
  {
    using NullSnapdClient client = new ( );
    Assert.That ( await client.RemoveSnapAsync ( "core22", "226", purge ), Is.Null );
  }
}
