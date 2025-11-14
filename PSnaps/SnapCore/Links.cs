#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

#pragma warning disable CS1591

namespace PSnaps.SnapCore;

using System.Collections.Generic;
using System.Text.Json.Serialization;

/// <summary>
///   Corresponds to the links property of snap packages, in the snapd API.
/// </summary>
[PublicAPI]
public sealed class Links : IEqualityOperators<Links, Links, bool>, IEquatable<Links>
{
  [JsonPropertyName ( "contact" )]
  public string[]? Contact { get; set; }

  [JsonPropertyName ( "issues" )]
  public string[]? Issues { get; set; }

  [JsonPropertyName ( "source" )]
  public string[]? Source { get; set; }

  [JsonPropertyName ( "source-code" )]
  public string[]? Sourcecode { get; set; }

  [JsonPropertyName ( "website" )]
  public string[]? Website { get; set; }
}
