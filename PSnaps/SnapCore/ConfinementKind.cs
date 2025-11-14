#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

#pragma warning disable CS1591
namespace PSnaps.SnapCore;

[PublicAPI]
public enum ConfinementKind
{
  [JsonStringEnumMemberName ( "none" )]
  None,

  [JsonStringEnumMemberName ( "partial" )]
  Partial,

  [JsonStringEnumMemberName ( "strict" )]
  Strict,

  [JsonStringEnumMemberName ( "classic" )]
  Classic,

  [JsonStringEnumMemberName ( "devmode" )]
  DevMode
}
