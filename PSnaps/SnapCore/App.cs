#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapCore;

using System.Text.Json.Serialization;

/// <summary>
///   Basic metadata about a snap "App."
/// </summary>
[PublicAPI]
public sealed record App
{
  [JsonPropertyName ( "daemon" )]
  public DaemonKind Daemon { get; set; }

  [JsonPropertyName ( "daemon-scope" )]
  public string? DaemonScope { get; set; }

  [JsonPropertyName ( "desktop-file" )]
  public string? DesktopFile { get; set; }

  [JsonPropertyName ( "enabled" )]
  public bool Enabled { get; set; }

  [JsonPropertyName ( "name" )]
  public required string Name { get; set; }

  [JsonPropertyName ( "snap" )]
  public string? Snap { get; set; }
}
