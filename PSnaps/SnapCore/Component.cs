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
public sealed record Component
{
  [JsonPropertyName ( "description" )]
  public required string Description { get; set; }

  [JsonPropertyName ( "name" )]
  public required string Name { get; set; }

  [JsonPropertyName ( "revision" )]
  public required string Revision { get; set; }

  [JsonPropertyName ( "summary" )]
  public required string Summary { get; set; }

  [JsonPropertyName ( "type" )]
  public ComponentKind Type { get; set; }

  [JsonPropertyName ( "version" )]
  public required string Version { get; set; }
}
