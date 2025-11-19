#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Requests;

/// <summary>
///   Parameters used for POST operations to the /v2/snaps endpoint.
/// </summary>
[PublicAPI]
public record SnapsPostData
{
  /// <summary>
  ///   Creates a default instance of <see cref="SnapsPostData" />.
  /// </summary>
  [ExcludeFromCodeCoverage ( Justification = "Nothing to test." )]
  public SnapsPostData ( )
  {
  }

  /// <summary>
  ///   Creates an instance of <see cref="SnapsPostData" /> with the specified <see cref="Action" /> type.
  /// </summary>
  /// <param name="action">The <see cref="SnapRequestActionKind" /> of the request.</param>
  public SnapsPostData ( SnapRequestActionKind action )
  {
    Action = action;
  }

  /// <summary>
  ///   The <see cref="SnapRequestActionKind" /> of the operation.
  /// </summary>
  /// <remarks>
  ///   Ideally, derived types should set this in their call to base()
  /// </remarks>
  [JsonPropertyName ( "action" )]
  public SnapRequestActionKind Action { get; }
}
