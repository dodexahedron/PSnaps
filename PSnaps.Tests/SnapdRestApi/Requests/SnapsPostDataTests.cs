#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.Tests.SnapdRestApi.Requests;

[TestFixture]
[TestOf ( typeof( SnapsPostData ) )]
public class SnapsPostDataTests
{
  [Test]
  public void ConstructorWithAction_SetsAction ( [Values] SnapRequestActionKind actionKind )
  {
    SnapsPostData testData = new ( actionKind );
    Assume.That ( testData,   Is.Not.Null );
    Assume.That ( actionKind, Is.InRange ( 0, 9 ) );

    Assert.That ( testData.Action, Is.EqualTo ( actionKind ) );
  }
}
