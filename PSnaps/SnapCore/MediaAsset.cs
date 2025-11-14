#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace PSnaps.SnapCore;

[PublicAPI]
public class MediaAsset
{
  [JsonPropertyName ( "height" )]
  public int? Height { get; set; }

  [JsonPropertyName ( "type" )]
  public required MediaAssetKind Type { get; set; }

  [JsonPropertyName ( "url" )]
  public required string Url { get; set; }

  [JsonPropertyName ( "width" )]
  public int? Width { get; set; }
}
