#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Responses;

/// <summary>
///   An async API response.
/// </summary>
/// <param name="Change">
///   The change ID, for retrieval of the progress and outcome of the operation.<br />
///   This is always a base-10 number as a string.
/// </param>
/// <remarks>
///   Async responses always contain a <see cref="Change" /> identifier, which can be used to retrieve the outcome of the operation
///   as a <see cref="ChangeSet" /> via the <c>changes/{id}</c> API endpoint.
/// </remarks>
[PublicAPI]
public sealed record SnapApiAsyncResponse (
  [property: JsonPropertyName ( "change" )]
  string Change
) : SnapApiResponse, IEqualityOperators<SnapApiAsyncResponse, SnapApiAsyncResponse, bool>;
