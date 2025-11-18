#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Responses;

/// <summary>
///   The type of response received from snapd when an operation fails.
/// </summary>
/// <param name="Result"></param>
public sealed record SnapApiErrorResponse (
  [property: JsonPropertyName ( "result" )]
  SnapApiError Result
) : SnapApiResponse, IEqualityOperators<SnapApiErrorResponse, SnapApiErrorResponse, bool>;
