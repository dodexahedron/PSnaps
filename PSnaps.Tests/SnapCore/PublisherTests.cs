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
[TestOf ( typeof( Publisher ) )]
[Parallelizable ( ParallelScope.All )]
public class PublisherTests
{
  [Test ( ExpectedResult = false )]
  public bool Verified_FalseWhenNotVerified ( )
  {
    return new Publisher { DisplayName = "Verified Publisher", Validation = "something other than verified" }.Verified;
  }

  [Test ( ExpectedResult = true )]
  public bool Verified_TrueWhenVerified ( )
  {
    return new Publisher { DisplayName = "Verified Publisher", Validation = "verified" }.Verified;
  }
}
