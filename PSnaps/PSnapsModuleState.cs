#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps;

using OldClientNewClientPair = (ISnapdRestClient? oldClient, ISnapdRestClient? newClient);

/// <summary>
///   Global module state manager class.<br />
///   This class holds a master cancellation token that is provided and linked in all operations, as well as an instance of
///   <see cref="ISnapdRestClient" /> that is used by module cmdlets.
/// </summary>
public static class PSnapsModuleState
{
  private static readonly CancellationTokenSource                       Cts     = new ( );

  // TODO: Handle disposal of the client on module unload.
  private static          ISnapdRestClient? _client = new SnapdClient ( );

  /// <summary>
  ///   Atomically gets or sets the client to be used by the module for future operations via
  ///   <see cref="Volatile.Read{T}(ref readonly T)" /> and <see cref="Interlocked.Exchange{T}(ref T, T)" />.
  /// </summary>
  public static ISnapdRestClient? Client => Volatile.Read ( ref _client );

  internal static CancellationToken ModuleMasterCancellationToken => Cts.Token;

    /// <summary>
    ///   Uses <see cref="Interlocked.Exchange{T}(ref T, T)" /> to atomically replace the client used by the module with the referenced
    ///   <paramref name="newClient" /> and return the old client.
    /// </summary>
    /// <param name="newClient">
    ///   A reference to an <see cref="ISnapdRestClient" /> object that is fully constructed and ready for use.
    /// </param>
    /// <returns>The previous client object, if any, or <see langword="null" />.</returns>
    /// <remarks>
    ///   The caller is responsible for disposing of non-null old clients returned by this method if they are no longer needed.
    /// </remarks>
    /// <exception cref="NullReferenceException">If <paramref name="newClient" /> is a null reference.</exception>
    /// <exception cref="NotSupportedException">
    ///   If <paramref name="newClient" /> is a type that is not supported by <see cref="Interlocked.Exchange{T}(ref T, T)" />.
    /// </exception>
    /// <exception cref="SetValueException">If the new client is the same object as the old client.</exception>
    public static ISnapdRestClient? ExchangeSnapdClient (ISnapdRestClient newClient )
  {
    return ReferenceEquals ( _client, newClient )
             ? throw new SetValueException ( "The old and new client are the same object." )
             : Interlocked.Exchange ( ref _client, newClient );
  }
}
