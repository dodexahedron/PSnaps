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
  public string[]? Contact { get; init; }

  [JsonPropertyName ( "issues" )]
  public string[]? Issues { get; init; }

  [JsonPropertyName ( "source" )]
  public string[]? Source { get; init; }

  [JsonPropertyName ( "source-code" )]
  public string[]? Sourcecode { get; init; }

  [JsonPropertyName ( "website" )]
  public string[]? Website { get; init; }

  /// <inheritdoc />
  public static bool operator == ( Links? left, Links? right )
  {
    return EqualityComparer<Links>.Default.Equals ( left, right );
  }

  /// <inheritdoc />
  public static bool operator != ( Links? left, Links? right )
  {
    return !( left == right );
  }

  /// <inheritdoc />
  public bool Equals ( Links? other )
  {
    return other is not null && EqualityComparer<string[]?>.Default.Equals ( Contact, other.Contact ) && EqualityComparer<string[]?>.Default.Equals ( Issues, other.Issues ) && EqualityComparer<string[]?>.Default.Equals ( Source, other.Source ) && EqualityComparer<string[]?>.Default.Equals ( Sourcecode, other.Sourcecode ) && EqualityComparer<string[]?>.Default.Equals ( Website, other.Website );
  }

  /// <inheritdoc />
  public override bool Equals ( object? obj )
  {
    return Equals ( obj as Links );
  }

  /// <inheritdoc />
  public override int GetHashCode ( )
  {
    return HashCode.Combine ( Contact, Issues, Source, Sourcecode, Website );
  }
}
