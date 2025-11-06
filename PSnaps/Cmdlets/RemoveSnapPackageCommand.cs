#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using System.Collections.Immutable;
using System.Globalization;
using System.Management.Automation;
using System.Text.RegularExpressions;
using PSnaps.SnapdRestApi.Responses;

namespace PSnaps.Cmdlets;

[PublicAPI]
[Cmdlet ( VerbsCommon.Remove, "SnapPackage", ConfirmImpact = ConfirmImpact.Medium, DefaultParameterSetName = NamedSnapsParameterSet )]
[Alias ( "Remove-Snap", "Remove-Snaps", "Remove-SnapPackages" )]
[OutputType ( typeof( IEnumerable<SnapApiResponse> ) )]
public class RemoveSnapPackageCommand : PSCmdlet
{
  private const string AllSnapsParameterSet   = "AllSnaps";
  private const string NamedSnapsParameterSet = "SingleSnap";

  [Parameter ( Mandatory = true, ParameterSetName = AllSnapsParameterSet )]
  public SwitchParameter All { get; set; }

  [Parameter ( Mandatory = false, ParameterSetName = NamedSnapsParameterSet )]
  [Parameter ( Mandatory = false, ParameterSetName = AllSnapsParameterSet )]
  public SwitchParameter Disabled { get; set; }

  [Parameter ( Mandatory = false, ParameterSetName = NamedSnapsParameterSet )]
  [Parameter ( Mandatory = false, ParameterSetName = AllSnapsParameterSet )]
  public SwitchParameter Purge { get; set; }

  [Parameter ( Mandatory = true, Position = 1, ParameterSetName = NamedSnapsParameterSet )]
  [ValidateRange ( ValidateRangeKind.NonNegative )]
  public int Revision { get; set; }

  [Parameter ( Mandatory = true, Position = 0, ParameterSetName = NamedSnapsParameterSet )]
  public string[]? Snaps { get; set; }

  [Parameter ( Mandatory = false, DontShow = true )]
  [ValidateRange ( ValidateRangeKind.Positive )]
  public int Timeout { get; set; } = 30000;

  protected override void ProcessRecord ( )
  {
    ArgumentException.ThrowIfNullOrWhiteSpace ( Snaps [ 0 ] );
    CancellationTokenSource cts    = new ( Timeout );
    using SnapdClient       client = new ( );

    switch ( ParameterSetName )
    {
      case NamedSnapsParameterSet:
      {
        if ( Disabled.IsPresent )
        {
          // Remove disabled instances of the package only (those with status=installed).
          SnapPackage[]? allRevisions =
            client
             .GetSnapAsync ( Snaps [ 0 ], true, 5000, cts.Token )
             .GetAwaiter ( )
             .GetResult ( );

          if ( allRevisions is not { Length: > 1 } )
          {
            // There was only the one result, so respond with an empty array.
            WriteObject ( ImmutableArray<SnapApiAsyncResponse>.Empty );
            return;
          }

          WriteObject (
                       ImmutableArray
                        .Create (
                                 allRevisions
                                  .Where ( static package => package.Status == PackageStatus.Installed )
                                   // ReSharper disable once AccessToDisposedClosure
                                  .Select ( package => new RemoveSnapInfo ( client, package.Name, package.Revision.ToString ( NumberFormatInfo.InvariantInfo ), Purge.IsPresent, cts.Token ) )
                                  .Select ( RemoveSingleSnap )
                                )
                      );
          GC.KeepAlive ( client );
          return;
        }

        //ArgumentException.ThrowIfNullOrWhiteSpace ( Snaps );
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero ( Revision );
        throw new NotImplementedException("This functionality not yet complete.");
        //WriteObject ( ImmutableArray.Create ( RemoveSingleSnap ( new ( client, Snaps, Revision.ToString ( NumberFormatInfo.InvariantInfo ), Purge.IsPresent, cts.Token ) ) ) );
      }
        return;

      case AllSnapsParameterSet:
      {
        // TODO: Handle the -All parameter.
        throw new NotImplementedException ( "The -All parameter is not yet implemented." );
      }
    }
  }

  private static SnapApiResponse? RemoveSingleSnap ( RemoveSnapInfo removalData )
  {
    ( SnapdClient client, string packageName, string revision, bool purge, CancellationToken cancellationToken ) = removalData;
    return client
          .RemoveSnapAsync ( packageName, revision, purge, 5000, cancellationToken )
          .GetAwaiter ( )
          .GetResult ( );
  }

  private record struct RemoveSnapInfo ( SnapdClient Client, string Name, string Revision, bool Purge, CancellationToken CancellationToken );
}

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
