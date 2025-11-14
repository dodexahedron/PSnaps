#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace PSnaps.SnapdRestApi;

/// <summary>
///   Enumeration of actions valid for tasks operating on snap packages, such as installing and removing.
/// </summary>
[PublicAPI]
public enum SnapRequestActionKind
{
  [JsonStringEnumMemberName ( "install" )]
  Install,

  ///<remarks>Not currently used by PSnaps.</remarks>
  [JsonStringEnumMemberName ( "refresh" )]
  Refresh,

  ///<remarks>Not currently used by PSnaps.</remarks>
  [JsonStringEnumMemberName ( "revert" )]
  Revert,

  [JsonStringEnumMemberName ( "remove" )]
  Remove,

  ///<remarks>Not currently used by PSnaps.</remarks>
  [JsonStringEnumMemberName ( "hold" )]
  Hold,

  ///<remarks>Not currently used by PSnaps.</remarks>
  [JsonStringEnumMemberName ( "unhold" )]
  Unhold,

  ///<remarks>Not currently used by PSnaps.</remarks>
  [JsonStringEnumMemberName ( "enable" )]
  Enable,

  ///<remarks>Not currently used by PSnaps.</remarks>
  [JsonStringEnumMemberName ( "disable" )]
  Disable,

  ///<remarks>Not currently used by PSnaps.</remarks>
  [JsonStringEnumMemberName ( "switch" )]
  Switch,

  ///<remarks>Not currently used by PSnaps.</remarks>
  [JsonStringEnumMemberName ( "snapshot" )]
  Snapshot
}
