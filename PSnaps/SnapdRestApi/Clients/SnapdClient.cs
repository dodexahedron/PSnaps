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

[PublicAPI]
[MustDisposeResource]
[SupportedOSPlatform ( "linux" )]
public record SnapdClient : ISnapdRestClient
{
  private readonly HttpClient              _httpClient;
  private readonly CancellationTokenSource _snapdClientCancellationTokenSource;
  private          long                    _nonZeroMeansDisposed;

  public SnapdClient ( ) : this ( ISnapdRestClient.DefaultApiBaseUriV2 )
  {
  }

  public SnapdClient (
    string            absoluteBaseUri   = ISnapdRestClient.DefaultApiBaseUriV2,
    string            socketPath        = ISnapdRestClient.DefaultSnapdUnixSocketPath,
    CancellationToken cancellationToken = default
  )
  {
    _snapdClientCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource ( cancellationToken );
    BaseUri                             = new ( absoluteBaseUri, UriKind.Absolute );
    SnapdSocketPath                     = socketPath;
    SnapdUnixSocketUri                  = new ( SnapdSocketPath, UriKind.Absolute );

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

  public Uri BaseUri { get; init; }

  public string SnapdSocketPath { get; init; }

  public Uri SnapdUnixSocketUri { get; }

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
    GC.SuppressFinalize ( this );
  }

  [CollectionAccess ( CollectionAccessType.UpdatedContent )]
  public async Task<SnapPackage[]?> GetAllSnapsAsync ( int timeout = 30000, CancellationToken cancellationToken = default )
  {
    return await GetResultAsync ( "snaps?select=all", SnapApiJsonSerializerContext.Default.IHaveResultSnapPackageArray, timeout, cancellationToken );
  }

  public async Task<ChangeSet?> GetChangesAsync ( string actionId, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    return await GetResultAsync ( $"changes/{actionId}", SnapApiJsonSerializerContext.Default.IHaveResultChangeSet, timeout, cancellationToken );
  }

  [CollectionAccess ( CollectionAccessType.UpdatedContent )]
  public async Task<SnapPackage[]?> GetSingleSnapAsync ( string snapName, bool includeInactive = false, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    return await GetResultAsync (
                                 $"snaps/{snapName}{( includeInactive ? "?select=all" : string.Empty )}",
                                 SnapApiJsonSerializerContext.Default.IHaveResultSnapPackageArray,
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
                                                        SnapApiJsonSerializerContext.Default.IHaveResultSnapPackageArray,
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

  public async Task<TResult?> GetResultAsync<TResult> ( string path, JsonTypeInfo<IHaveResult<TResult>> jsonSerializationTypeInfo, int timeout, CancellationToken cancellationToken = default )
    where TResult : class
  {
    using CancellationTokenSource taskCts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    taskCts.Token.ThrowIfCancellationRequested ( );
    taskCts.CancelAfter ( timeout );

    IHaveResult<TResult>? response =
      await _httpClient.GetFromJsonAsync (
                                          path,
                                          jsonSerializationTypeInfo,
                                          taskCts.Token
                                         )
                       .ConfigureAwait ( true );
    return response?.Result;
  }

  public async Task<TResponse?> PostAsync<TResponse, TPostData> ( string path, TPostData postData, JsonTypeInfo<TPostData> postDataJsonTypeInfo, JsonTypeInfo<TResponse> responseJsonTypeInfo, int timeout = 30000, CancellationToken cancellationToken = default )
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
