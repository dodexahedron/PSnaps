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
using System.Text.Json.Serialization.Metadata;
using PSnaps.SnapdRestApi;
using PSnaps.SnapdRestApi.Responses;

namespace PSnaps;

[PublicAPI]
[MustDisposeResource]
public record SnapdClient : IDisposable
{
  private readonly HttpClient              _httpClient;
  private readonly CancellationTokenSource _snapdClientCancellationTokenSource;
  private const    string                  DefaultApiBaseUriV2        = "http://localhost/v2/";
  private const    string                  DefaultSnapdUnixSocketPath = "unix:///run/snapd.socket";
  private          long                    _nonZeroMeansDisposed;

  public SnapdClient ( string absoluteBaseUri = DefaultApiBaseUriV2, string socketPath = DefaultSnapdUnixSocketPath, CancellationToken cancellationToken = default )
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

  public Uri    BaseUri            { get; }
  public string SnapdSocketPath    { get; }
  public Uri    SnapdUnixSocketUri { get; }

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
  public async Task<SnapPackage[]?> GetAllSnapsAsync ( int timeout = 30000, CancellationToken cancellationToken = default ) => await GetResultAsync ( "snaps?select=all", SnapApiJsonSerializerContext.Default.IHaveResultSnapPackageArray, timeout, cancellationToken );

  public async Task<ChangeSet?> GetChangesAsync ( string actionId, int timeout = 30000, CancellationToken cancellationToken = default ) => await GetResultAsync ( $"changes/{actionId}", SnapApiJsonSerializerContext.Default.IHaveResultChangeSet, timeout, cancellationToken );

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

  [CollectionAccess ( CollectionAccessType.UpdatedContent )]
  public async Task<SnapPackage[]?> GetSnapAsync ( string snapName, bool includeInactive = false, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    return await GetResultAsync (
                                 $"snaps/{snapName}{( includeInactive ? "?select=all" : string.Empty )}",
                                 SnapApiJsonSerializerContext.Default.IHaveResultSnapPackageArray,
                                 timeout,
                                 cancellationToken
                                )
            .ConfigureAwait ( false );
  }

  public async Task<SnapApiResponse?> InstallMultipleSnapsAsync ( string[] snapNames, TransactionMode transactionMode = TransactionMode.PerPackage, bool restartIfRequired = false, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    SnapApiResponse? snapApiResponse = await PostAsync (
                                                        "snaps",
                                                        new ( snapNames, transactionMode, restartIfRequired ),
                                                        SnapApiJsonSerializerContext.Default.InstallMultipleSnapsPostData,
                                                        SnapApiJsonSerializerContext.Default.SnapApiResponse,
                                                        timeout,
                                                        cancellationToken
                                                       );
    return snapApiResponse;
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

  public async Task<SnapApiResponse?> RemoveSnapAsync ( string name, string revision, bool purge = false, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    return await PostAsync (
                            $"snaps/{name}",
                            new ( revision, purge ),
                            SnapApiJsonSerializerContext.Default.RemoveSnapsPostData,
                            SnapApiJsonSerializerContext.Default.SnapApiResponse,
                            timeout,
                            cancellationToken
                           )
            .ConfigureAwait ( false );
  }
}