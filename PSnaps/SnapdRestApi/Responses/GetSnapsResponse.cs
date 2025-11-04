#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Responses;

using System.Text.Json.Serialization;

/// <summary>
///   A response received as the result of a request to the <c>/v2/snaps</c> endpoint.
/// </summary>
public sealed record GetSnapsResponse
{
  [JsonPropertyName ( "result" )]
  public GetSnapsResponseResult[]? Result { get; set; }

  [JsonPropertyName ( "sources" )]
  public string[]? Sources { get; set; }

  [JsonPropertyName ( "status" )]
  public string? Status { get; set; }

  [JsonPropertyName ( "status-code" )]
  public int StatusCode { get; set; }

  [JsonPropertyName ( "type" )]
  public string? Type { get; set; }
}
