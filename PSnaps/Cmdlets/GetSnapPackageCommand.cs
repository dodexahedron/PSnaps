#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using System.Management.Automation;
using PSnaps.SnapdRestApi.Clients;

namespace PSnaps.Cmdlets;

[PublicAPI]
[Cmdlet ( VerbsCommon.Get, "SnapPackage", ConfirmImpact = ConfirmImpact.None )]
[OutputType ( typeof( List<SnapPackage> ) )]
public sealed class GetSnapPackageCommand : SnapdClientCmdlet
{
  [Parameter ( Mandatory = false )]
  [Alias ( "IncludeInactive" )]
  public SwitchParameter All { get; set; }

  [Parameter ( Mandatory = false, Position = 0 )]
  public string[]? Name { get; set; }

  /// <summary>
  ///   Gets snap packages according to user input.
  /// </summary>
  /// <param name="apiClient">The <see cref="ISnapdRestClient" /> to use for the operation.</param>
  /// <param name="snapNames">
  ///   An array of zero or more snap package names.<br />If empty or <see langword="null" />, <paramref name="all" /> must be true or
  ///   no results will be returned.
  /// </param>
  /// <param name="all">
  ///   If <see langword="true" />, the result set will include packages with any installation status and, if no names were provided
  ///   for <paramref name="snapNames" /> all snaps on the system will be included.
  /// </param>
  /// <param name="timeout">A timeout in milliseconds to use for the operation. Default: 30 seconds.</param>
  /// <param name="cancellationToken">
  ///   A <see cref="CancellationToken" /> to use for this operation, which will be linked with the master
  ///   <see cref="CancellationTokenSource" /> for the cmdlet as well.
  /// </param>
  /// <returns>
  ///   A <see cref="List{T}" /> of <see cref="SnapPackage" /> or <see langwrd="null" />.
  /// </returns>
  /// <remarks>
  ///   If multiple results are returned, they are sorted according to <see cref="SnapPackage.CompareTo(SnapPackage?)" />, which sorts
  ///   first by name, then channel, then revision.
  /// </remarks>
  /// <exception cref="PipelineStoppedException">
  ///   If any part of the operation throws an exception.<br />
  ///   The original exception will be included in the <see cref="PipelineStoppedException" />'s
  ///   <see cref="Exception.InnerException" /> property.
  /// </exception>
  /// <exception cref="OperationCanceledException">
  ///   If any of the linked <see cref="CancellationToken" />s are canceled, such as when exceeding the <paramref name="timeout" /> or
  ///   when <see cref="Cmdlet.StopProcessing" /> is called.
  /// </exception>
  public static List<SnapPackage>? GetSnapPackages ( ISnapdRestClient apiClient, string[]? snapNames, bool all = false, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( cancellationToken );
    cts.CancelAfter ( timeout );

    cts.Token.ThrowIfCancellationRequested ( );

    if ( snapNames is { Length: > 0 } )
    {
      // Retrieve each snap in parallel and collect the results.
      return apiClient.GetSnapsAsync ( snapNames, all, timeout, cts.Token )
                      .GetAwaiter ( )
                      .GetResult ( )
                      ?.Order ( )
                      .ToList ( );
    }

    cts.Token.ThrowIfCancellationRequested ( );

    if ( !all )
    {
      // No names were provided, and the -All parameter was not specified; nothing to retrieve.
      return null;
    }

    // Retrieve all snaps.
    List<SnapPackage> allSnaps
      = apiClient.GetAllSnapsAsync ( timeout, cts.Token )
                 .GetAwaiter ( )
                 .GetResult ( )
                ?.Order ( )
                 .ToList ( )
     ?? [ ];

    return allSnaps;
  }

  [ExcludeFromCodeCoverage]
  protected override void ProcessRecord ( )
  {
    WriteObject ( GetSnapPackages ( CmdletCancellationTokenSource.Token ) );
  }

  internal List<SnapPackage>? GetSnapPackages ( CancellationToken cancellationToken = default )
  {
    return GetSnapPackages ( ApiClient, Name, All.IsPresent, Timeout, cancellationToken );
  }
}
