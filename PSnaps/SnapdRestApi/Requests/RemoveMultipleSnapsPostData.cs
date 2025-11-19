#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Requests;

/// <summary>
///   Post data object used when calling the /v2/snaps endpoint with a "remove" action and multiple snaps.
/// </summary>
/// <param name="Snaps">An array of snap package names to remove.</param>
/// <param name="Purge">Whether to include the purge parameter in the request.</param>
[PublicAPI]
[ExcludeFromCodeCoverage ( Justification = "Nothing to test." )]
public sealed record RemoveMultipleSnapsPostData (
  [property: JsonPropertyName ( "snaps" )]
  string[] Snaps,
  [property: JsonPropertyName ( "purge" )]
  bool Purge = false
) : SnapsPostData ( SnapRequestActionKind.Remove );
