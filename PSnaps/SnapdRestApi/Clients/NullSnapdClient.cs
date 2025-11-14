#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using PSnaps.SnapdRestApi.Responses;

namespace PSnaps.SnapdRestApi.Clients;

/// <summary>
///   A dummy implementation of <see cref="ISnapdRestClient" /> which takes no actions and returns null or completed tasks returning
///   null for all methods.
/// </summary>
public sealed class NullSnapdClient : ISnapdRestClient
{
  /// <inheritdoc />
  public void Dispose ( )
  {
  }

  /// <inheritdoc />
  public Task<SnapPackage[]?> GetAllSnapsAsync ( int timeout = 30000, CancellationToken cancellationToken = default )
  {
    return Task.FromResult<SnapPackage[]?> ( null );
  }

  /// <inheritdoc />
  public Task<ChangeSet?> GetChangesAsync ( string actionId, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    return Task.FromResult<ChangeSet?> ( null );
  }

  /// <inheritdoc />
  public Task<SnapPackage[]?> GetSingleSnapAsync ( string snapName, bool includeInactive = false, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    return Task.FromResult<SnapPackage[]?> ( null );
  }

  /// <inheritdoc />
  public Task<List<SnapPackage>?> GetSnapsAsync ( string[] snapNames, bool includeInactive = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    return Task.FromResult<List<SnapPackage>?> ( null );
  }

  /// <inheritdoc />
  public Task<SnapApiAsyncResponse?> InstallMultipleSnapsAsync ( string[] snapNames, TransactionMode transactionMode = TransactionMode.PerPackage, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    return Task.FromResult<SnapApiAsyncResponse?> ( null );
  }

  /// <inheritdoc />
  public Task<SnapApiAsyncResponse?> RemoveMultipleSnapsAsync ( string[] snapNames, bool purge = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    return Task.FromResult<SnapApiAsyncResponse?> ( null );
  }

  /// <inheritdoc />
  public Task<SnapApiAsyncResponse?> RemoveSnapAsync ( string name, string revision, bool purge = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    return Task.FromResult<SnapApiAsyncResponse?> ( null );
  }
}
