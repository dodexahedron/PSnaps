#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.Cmdlets;

using System.Collections.Concurrent;
using System.Management.Automation;
using JetBrains.Annotations;

using PSnaps.SnapdRestApi.Responses;

using SnapdRestApi;

[PublicAPI]
[Cmdlet ( VerbsCommon.Get, "SnapPackage", ConfirmImpact = ConfirmImpact.None )]
[OutputType ( typeof( List<GetSnapsResponseResult> ) )]
public class GetSnapPackageCommand : Cmdlet
{
  [Parameter ( Mandatory = false )]
  [Alias ( "IncludeInactive" )]
  public SwitchParameter All { get; set; }

  [Parameter ( Mandatory = false, Position = 0 )]
  public string[]? Name { get; set; }

  protected override void ProcessRecord ( )
  {
    using CancellationTokenSource cts = new ( );

    try
    {
      using SnapdClient client = new ( );

      if ( Name is { Length: > 0 } )
      {
        ConcurrentBag<GetSnapsResponseResult> snaps = [ ];
        List<Task>                            tasks = [ ];

        foreach ( string snapName in Name )
        {
          tasks.Add (
                     client.GetSnapAsync ( snapName, All.IsPresent, cancellationToken: cts.Token )
                           .ContinueWith ( CollectResults, snaps, cts.Token )
                    );
        }

        Task.WaitAll ( tasks, cts.Token );
        WriteObject ( snaps.ToList ( ) );

        return;
      }

      if ( All.IsPresent )
      {
        List<GetSnapsResponseResult> allSnaps
          = client.GetAllSnapsAsync ( cancellationToken: cts.Token )
                  .GetAwaiter ( )
                  .GetResult ( )
                 ?.OrderBy ( static result => result.Name )
                  .ToList ( )
         ?? [ ];

        WriteObject ( allSnaps );

        return;
      }

      GC.KeepAlive ( client );
    }
    catch ( Exception e )
    {
      WriteObject ( e.Message );
    }

    return;

    static async Task CollectResults ( Task<GetSnapsResponseResult[]?> task, object? o )
    {
      GetSnapsResponseResult[]? taskResults = await task;

      if ( taskResults is { Length: > 0 } && o is ConcurrentBag<GetSnapsResponseResult> bag )
      {
        foreach ( GetSnapsResponseResult taskResult in taskResults )
        {
          bag.Add ( taskResult );
        }
      }
    }
  }
}
