#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Requests;

public sealed record InstallMultipleSnapsPostData (
  [property: JsonPropertyName ( "snaps" )]
  string[] Snaps,
  [property: JsonPropertyName ( "transaction" )]
  TransactionMode Transaction = TransactionMode.PerPackage,
  [property: JsonPropertyName ( "system-restart-immediate" )]
  bool SystemRestartImmediate = false
) : SnapsPostData ( SnapRequestActionKind.Install );
