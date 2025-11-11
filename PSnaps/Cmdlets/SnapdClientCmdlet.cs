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

/// <summary>
///   A base class for cmdlets in the module, providing common baseline functionality.
/// </summary>
[PublicAPI]
public abstract class SnapdClientCmdlet : PSCmdlet
{
  /// <summary>
  ///   A timeout in milliseconds to use for operations in this cmdlet.
  /// </summary>
  /// <remarks>Default is 30 seconds.</remarks>
  [Parameter ( Mandatory = false )]
  [ValidateRange ( ValidateRangeKind.Positive )]
  public virtual int Timeout { get; set; } = 30000;

  /// <summary>
  ///   Gets the client to use for the lifetime of the <see cref="SnapdClientCmdlet" /> invocation.
  /// </summary>
  /// <remarks></remarks>
  protected internal ISnapdRestClient ApiClient { get; } = PSnapsModuleState.Client ?? new NullSnapdClient ( );

  /// <summary>
  ///   A <see cref="CmdletCancellationTokenSource" /> that is linked to the module's master cancellation token.
  /// </summary>
  protected internal CancellationTokenSource CmdletCancellationTokenSource { get; } = CancellationTokenSource.CreateLinkedTokenSource ( PSnapsModuleState.ModuleMasterCancellationToken );

  /// <summary>
  ///   The time at which this instance of a <see cref="SnapdClientCmdlet" /> was constructed.
  /// </summary>
  internal DateTimeOffset ExecutedAt { get; } = DateTimeOffset.Now;
}
