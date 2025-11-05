#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using System.Management.Automation;
using System.Text.Json;
using PSnaps.SnapdRestApi;
using PSnaps.SnapdRestApi.Responses;

namespace PSnaps.Cmdlets;

[PublicAPI]
[Cmdlet ( VerbsCommon.Remove, "SnapPackage", ConfirmImpact = ConfirmImpact.Medium, DefaultParameterSetName = SingleSnapParameterSet )]
[Alias ( "Remove-Snap", "Remove-Snaps", "Remove-SnapPackages" )]
[OutputType ( typeof( RemoveSnapResult ),       ParameterSetName = [ SingleSnapParameterSet ] )]
[OutputType ( typeof( List<RemoveSnapResult> ), ParameterSetName = [ AllSnapsParameterSet ] )]
public class RemoveSnapPackageCommand : PSCmdlet
{
  private const string AllSnapsParameterSet   = "AllSnaps";
  private const string SingleSnapParameterSet = "SingleSnap";

  [Parameter ( Mandatory = true, ParameterSetName = AllSnapsParameterSet )]
  public SwitchParameter All { get; set; }

  [Parameter ( Mandatory = false, ParameterSetName = SingleSnapParameterSet )]
  [Parameter ( Mandatory = false, ParameterSetName = AllSnapsParameterSet )]
  public SwitchParameter Disabled { get; set; }

  [Parameter ( Mandatory = true, Position = 0, ParameterSetName = SingleSnapParameterSet )]
  public string? Name { get; set; }

  [Parameter ( Mandatory = false, ParameterSetName = SingleSnapParameterSet )]
  [Parameter ( Mandatory = false, ParameterSetName = AllSnapsParameterSet )]
  public SwitchParameter Purge { get; set; }

  [Parameter ( Mandatory = true, Position = 1, ParameterSetName = SingleSnapParameterSet )]
  public int Revision { get; set; }

  protected override void ProcessRecord ( )
  {
    CancellationTokenSource cts    = new ( 30000 );
    using SnapdClient       client = new ( );

    switch ( ParameterSetName )
    {
      case SingleSnapParameterSet:
      {
        if ( Disabled.IsPresent )
        {
          // TODO: Handle the -Disabled parameter.
          throw new NotImplementedException ( "The -Disabled parameter is not yet implemented." );
        }

        if ( Purge.IsPresent )
        {
          // TODO: Handle the -Purge parameter.
          throw new NotImplementedException ( "The -Purge parameter is not yet implemented." );
        }

        ArgumentException.ThrowIfNullOrWhiteSpace ( Name );
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero ( Revision );
        RemoveSnapResult? result = client.RemoveSnapAsync ( Name, Revision, cancellationToken: cts.Token ).GetAwaiter ( ).GetResult ( );
        WriteVerbose ( JsonSerializer.Serialize ( result, SnapApiJsonSerializerContext.Default.RemoveSnapResult ) );
      }
        return;

      case AllSnapsParameterSet:
      {
        // TODO: Handle the -All parameter.
        throw new NotImplementedException ( "The -All parameter is not yet implemented." );
      }
    }
  }
}
