#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapCore;

/// <summary>
///   Represents a Snap package.
/// </summary>
[PublicAPI]
[JsonPolymorphic ( TypeDiscriminatorPropertyName = "type", UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor, IgnoreUnrecognizedTypeDiscriminators = true )]
[JsonDerivedType ( typeof( BaseSnap ),  "base" )]
[JsonDerivedType ( typeof( AppSnap ),   "app" )]
[JsonDerivedType ( typeof( SnapdSnap ), "snapd" )]
public record SnapPackage : IComparable<SnapPackage>, IComparable, IComparisonOperators<SnapPackage, SnapPackage, bool>
{
  [JsonPropertyName ( "channel" )]
  public required string Channel { get; set; }

  [JsonPropertyName ( "common-ids" )]
  public string[]? CommonIds { get; set; }

  [JsonPropertyName ( "components" )]
  public Component[]? Components { get; set; }

  [JsonPropertyName ( "confinement" )]
  public ConfinementKind? Confinement { get; set; }

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

  /// <summary>
  ///   Gets the idempotent identifier for the <see cref="SnapPackage" /> instance.
  /// </summary>
  /// <remarks>
  ///   This property is used by <see cref="GetHashCode" /> and MUST be unique in collections keyed on instances of
  ///   <see cref="SnapPackage" />.
  /// </remarks>
  [JsonPropertyName ( "id" )]
  public required string Id { get; init; }

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

  [JsonPropertyName ( "status" )]
  public PackageStatus Status { get; set; }

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
  /// <remarks>
  ///   <para>
  ///     Sorting order is as follows:<br />
  ///     1. Reference equality immediately returns 0.<br />
  ///     2. Other object <see langword="null" /> immediately returns 1 (<see langword="null" /> comes first).<br />
  ///     3. <see cref="Name" /><br />
  ///     4. <see cref="TrackingChannel" /><br />
  ///     5. <see cref="Version" /><br />
  ///     6. <see cref="Revision" /><br />
  ///     7. <see cref="InstallDate" />
  ///   </para>
  ///   <para>
  ///     All string property comparisons are performed as ordinal and case-insensitive.<br />
  ///     Date comparison is by DateTimeOffset value.
  ///   </para>
  /// </remarks>
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

    int comparisonResult = string.Compare ( Name, other.Name, StringComparison.OrdinalIgnoreCase );

    if ( comparisonResult != 0 )
    {
      return comparisonResult;
    }

    comparisonResult = string.Compare ( TrackingChannel, other.TrackingChannel, StringComparison.OrdinalIgnoreCase );

    if ( comparisonResult != 0 )
    {
      return comparisonResult;
    }

    comparisonResult = string.Compare ( Version, other.Version, StringComparison.OrdinalIgnoreCase );

    if ( comparisonResult != 0 )
    {
      return comparisonResult;
    }

    comparisonResult = string.Compare ( Revision, other.Revision, StringComparison.OrdinalIgnoreCase );

    if ( comparisonResult != 0 )
    {
      return comparisonResult;
    }

    return DateTimeOffset.Compare ( InstallDate, other.InstallDate );
  }

  /// <inheritdoc />
  public static bool operator > ( SnapPackage? left, SnapPackage? right )
  {
    return Compare ( left, right ) > 0;
  }

  /// <inheritdoc />
  public static bool operator >= ( SnapPackage? left, SnapPackage? right )
  {
    return Compare ( left, right ) >= 0;
  }

  /// <inheritdoc />
  public static bool operator < ( SnapPackage? left, SnapPackage? right )
  {
    return Compare ( left, right ) < 0;
  }

  /// <inheritdoc />
  public static bool operator <= ( SnapPackage? left, SnapPackage? right )
  {
    return Compare ( left, right ) <= 0;
  }

  /// <inheritdoc cref="IComparer{T}.Compare(T?, T?)" />
  public static int Compare ( SnapPackage? left, SnapPackage? right )
  {
    return ( left, right ) switch
           {
             (not null, _)    => left.CompareTo ( right ),
             (null, null)     => 0,
             (null, not null) => -1
           };
  }

  /// <inheritdoc />
  /// <remarks>
  ///   The <see cref="Id" /> of the <see cref="SnapPackage" /> is the basis of the hash code, as it is idempotent.
  /// </remarks>
  public override int GetHashCode ( )
  {
    return Id.GetHashCode ( );
  }
}
