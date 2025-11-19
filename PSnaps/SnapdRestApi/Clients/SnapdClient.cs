#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Runtime.Versioning;
using System.Text.Json.Serialization.Metadata;
using PSnaps.SnapdRestApi.Responses;

namespace PSnaps.SnapdRestApi.Clients;

/// <summary>
///   Default implementation of the <see cref="ISnapdRestClient" /> interface, used by the module for normal operation.
/// </summary>
[PublicAPI]
[MustDisposeResource]
public sealed record SnapdClient : ISnapdRestClient
{
  private readonly HttpClient              _httpClient;
  private readonly CancellationTokenSource _snapdClientCancellationTokenSource;
  private          long                    _nonZeroMeansDisposed;

  /// <summary>
  ///   Creates an instance of <see cref="SnapdClient" /> with default URIs and a <see cref="CancellationToken" /> that is not linked
  ///   to any parent <see cref="CancellationToken" />s.
  /// </summary>
  public SnapdClient ( ) : this ( ISnapdRestClient.DefaultApiBaseUriV2 )
  {
  }

  /// <summary>
  ///   Creates an instance of <see cref="SnapdClient" /> with an overridden path to the snapd REST API Unix Domain Socket.
  /// </summary>
  /// <param name="socketUri">The <c>unix:///path/to/the/snapd.socket</c> file.</param>
  public SnapdClient ( string socketUri ) : this ( socketPath: socketUri )
  {
  }

  /// <summary>
  ///   Creates an instance of <see cref="SnapdClient" /> with the specified or default URIs and <see cref="CancellationToken" />.
  /// </summary>
  /// <param name="absoluteBaseUri">
  ///   If specified, overrides the absolute base URI the client will use when making requests over the snapd socket.
  /// </param>
  /// <param name="socketPath">
  ///   If specified, overrides the absolute URI to the snapd Unix Domain Socket.<br />
  ///   Must be of the form unix:///path/to/socket, or a fully-qualified file path without scheme token.
  /// </param>
  /// <param name="cancellationToken">
  ///   If specified, the new instance of <see cref="SnapdClient" /> will link its token source to the provided
  ///   <see cref="CancellationToken" />.
  /// </param>
  public SnapdClient (
    string            absoluteBaseUri   = ISnapdRestClient.DefaultApiBaseUriV2,
    string            socketPath        = ISnapdRestClient.DefaultSnapdUnixSocketPath,
    CancellationToken cancellationToken = default
  )
  {
    _snapdClientCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource ( cancellationToken );
    BaseUri                             = new ( absoluteBaseUri, UriKind.Absolute );
    SnapdUnixSocketUri                  = new ( socketPath, UriKind.Absolute );

    _httpClient = new (
                       new SocketsHttpHandler
                       {
                         ConnectCallback = async ( context, token ) =>
                                           {
                                             Socket                   socket   = new ( AddressFamily.Unix, SocketType.Stream, ProtocolType.IP );
                                             UnixDomainSocketEndPoint endpoint = new ( SnapdUnixSocketUri.AbsolutePath );
                                             await socket.ConnectAsync ( endpoint, token );
                                             context.InitialRequestMessage.Headers.AcceptEncoding.Clear ( );
                                             context.InitialRequestMessage.Headers.AcceptEncoding.Add ( new ( "identity" ) );
                                             context.InitialRequestMessage.Headers.ConnectionClose = true;
                                             context.InitialRequestMessage.Headers.UserAgent.Clear ( );
                                             context.InitialRequestMessage.Headers.UserAgent.Add ( new ( "PSnaps", "1.0" ) );
                                             return new NetworkStream ( socket, FileAccess.ReadWrite, true );
                                           }
                       },
                       true
                      )
                  {
                    BaseAddress           = BaseUri,
                    DefaultRequestVersion = HttpVersion.Version10
                  };
  }

  /// <summary>
  ///   Gets the base URI for the snapd API.
  /// </summary>
  /// <remarks>Default is provided by <see cref="ISnapdRestClient.DefaultApiBaseUriV2" />.</remarks>
  public Uri BaseUri { get; init; }

  /// <inheritdoc />
  public Uri SnapdUnixSocketUri { get; }

  /// <inheritdoc />
  public virtual void Dispose ( )
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
      catch ( AggregateException )
      {
        // TODO: Decide if doing something other than eating this is appropriate.
      }
    }

    _httpClient.Dispose ( );
    _snapdClientCancellationTokenSource.Dispose ( );
  }

  /// <inheritdoc />
  [CollectionAccess ( CollectionAccessType.UpdatedContent )]
  public async Task<SnapPackage[]?> GetAllSnapsAsync ( int timeout = 10000, CancellationToken cancellationToken = default )
  {
    return await GetResultAsync ( "snaps?select=all", SnapApiJsonSerializerContext.Default.SnapApiSyncResponseSnapPackageArray, timeout, cancellationToken );
  }

  /// <inheritdoc />
  public async Task<ChangeSet?> GetChangesAsync ( string actionId, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    return await GetResultAsync ( $"changes/{actionId}", SnapApiJsonSerializerContext.Default.SnapApiSyncResponseChangeSet, timeout, cancellationToken );
  }

  /// <inheritdoc />
  [CollectionAccess ( CollectionAccessType.UpdatedContent )]
  public async Task<SnapPackage[]?> GetSingleSnapAsync ( string snapName, bool includeInactive = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    return await GetResultAsync (
                                 $"snaps/{snapName}{( includeInactive ? "?select=all" : string.Empty )}",
                                 SnapApiJsonSerializerContext.Default.SnapApiSyncResponseSnapPackageArray,
                                 timeout,
                                 cancellationToken
                                )
            .ConfigureAwait ( false );
  }

  /// <inheritdoc />
  public async Task<List<SnapPackage>?> GetSnapsAsync ( string[] snapNames, bool includeInactive = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    SnapPackage[]? snapPackages = await GetResultAsync (
                                                        $"snaps?snaps={string.Join ( ',', snapNames )}{( includeInactive ? "&select=all" : string.Empty )}",
                                                        SnapApiJsonSerializerContext.Default.SnapApiSyncResponseSnapPackageArray,
                                                        timeout,
                                                        cancellationToken
                                                       )
                                   .ConfigureAwait ( false );
    return snapPackages is null ? null : [ ..snapPackages ];
  }

  /// <inheritdoc />
  public async Task<SnapApiAsyncResponse?> InstallMultipleSnapsAsync ( string[] snapNames, TransactionMode transactionMode = TransactionMode.PerPackage, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    SnapApiAsyncResponse? snapApiResponse = await PostAsync (
                                                             "snaps",
                                                             new ( snapNames, transactionMode ),
                                                             SnapApiJsonSerializerContext.Default.InstallMultipleSnapsPostData,
                                                             SnapApiJsonSerializerContext.Default.SnapApiAsyncResponse,
                                                             timeout,
                                                             cancellationToken
                                                            );
    return snapApiResponse;
  }

  /// <inheritdoc />
  public async Task<SnapApiAsyncResponse?> RemoveMultipleSnapsAsync ( string[] snapNames, bool purge = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    return await PostAsync (
                            "snaps",
                            new ( snapNames, purge ),
                            SnapApiJsonSerializerContext.Default.RemoveMultipleSnapsPostData,
                            SnapApiJsonSerializerContext.Default.SnapApiAsyncResponse,
                            timeout,
                            cancellationToken
                           )
            .ConfigureAwait ( false );
  }

  /// <inheritdoc />
  public async Task<SnapApiAsyncResponse?> RemoveSnapAsync ( string name, string revision, bool purge = false, int timeout = 10000, CancellationToken cancellationToken = default )
  {
    return await PostAsync (
                            $"snaps/{name}",
                            new ( name, revision, purge ),
                            SnapApiJsonSerializerContext.Default.RemoveSingleSnapPostData,
                            SnapApiJsonSerializerContext.Default.SnapApiAsyncResponse,
                            timeout,
                            cancellationToken
                           )
            .ConfigureAwait ( false );
  }

  private async Task<TResult?> GetResultAsync<TResult> ( string path, JsonTypeInfo<SnapApiSyncResponse<TResult>> jsonSerializationTypeInfo, int timeout, CancellationToken cancellationToken = default )
    where TResult : class
  {
    using CancellationTokenSource taskCts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    taskCts.Token.ThrowIfCancellationRequested ( );
    taskCts.CancelAfter ( timeout );

    SnapApiSyncResponse<TResult>? response =
      await _httpClient
           .GetFromJsonAsync (
                              path,
                              jsonSerializationTypeInfo,
                              taskCts.Token
                             )
           .ConfigureAwait ( true );
    return response?.Result;
  }

  private async Task<TResponse?> PostAsync<TResponse, TPostData> ( string path, TPostData postData, JsonTypeInfo<TPostData> postDataJsonTypeInfo, JsonTypeInfo<TResponse> responseJsonTypeInfo, int timeout = 30000, CancellationToken cancellationToken = default )
    where TResponse : SnapApiResponse
  {
    using CancellationTokenSource taskCts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    taskCts.Token.ThrowIfCancellationRequested ( );
    taskCts.CancelAfter ( timeout );

    HttpResponseMessage response = await _httpClient.PostAsJsonAsync ( path, postData, postDataJsonTypeInfo, taskCts.Token ).ConfigureAwait ( true );

    if ( response.IsSuccessStatusCode )
    {
      return await response.Content.ReadFromJsonAsync ( responseJsonTypeInfo, taskCts.Token );
    }

    return null;
  }
}
