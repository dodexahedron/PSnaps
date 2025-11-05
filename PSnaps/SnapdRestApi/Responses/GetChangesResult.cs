#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Responses;

[PublicAPI]
public class GetChangesResult : ApiResponseResult
{
  [JsonPropertyName ( "id" )]
  public required string Id { get; set; }

  [JsonPropertyName ( "ready" )]
  public bool Ready { get; set; }

  [JsonPropertyName ( "ready-time" )]
  public DateTimeOffset ReadyTime { get; set; }

  [JsonPropertyName ( "spawn-time" )]
  public DateTimeOffset SpawnTime { get; set; }

  [JsonPropertyName ( "summary" )]
  public string? Summary { get; set; }

  public ApiTask[]? Tasks { get; set; }
}
