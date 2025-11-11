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

[Cmdlet ( VerbsCommon.Set, Noun, ConfirmImpact = ConfirmImpact.Low )]
[OutputType ( typeof( ISnapdRestClient ) )]
[PublicAPI]
public sealed class SetSnapdClientCommand : PSCmdlet
{
  public const  string Noun                = "SnapdClient";
  private const string InterfaceTypeName   = $"{PSnapsNamespaceName}.{nameof (ISnapdRestClient)}";
  private const string PSnapsNamespaceName = nameof (PSnaps);

  [Parameter ( Mandatory = true, Position = 0, HelpMessage = $"A constructed and valid instance of an [{InterfaceTypeName}] to use for future operations by the PSnaps module.", ValueFromPipeline = true )]
  [ValidateNotNull]
  public required ISnapdRestClient? Client { get; set; }

  [Parameter ( Mandatory = false, HelpMessage = "If set, the previous client object will will NOT be disposed. Default behavior is to dispose of the previous client object." )]
  public SwitchParameter DoNotDisposeOldClient { get; set; }

  /// <inheritdoc />
  protected override void ProcessRecord ( )
  {
    ArgumentNullException.ThrowIfNull ( Client );

    ISnapdRestClient? oldClient = PSnapsModuleState.ExchangeSnapdClient ( Client );

    if ( !DoNotDisposeOldClient.IsPresent )
    {
      oldClient?.Dispose ( );
    }

    WriteObject ( PSnapsModuleState.Client );
  }
}
