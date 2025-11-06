#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Responses;

/// <summary>
///   Top-level class for all API responses.
/// </summary>
/// <remarks>
///   <para>
///     Responses can be of three types: async, sync, or error.<br />
///     Sync responses are typically returned from GET operations.<br />
///     Async responses are typically retuned from POST operations.<br />
///     Error responses are typically returned if something is wrong with the request itself or if an operation failed immediately
///     when called.
///   </para>
///   <para>
///     This base type has no members except for the <c>type</c>, which is hidden by the source generator and used to determine which
///     of the three types of responses something is.
///   </para>
/// </remarks>
[JsonPolymorphic ( TypeDiscriminatorPropertyName = "type" )]
[JsonDerivedType ( typeof( SnapApiSyncResponse ),  "sync" )]
[JsonDerivedType ( typeof( SnapApiAsyncResponse ), "async" )]
[JsonDerivedType ( typeof( SnapApiErrorResponse ), "error" )]
public record SnapApiResponse : IHaveStatus<int, string>
{
  /// <summary>
  ///   A status code. Generally corresponds to the HTTP status code of the response.
  /// </summary>
  [JsonPropertyName ( "status-code" )]
  public required int StatusCode { get; set; }

  /// <summary>
  ///   Textual status description.
  /// </summary>
  [JsonPropertyName ( "status" )]
  public required string Status { get; set; }

  [JsonPropertyName ( "maintenance" )]
  public MaintenanceInfo? Maintenance { get; set; }

  [PublicAPI]
  public sealed record MaintenanceInfo
  {
    [JsonPropertyName ( "message" )]
    public required string Message { get; set; }

    [JsonPropertyName ( "kind" )]
    public required string Kind { get; set; }
  }
}
