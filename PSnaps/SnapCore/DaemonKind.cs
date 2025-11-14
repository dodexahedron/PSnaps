#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace PSnaps.SnapCore;

[PublicAPI]
public enum DaemonKind
{
  [JsonStringEnumMemberName ( "simple" )]
  Simple = 0,

  [JsonStringEnumMemberName ( "forking" )]
  Forking = 1,

  [JsonStringEnumMemberName ( "oneshot" )]
  OneShot = 2,

  [JsonStringEnumMemberName ( "dbus" )]
  DBus = 3,

  [JsonStringEnumMemberName ( "notify" )]
  Notify = 4
}
