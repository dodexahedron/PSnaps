#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using System.Globalization;
using System.Text.RegularExpressions;

namespace PSnaps.Cmdlets;

public sealed partial record SnapToRemove : IParsable<SnapToRemove>
{
  private readonly string _revision;

  private readonly string _version;

  [SetsRequiredMembers]
  public SnapToRemove ( string Name, string Version = "*", string Revision = "*" )
  {
    ArgumentException.ThrowIfNullOrWhiteSpace ( Name );
    this.Name     = Name;
    this.Version  = Version;
    this.Revision = Revision;
  }

  public required string Name { get; init; }

  public required string Revision
  {
    get => _revision;
    [MemberNotNull ( nameof (_revision) )]
    init => _revision = string.IsNullOrWhiteSpace ( value ) ? "*" : value;
  }

  public required string Version
  {
    get => _version;
    [MemberNotNull ( nameof (_version) )]
    init => _version = string.IsNullOrWhiteSpace ( value ) ? "*" : value;
  }

  [GeneratedRegex ( @"(?<Name>^[^\s/\\]{2,})(/(?<Version>[^\s/\\]+)(/(?<Revision>(\d+)|(\*)))?)?$" )]
  private static partial Regex ParseRegex { get; }

  /// <inheritdoc />
  public static SnapToRemove Parse ( string? s, IFormatProvider? provider = null )
  {
    ArgumentException.ThrowIfNullOrWhiteSpace ( s );

    if ( ParseRegex.Matches ( s ) is not { Count: > 0 } matches )
    {
      throw new FormatException ( "Snap format not recognized. Must be of the form name[/version[/revision]]" );
    }

    Match longestMatch = matches.MaxBy ( static match => match.Length ) ?? matches [ 0 ];
    return new (
                longestMatch.Groups [ "Name" ].Value,
                longestMatch.Groups [ "Version" ].Success ? longestMatch.Groups [ "Version" ].Value : "*",
                longestMatch.Groups [ "Revision" ].Success ? longestMatch.Groups [ "Revision" ].Value : "*"
               );
  }

  /// <inheritdoc />
  public static bool TryParse ( [NotNullWhen ( true )] string? s, IFormatProvider? provider, [NotNullWhen ( true )] out SnapToRemove? result )
  {
    try
    {
      result = Parse ( s, provider ?? CultureInfo.InvariantCulture );
      return true;
    }
    catch ( FormatException )
    {
      result = null;
      return false;
    }
  }

  [SuppressMessage ( "ReSharper", "InconsistentNaming",   Justification = "It's a deconstructor..." )]
  [SuppressMessage ( "ReSharper", "ParameterHidesMember", Justification = "It's a deconstructor..." )]
  public void Deconstruct ( out string Name, out string Version, out string Revision )
  {
    Name     = this.Name;
    Version  = this.Version;
    Revision = this.Revision;
  }

  public static implicit operator string ( SnapToRemove snap )
  {
    return string.Create (
                          snap.Name.Length + snap.Version.Length + snap.Revision.Length + 2,
                          snap,
                          static ( span, theSnap ) =>
                          {
                            int position      = 0;
                            int versionLength = theSnap.Version.Length;

                            span [ theSnap.Name.Length ] = span [ theSnap.Name.Length + 1 + versionLength ] = '/';
                            theSnap.Name.AsSpan ( ).CopyTo ( span [ position..( position      += theSnap.Name.Length ) ] );
                            theSnap.Version.AsSpan ( ).CopyTo ( span [ ++position..( position += versionLength ) ] );
                            theSnap.Revision.AsSpan ( ).CopyTo ( span [ ++position.. ] );
                          }
                         );
  }

  /// <inheritdoc />
  public override string ToString ( )
  {
    return this;
  }
}
