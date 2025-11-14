#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapCore;

/// <summary>
///   An enum of possible values for the <see cref="Component.Type" /> property.
/// </summary>
[PublicAPI]
public enum ComponentKind
{
  /// <summary>
  ///   Corresponds to the value "standard" in the snapd API.
  /// </summary>
  [JsonStringEnumMemberName ( "standard" )]
  Standard,

  /// <summary>
  ///   Corresponds to the value "kernel-modules" in the snapd API.
  /// </summary>
  [JsonStringEnumMemberName ( "kernel-modules" )]
  KernelModules
}
