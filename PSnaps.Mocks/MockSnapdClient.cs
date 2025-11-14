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
  private static JsonSerializerOptions _serializerOptions = new ( )
                                                            {
                                                              RespectNullableAnnotations           = true,
                                                              RespectRequiredConstructorParameters = true,
                                                              AllowOutOfOrderMetadataProperties    = true,
                                                              UnmappedMemberHandling               = JsonUnmappedMemberHandling.Skip
                                                            };

  private CancellationTokenSource? _cts = CancellationTokenSource.CreateLinkedTokenSource ( realCancellationToken );

  /// <inheritdoc />
  public void Dispose ( )
  {
    CancellationTokenSource? cts = Interlocked.Exchange ( ref _cts, null );

    if ( cts is not null )
    {
      cts.Dispose ( );
      GC.SuppressFinalize ( this );
    }
    else
    {
      throw new ObjectDisposedException ( nameof (MockSnapdClient) );
    }
  }

  /// <inheritdoc />
  public Uri SnapdUnixSocketUri { get; } = new ( ISnapdRestClient.DefaultSnapdUnixSocketPath, UriKind.Absolute );

  /// <inheritdoc />
  [SuppressMessage ( "ReSharper", "StringLiteralTypo" )]
  public async Task<SnapPackage[]?> GetAllSnapsAsync ( int timeout = 30000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _cts is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _cts.Token, cancellationToken );
    cts.CancelAfter ( 5000 );
    SnapPackage[] response = JsonSerializer
     .Deserialize<SnapPackage[]> (
                                  await File.ReadAllTextAsync ( "SnapPackageArray.json", cts.Token ),
                                  SnapApiJsonSerializerContext.Default.SnapPackageArray
                                 )!;
    return response;
  }

  /// <inheritdoc />
  public async Task<ChangeSet?> GetChangesAsync ( string actionId, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _cts is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _cts.Token, cancellationToken );
    cts.CancelAfter ( timeout );
    return await Task.FromResult<ChangeSet?> ( null );
  }

  /// <inheritdoc />
  public async Task<SnapPackage[]?> GetSingleSnapAsync ( string snapName, bool includeInactive = false, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _cts is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _cts.Token, cancellationToken );
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
    ObjectDisposedException.ThrowIf ( _cts is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _cts.Token, cancellationToken );
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
    ObjectDisposedException.ThrowIf ( _cts is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _cts.Token, cancellationToken );
    cts.CancelAfter ( timeout );
    return await Task.FromResult<SnapApiAsyncResponse?> ( null );
  }

  /// <inheritdoc />
  public async Task<SnapApiAsyncResponse?> RemoveMultipleSnapsAsync ( string[] snapNames, bool purge = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _cts is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _cts.Token, cancellationToken );
    cts.CancelAfter ( timeout );
    return await Task.FromResult<SnapApiAsyncResponse?> ( null );
  }

  /// <inheritdoc />
  public async Task<SnapApiAsyncResponse?> RemoveSnapAsync ( string name, string revision, bool purge = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    ObjectDisposedException.ThrowIf ( _cts is null, this );
    using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource ( _cts.Token, cancellationToken );
    cts.CancelAfter ( timeout );
    return await Task.FromResult<SnapApiAsyncResponse?> ( null );
  }
}
