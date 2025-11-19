#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using System.Diagnostics.CodeAnalysis;
using PSnaps.SnapCore;
using PSnaps.SnapdRestApi;
using PSnaps.SnapdRestApi.Clients;
using PSnaps.SnapdRestApi.Responses;

namespace PSnaps.Mocks;

[PublicAPI]
public class MockSnapdClient ( CancellationToken realCancellationToken = default ) : ISnapdRestClient
{
  private readonly CancellationTokenSource _snapdClientCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource ( realCancellationToken );

  private long _nonZeroMeansDisposed;

  /// <inheritdoc />
  public void Dispose ( )
  {
    if ( Interlocked.CompareExchange ( ref _nonZeroMeansDisposed, -1L, 0L ) != 0L )
    {
      // Already disposed or being disposed.
      return;
    }

    if ( _snapdClientCancellationTokenSource.Token.CanBeCanceled )
    {
      try
      {
        _snapdClientCancellationTokenSource.Cancel ( true );
      }
      catch
      {
        // Just eat any exception. Nothing to do here.
      }
    }

    _snapdClientCancellationTokenSource.Dispose ( );
    GC.SuppressFinalize ( this );
  }

  /// <inheritdoc />
  public Uri SnapdUnixSocketUri { get; } = new ( ISnapdRestClient.DefaultSnapdUnixSocketPath, UriKind.Absolute );

  /// <inheritdoc />
  [SuppressMessage ( "ReSharper", "StringLiteralTypo" )]
  public async Task<SnapPackage[]?> GetAllSnapsAsync ( int timeout = 30000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _snapdClientCancellationTokenSource is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    cts.CancelAfter ( timeout );
    string jsonFromFile = await File.ReadAllTextAsync ( "SampleJSON/GetAllSnaps.json", cts.Token );
    SnapApiSyncResponse<SnapPackage[]?> response = JsonSerializer
     .Deserialize<SnapApiSyncResponse<SnapPackage[]?>> (
                                                        jsonFromFile,
                                                        SnapApiJsonSerializerContext.Default.SnapApiSyncResponseSnapPackageArray
                                                       )!;
    return response.Result;
  }

  /// <inheritdoc />
  public async Task<ChangeSet?> GetChangesAsync ( string actionId, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _snapdClientCancellationTokenSource is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    cts.CancelAfter ( timeout );
    return await Task.FromResult<ChangeSet?> ( null );
  }

  /// <inheritdoc />
  public async Task<SnapPackage[]?> GetSingleSnapAsync ( string snapName, bool includeInactive = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _snapdClientCancellationTokenSource is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    SnapPackage[] response = JsonSerializer
     .Deserialize<SnapPackage[]> (
                                  await File.ReadAllTextAsync ( "SnapPackageArray.json", cts.Token ),
                                  SnapApiJsonSerializerContext.Default.SnapPackageArray
                                 )!;
    return response
          .Where ( snap => snap.Name == snapName && ( includeInactive || snap.Status == PackageStatus.Active ) )
          .ToArray ( );
  }

  /// <inheritdoc />
  public async Task<List<SnapPackage>?> GetSnapsAsync ( string[] snapNames, bool includeInactive = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _snapdClientCancellationTokenSource is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    cts.CancelAfter ( timeout );
    SnapPackage[] response = JsonSerializer
     .Deserialize<SnapPackage[]> (
                                  await File.ReadAllTextAsync ( "SnapPackageArray.json", cts.Token ),
                                  SnapApiJsonSerializerContext.Default.SnapPackageArray
                                 )!;
    return response
          .Where ( snap => snapNames.Contains ( snap.Name ) && ( includeInactive || snap.Status == PackageStatus.Active ) )
          .ToList ( );
  }

  /// <inheritdoc />
  public async Task<SnapApiAsyncResponse?> InstallMultipleSnapsAsync ( string[] snapNames, TransactionMode transactionMode = TransactionMode.PerPackage, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _snapdClientCancellationTokenSource is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    cts.CancelAfter ( timeout );
    return await Task.FromResult<SnapApiAsyncResponse?> ( null );
  }

  /// <inheritdoc />
  public async Task<SnapApiAsyncResponse?> RemoveMultipleSnapsAsync ( string[] snapNames, bool purge = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _snapdClientCancellationTokenSource is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    cts.CancelAfter ( timeout );
    return await Task.FromResult<SnapApiAsyncResponse?> ( null );
  }

  /// <inheritdoc />
  public async Task<SnapApiAsyncResponse?> RemoveSnapAsync ( string name, string revision, bool purge = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _snapdClientCancellationTokenSource is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    cts.CancelAfter ( timeout );
    return await Task.FromResult<SnapApiAsyncResponse?> ( null );
  }
}
