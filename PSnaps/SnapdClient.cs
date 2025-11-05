#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps;

using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text.Json.Serialization.Metadata;
using SnapdRestApi;
using SnapdRestApi.Responses;

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

  public async Task<GetSnapsResponseResult[]?> GetAllSnapsAsync ( int timeout = 30000, CancellationToken cancellationToken = default )
  {
    using CancellationTokenSource taskCts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    taskCts.Token.ThrowIfCancellationRequested ( );
    taskCts.CancelAfter ( timeout );

    GetSnapsResponse? response =
      await _httpClient.GetFromJsonAsync (
                                          "snaps?select=all",
                                          SnapdRestApi.SnapApiJsonSerializerContext.Default.GetSnapsResponse,
                                          taskCts.Token
                                         )
                       .ConfigureAwait ( true );
    return response?.Result;
  }

  public async Task<GetChangesResult?> GetChangesAsync ( string actionId, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    using CancellationTokenSource taskCts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    taskCts.Token.ThrowIfCancellationRequested ( );
    taskCts.CancelAfter ( timeout );

    GetChangesResponse? response =
      await _httpClient.GetFromJsonAsync (
                                          $"changes/{actionId}",
                                          SnapdRestApi.SnapApiJsonSerializerContext.Default.GetChangesResponse,
                                          taskCts.Token
                                         );

    return response?.Result;
  }

  public async Task<GetSnapsResponseResult[]?> GetSnapAsync ( string snapName, bool includeInactive = false, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    using CancellationTokenSource taskCts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    taskCts.Token.ThrowIfCancellationRequested ( );
    taskCts.CancelAfter ( timeout );

    GetSnapsResponse? response =
      await _httpClient.GetFromJsonAsync (
                                          $"snaps/{snapName}{( includeInactive ? "?select=all" : string.Empty )}",
                                          SnapdRestApi.SnapApiJsonSerializerContext.Default.GetSnapsResponse,
                                          taskCts.Token
                                         )
                       .ConfigureAwait ( true );
    return response?.Result;
  }

  public async Task<RemoveSnapResult?> RemoveSnapAsync ( string name, int revision, int timeout = 30000, CancellationToken cancellationToken = default )
  {
    using CancellationTokenSource taskCts = CancellationTokenSource.CreateLinkedTokenSource ( _snapdClientCancellationTokenSource.Token, cancellationToken );
    taskCts.Token.ThrowIfCancellationRequested ( );
    taskCts.CancelAfter ( timeout );

    HttpResponseMessage response =
      await _httpClient.PostAsJsonAsync (
                                         $"snaps/{name}",
                                         new ( revision ),
                                         SnapdRestApi.SnapApiJsonSerializerContext.Default.RemoveSnapsPostData,
                                         taskCts.Token
                                        )
                       .ConfigureAwait ( true );

    if ( response.IsSuccessStatusCode )
    {
      return await response.Content.ReadFromJsonAsync ( SnapdRestApi.SnapApiJsonSerializerContext.Default.RemoveSnapResult, taskCts.Token );
    }

    return null;
  }
}
