#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using System.Management.Automation;
using PSnaps.SnapdRestApi;
using PSnaps.SnapdRestApi.Responses;

namespace PSnaps.Cmdlets;

[PublicAPI]
[Cmdlet ( VerbsLifecycle.Install, "SnapPackage", ConfirmImpact = ConfirmImpact.Medium )]
[OutputType ( typeof( SnapApiResponse ) )]
public class InstallSnapPackageCommand : Cmdlet
{
  [Parameter ( Mandatory = true, Position = 0 )]
  [ValidateCount ( 1, int.MaxValue )]
  [ValidateNotNullOrWhiteSpace]
  [ValidateLength ( 2, int.MaxValue )]
  public required string[] Name
  {
    [CollectionAccess ( CollectionAccessType.Read )]
    get;
    set;
  }

  [Parameter ( Mandatory = false )]
  public TransactionMode TransactionMode { get; set; }

  [Parameter ( Mandatory = false )]
  public SwitchParameter RestartIfRequired { get; set; }

  [Parameter ( Mandatory = false, DontShow = true )]
  public int Timeout { get; set; } = 30000;

  protected override void ProcessRecord ( )
  {
    using CancellationTokenSource cts = new ( );

    try
    {
      using SnapdClient client = new ( );
      WriteObject ( client.InstallMultipleSnapsAsync ( Name, TransactionMode, RestartIfRequired.IsPresent, Timeout, cts.Token ).GetAwaiter ( ).GetResult ( ) );
      GC.KeepAlive ( client );
    }
    catch ( Exception e )
    {
      WriteObject ( e.Message );
    }
  }
}