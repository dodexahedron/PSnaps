#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using PSnaps.SnapdRestApi.Responses;

namespace PSnaps.Cmdlets;

/// <summary>
///   Installs one or more snap packages.
/// </summary>
[PublicAPI]
[Cmdlet ( VerbsLifecycle.Install, "SnapPackage", ConfirmImpact = ConfirmImpact.Medium )]
[OutputType ( typeof( SnapApiResponse ) )]
public sealed class InstallSnapPackageCommand : SnapdClientCmdlet
{
  /// <summary>
  ///   One or more names of snap packages to install.
  /// </summary>
  [Parameter ( Mandatory = true, Position = 0 )]
  [ValidateCount ( 1, int.MaxValue )]
  [ValidateNotNullOrWhiteSpace]
  [ValidateLength ( 2, int.MaxValue )]
  public required string[] Snaps { get; set; }

  /// <summary>
  ///   Specifies the transaction mode to use for the installation operation.<br />
  ///   If <see cref="TransactionMode.PerPackage" /> (default for PSnaps as well as snapd), each package installation uses an isolated
  ///   transaction and failures do not affect other installations (barring dependency issues).<br />
  ///   If <see cref="TransactionMode.AllPackage" />, then all packages are installed in one transaction, and all must succeed or else
  ///   all installations will be rolled back.
  /// </summary>
  [Parameter ( Mandatory = false )]
  public TransactionMode TransactionMode { get; set; }

  /// <inheritdoc />
  [ExcludeFromCodeCoverage]
  protected override void ProcessRecord ( )
  {
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( CmdletCancellationTokenSource.Token );

    try
    {
      SnapApiResponse? response =
        ApiClient
         .InstallMultipleSnapsAsync (
                                     Snaps,
                                     TransactionMode,
                                     Timeout,
                                     cts.Token
                                    )
         .GetAwaiter ( )
         .GetResult ( );

      WriteObject ( response );
    }
    catch ( Exception e )
    {
      WriteObject ( e.Message );
    }
  }
}
