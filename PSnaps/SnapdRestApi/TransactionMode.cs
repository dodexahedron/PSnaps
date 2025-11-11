#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi;

[PublicAPI]
public enum TransactionMode
{
  /// <summary>
  ///   Performs the operation transactionally on a per-package basis. All package operations will individually either succeed or be
  ///   rolled back on failure, without impacting other packages in the operation.
  /// </summary>
  [JsonStringEnumMemberName ( "per-package" )]
  PerPackage,

  /// <summary>
  ///   Performs the operations transactionally on an all-or-nothing basis. All package operations must succeed or all operations will
  ///   be rolled back and any remaining that have not yet started will be aborted.
  /// </summary>
  [JsonStringEnumMemberName ( "all-package" )]
  AllPackage
}
