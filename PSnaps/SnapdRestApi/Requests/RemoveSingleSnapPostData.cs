#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Requests;

/// <summary>
///   POST data used when removing a single snap package.
/// </summary>
/// <param name="Name">The name of the snap being removed.</param>
/// <param name="Revision">The revision of the snap being removed.</param>
/// <param name="Purge">Whether to include the purge parameter.</param>
[PublicAPI]
public sealed record RemoveSingleSnapPostData (
  [property: JsonIgnore] string? Name,
  [property: JsonPropertyName ( "revision" )]
  string Revision,
  [property: JsonPropertyName ( "purge" )]
  bool Purge = false
) : SnapsPostData ( SnapRequestActionKind.Remove );
