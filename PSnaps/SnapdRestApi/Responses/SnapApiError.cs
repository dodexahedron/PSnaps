#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Responses;

/// <summary>
///   An error response from the snapd API.
/// </summary>
/// <param name="Message">A message describing the error.</param>
/// <param name="Kind">The type of error.</param>
/// <param name="Value">Any additional error information provided in the response.</param>
[PublicAPI]
public sealed record SnapApiError (
  [property: JsonPropertyName ( "message" )]
  string Message,
  [property: JsonPropertyName ( "kind" )]
  string? Kind = null,
  [property: JsonPropertyName ( "value" )]
  object? Value = null
);
