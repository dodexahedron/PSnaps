#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using PSnaps.SnapdRestApi.Responses;

namespace PSnaps.SnapCore;

/// <summary>
///   Represents a Snap package.
/// </summary>
[PublicAPI]
[JsonPolymorphic ( TypeDiscriminatorPropertyName = "type" )]
[JsonDerivedType ( typeof( BaseSnap ),  "base" )]
[JsonDerivedType ( typeof( AppSnap ),   "app" )]
[JsonDerivedType ( typeof( SnapdSnap ), "snapd" )]
public record SnapPackage : IHaveStatus<PackageStatus>, IComparable<SnapPackage>, IComparable, IComparisonOperators<SnapPackage, SnapPackage, bool>
{
  [JsonPropertyName ( "channel" )]
  public required string Channel { get; set; }

  [JsonPropertyName ( "common-ids" )]
  public string[]? CommonIds { get; set; }

  [JsonPropertyName ( "components" )]
  public Component[]? Components { get; set; }

  [JsonPropertyName ( "confinement" )]
  public Confinement Confinement { get; set; }

  [JsonPropertyName ( "contact" )]
  public string? Contact { get; set; }

  [JsonPropertyName ( "description" )]
  public required string Description { get; set; }

  [JsonPropertyName ( "developer" )]
  public string? Developer { get; set; }

  // ReSharper disable once StringLiteralTypo
  [JsonPropertyName ( "devmode" )]
  public bool DevMode { get; set; }

  [JsonPropertyName ( "grade" )]
  public string? Grade { get; set; }

  [JsonPropertyName ( "icon" )]
  public string? Icon { get; set; }

  [JsonPropertyName ( "id" )]
  public required string Id { get; set; }

  [JsonPropertyName ( "ignore-validation" )]
  public bool IgnoreValidation { get; set; }

  [JsonPropertyName ( "install-date" )]
  public DateTimeOffset InstallDate { get; set; }

  [JsonPropertyName ( "installed-size" )]
  public long InstalledSize { get; set; }

  // ReSharper disable once StringLiteralTypo
  [JsonPropertyName ( "jailmode" )]
  public bool JailMode { get; set; }

  [JsonPropertyName ( "license" )]
  public string? License { get; set; }

  [JsonPropertyName ( "links" )]
  public Links? Links { get; set; }

  [JsonPropertyName ( "media" )]
  public MediaAsset[]? Media { get; set; }

  [JsonPropertyName ( "mounted-from" )]
  public string? MountedFrom { get; set; }

  [JsonPropertyName ( "name" )]
  public required string Name { get; set; }

  [JsonPropertyName ( "private" )]
  public bool Private { get; set; }

  [JsonPropertyName ( "publisher" )]
  public Publisher? Publisher { get; set; }

  [JsonPropertyName ( "revision" )]
  public required string Revision { get; set; }

  [JsonPropertyName ( "summary" )]
  public string? Summary { get; set; }

  [JsonPropertyName ( "title" )]
  public string? Title { get; set; }

  [JsonPropertyName ( "tracking-channel" )]
  public required string TrackingChannel { get; set; }

  [JsonPropertyName ( "version" )]
  public required string Version { get; set; }

  [JsonPropertyName ( "website" )]
  public string? Website { get; set; }

  /// <inheritdoc />
  public int CompareTo ( object? other )
  {
    return other is SnapPackage otherResult ? CompareTo ( otherResult ) : throw new ArgumentException ( $"Object must be of type {nameof (SnapPackage)}" );
  }

  /// <inheritdoc />
  public int CompareTo ( SnapPackage? other )
  {
    if ( ReferenceEquals ( this, other ) )
    {
      return 0;
    }

    if ( other is null )
    {
      return 1;
    }

    int nameComparison = string.Compare ( Name, other.Name, StringComparison.Ordinal );

    if ( nameComparison != 0 )
    {
      return nameComparison;
    }

    int trackingChannelComparison = string.Compare ( TrackingChannel, other.TrackingChannel, StringComparison.Ordinal );

    return trackingChannelComparison != 0 ? trackingChannelComparison : string.Compare ( Revision, other.Revision, StringComparison.Ordinal );
  }

  public static bool operator > ( SnapPackage? left, SnapPackage? right )
  {
    return Comparer<SnapPackage>.Default.Compare ( left, right ) > 0;
  }

  public static bool operator >= ( SnapPackage? left, SnapPackage? right )
  {
    return Comparer<SnapPackage>.Default.Compare ( left, right ) >= 0;
  }

  public static bool operator < ( SnapPackage? left, SnapPackage? right )
  {
    return Comparer<SnapPackage>.Default.Compare ( left, right ) < 0;
  }

  public static bool operator <= ( SnapPackage? left, SnapPackage? right )
  {
    return Comparer<SnapPackage>.Default.Compare ( left, right ) <= 0;
  }

  [JsonPropertyName ( "status" )]
  public PackageStatus Status { get; set; }
}