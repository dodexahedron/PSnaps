#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapCore;

public record Component
{
  public required string        Name        { get; set; }
  public required string        Description { get; set; }
  public required string        Summary     { get; set; }
  public required string        Version     { get; set; }
  public required string        Revision    { get; set; }
  public          ComponentKind Type        { get; set; }
}
