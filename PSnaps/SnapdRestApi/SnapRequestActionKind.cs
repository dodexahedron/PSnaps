#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi;

using System.Text.Json.Serialization;

/// <summary>
///   Enumeration of actions valid for tasks operating on snap packages, such as installing and removing.
/// </summary>
public enum SnapRequestActionKind
{
  [JsonStringEnumMemberName ( "install" )]
  Install,

  [JsonStringEnumMemberName ( "refresh" )]
  Refresh,

  [JsonStringEnumMemberName ( "revert" )]
  Revert,

  [JsonStringEnumMemberName ( "remove" )]
  Remove,

  [JsonStringEnumMemberName ( "hold" )]
  Hold,

  [JsonStringEnumMemberName ( "unhold" )]
  Unhold,

  [JsonStringEnumMemberName ( "enable" )]
  Enable,

  [JsonStringEnumMemberName ( "disable" )]
  Disable,

  [JsonStringEnumMemberName ( "switch" )]
  Switch,

  [JsonStringEnumMemberName ( "snapshot" )]
  Snapshot
}
