#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Requests;

/// <summary>
///   Represents the data required to install multiple snaps in a single request.
/// </summary>
/// <remarks>
///   This record is used to encapsulate the details of a request to install multiple snaps, including the
///   list of snap package names, the transaction mode, and whether an immediate system restart is respected after the
///   installation.
/// </remarks>
/// <param name="Snaps">An array of one or more snap package names to install.</param>
/// <param name="Transaction">The transaction mode for the operation.</param>
/// <param name="SystemRestartImmediate">If a restart is required, specifies whether to perform the restart or not.</param>
[UsedImplicitly ( ImplicitUseTargetFlags.WithMembers )]
public sealed record InstallMultipleSnapsPostData (
  [property: JsonPropertyName ( "snaps" )]
  string[] Snaps,
  [property: JsonPropertyName ( "transaction" )]
  TransactionMode Transaction = TransactionMode.PerPackage,
  [property: JsonPropertyName ( "system-restart-immediate" )]
  bool SystemRestartImmediate = false
) : SnapsPostData ( SnapRequestActionKind.Install );
