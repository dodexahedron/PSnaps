#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using System.Collections.Concurrent;
using System.Globalization;
using System.Management.Automation;
using PSnaps.SnapdRestApi.Clients;
using PSnaps.SnapdRestApi.Responses;

namespace PSnaps.Cmdlets;

[PublicAPI]
[Cmdlet ( VerbsCommon.Remove, "SnapPackage", ConfirmImpact = ConfirmImpact.Medium, DefaultParameterSetName = SingleSnapAllRevisionsParameterSetName )]
[Alias ( "Remove-Snap", "Remove-Snaps", "Remove-SnapPackages" )]
[OutputType ( typeof( IEnumerable<SnapApiAsyncResponse> ) )]
public class RemoveSnapPackageCommand : SnapdClientCmdlet
{
  private const string AllSnapsParameterSet                = "AllSnaps";
  private const string CleanUpAllDisabledSnapsParameterSet = nameof (CleanUpAllDisabledSnaps);

  /// <summary>
  ///   This parameter set is for removal of multiple snaps, by name.
  /// </summary>
  private const string MultipleNamedSnapsParameterSetName = "NamedSnaps";

  /// <summary>
  ///   This parameter set is for removal of a single snap, by name.
  /// </summary>
  private const string SingleSnapAllRevisionsParameterSetName = "SingleSnapAllRevisions";

  /// <summary>
  ///   This parameter set is for removal of a single snap package, by revision.
  /// </summary>
  private const string SingleSnapRevisionParameterSetName = "SingleSnapByRevision";

  [Parameter ( Mandatory = true, ParameterSetName = AllSnapsParameterSet )]
  public SwitchParameter All { get; set; }

  [Parameter ( Mandatory = true, ParameterSetName = CleanUpAllDisabledSnapsParameterSet )]
  public SwitchParameter CleanUpAllDisabledSnaps { get; set; }

  [Parameter ( Mandatory = false, ParameterSetName = AllSnapsParameterSet )]
  [Parameter ( Mandatory = false, ParameterSetName = MultipleNamedSnapsParameterSetName )]
  public SwitchParameter Disabled { get; set; }

  [Parameter ( Mandatory = false, ParameterSetName = AllSnapsParameterSet )]
  [Parameter ( Mandatory = false, ParameterSetName = CleanUpAllDisabledSnapsParameterSet )]
  [Parameter ( Mandatory = false, ParameterSetName = MultipleNamedSnapsParameterSetName )]
  [Parameter ( Mandatory = false, ParameterSetName = SingleSnapAllRevisionsParameterSetName )]
  [Parameter ( Mandatory = false, ParameterSetName = SingleSnapRevisionParameterSetName )]
  public SwitchParameter Purge { get; set; }

  [Parameter ( Mandatory = true, Position = 1, ParameterSetName = SingleSnapRevisionParameterSetName )]
  [ValidateRange ( ValidateRangeKind.NonNegative )]
  public int Revision { get; set; }

  [Parameter ( Mandatory = true, Position = 0, ParameterSetName = MultipleNamedSnapsParameterSetName )]
  [Parameter ( Mandatory = true, Position = 0, ParameterSetName = SingleSnapAllRevisionsParameterSetName )]
  [Parameter ( Mandatory = true, Position = 0, ParameterSetName = SingleSnapRevisionParameterSetName )]
  [ValidateLength ( 2, int.MaxValue )]
  public string[]? Snaps { get; set; }

  [ExcludeFromCodeCoverage]
  protected override void ProcessRecord ( )
  {
    WriteObject ( RemoveSnapPackages ( ) );
  }

  private ConcurrentBag<SnapApiAsyncResponse>? RemoveAllDisabledSnaps ( CancellationTokenSource cts )
  {
    // First, get all snaps with status=installed
    // Sort them in descending order of installation date, which should ensure proper removal ordering for dependencies.
    List<RemoveSnapInfo> snapsToRemove
      = ( GetSnapPackageCommand
           .GetSnapPackages ( ApiClient, null, true, Timeout, cts.Token )
       ?? [ ] )
       .Where ( static unfilteredSnap => unfilteredSnap.Status == PackageStatus.Installed )
       .OrderByDescending ( static snap => snap.InstallDate )
       .Select ( snap => new RemoveSnapInfo ( ApiClient, snap.Name, snap.Revision, Purge.IsPresent, cts.Token ) )
       .ToList ( );

    if ( snapsToRemove.Count == 0 )
    {
      // There are no disabled snaps to remove.
      return null;
    }

    // Remove whatever we came up with in parallel, storing the results in a ConcurrentBag.
    ConcurrentBag<SnapApiAsyncResponse> responses = [ ];
    Parallel.ForEach (
                      snapsToRemove,
                      ( ) => responses,
                      static ( snapRemovalInfo, _, responseBag ) =>
                      {
                        if ( RemoveSingleSnap ( snapRemovalInfo ) is { } nonNullResult )
                        {
                          responseBag.Add ( nonNullResult );
                        }

                        return responseBag;
                      },
                      static _ => { }
                     );

    return responses;
  }

  private static SnapApiAsyncResponse? RemoveSingleSnap ( RemoveSnapInfo removalData )
  {
    ( ISnapdRestClient client, string packageName, string revision, bool purge, CancellationToken cancellationToken ) = removalData;
    return client
          .RemoveSnapAsync ( packageName, revision, purge, 5000, cancellationToken )
          .GetAwaiter ( )
          .GetResult ( );
  }

  private ConcurrentBag<SnapApiAsyncResponse>? RemoveSnapPackages ( )
  {
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( CmdletCancellationTokenSource.Token );

    cts.Token.ThrowIfCancellationRequested ( );

    // Validate that there is at least one snap name provided (covered by the implied null check) and that none of the names are null/empty/whitespace.
    if ( Snaps?.Any ( string.IsNullOrWhiteSpace ) is not false )
    {
      ThrowTerminatingError (
                             new (
                                  new ArgumentException ( "Snap names cannot be null, empty, or whitespace." ),
                                  "InvalidSnapName",
                                  ErrorCategory.InvalidArgument,
                                  Snaps
                                 )
                            );
      return null;
    }

    if ( Snaps is not { Length: > 0 } oneOrMoreRequestedSnapNames )
    {
      // No snaps specified, so the only operation we support is to remove all that are disabled
      if ( Disabled.IsPresent && All.IsPresent )
      {
        // Both -Disabled and -All were specified, but no snap names were provided.
        // Remove all packages with status=installed.
        return RemoveAllDisabledSnaps ( cts );
      }

      // If either the -All or -Disabled parameter were not specified, bail out.
      ThrowTerminatingError (
                             new (
                                  new NotSupportedException ( "Unsupported input options. Must specify one or more snap package names or else provide both the -All and -Disabled parameters." ),
                                  "UnsupportedSnapRemovalOptions",
                                  ErrorCategory.InvalidArgument,
                                  this
                                 )
                            );
      return null;
    }

    switch ( ParameterSetName )
    {
      case SingleSnapRevisionParameterSetName:
      {
        // If the single snap parameter set is active, we can just pass the input on.
        SnapApiAsyncResponse? response = RemoveSingleSnap ( new ( ApiClient, oneOrMoreRequestedSnapNames [ 0 ], Revision.ToString ( "D", NumberFormatInfo.InvariantInfo ), Purge.IsPresent, cts.Token ) );

        if ( response is null )
        {
          return null;
        }

        return [ response ];
      }
    }

    return null;
  }

  private record struct RemoveSnapInfo ( ISnapdRestClient Client, string Name, string Revision, bool Purge, CancellationToken CancellationToken );
}
