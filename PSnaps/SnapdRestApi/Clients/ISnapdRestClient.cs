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
///   Interface representing the minimum functionality expected of a snapd client, by the library and module cmdlets.
/// </summary>
[PublicAPI]
public interface ISnapdRestClient : IDisposable
{
  /// <summary>
  ///   The base URI that snapd uses for the v2 API.<br />
  ///   All requests go to URIs below this base URI.
  /// </summary>
  public const string DefaultApiBaseUriV2 = "http://localhost/v2/";

  /// <summary>
  ///   The URI to the default snapd Unix Domain Socket listener on Ubuntu.
  /// </summary>
  public const string DefaultSnapdUnixSocketPath = "unix:///run/snapd.socket";

  /// <summary>
  ///   Gets the URI of the snapd Unix Domain Socket.
  /// </summary>
  Uri SnapdUnixSocketUri { get; }

  /// <summary>
  ///   Gets all snap packages via the <c>/v2/snaps?select=all</c> API call.
  /// </summary>
  /// <param name="timeout">
  ///   A global timeout for the operation, in milliseconds. If not specified, default is 10 seconds.
  /// </param>
  /// <param name="cancellationToken">
  ///   A <see cref="CancellationToken" /> for the operation to use. If not specified, the default implementation in
  ///   <see cref="SnapdClient" /> uses the client's own <see cref="CancellationTokenSource" /> to provide the token, which is linked
  ///   to the token the client was created with.
  /// </param>
  /// <returns>
  ///   An array of <see cref="SnapPackage" /> objects, deserialized from the <c>result</c> property of the API's JSON response.
  /// </returns>
  Task<SnapPackage[]?> GetAllSnapsAsync ( int timeout = 10000, CancellationToken cancellationToken = default );

  /// <summary>
  ///   Gets a <see cref="ChangeSet" />, by id, providing status of a previously-submitted snapd async request, such as most requests
  ///   that make a change.
  /// </summary>
  /// <param name="actionId">
  ///   The identifier of the action, as returned in a <see cref="SnapApiAsyncResponse" />, as a <see cref="string" />.
  /// </param>
  /// <param name="timeout">A timeout, in milliseconds, for the GET request. Default is 10 seconds.</param>
  /// <param name="cancellationToken">
  ///   A cancellation token to use for the operation, which will also be linked to the client's cancellation token source.
  /// </param>
  Task<ChangeSet?> GetChangesAsync ( string actionId, int timeout = 10000, CancellationToken cancellationToken = default );

  /// <summary>
  ///   Gets a single snap package by name, optionally including inactive versions if <paramref name="includeInactive" /> is
  ///   <see langword="true" />.
  /// </summary>
  /// <param name="snapName">The name of the snap package. This parameter is not case sensitive.</param>
  /// <param name="includeInactive">
  ///   If <see langword="true" />, returns all local instances of the snap package having status of either <c>active</c> or
  ///   <c>installed</c> (inactive).<br />
  ///   If <see langword="false" /> (default), only returns the <c>active</c> instance of the snap package.
  /// </param>
  /// <param name="timeout">A timeout, in milliseconds, for the GET request. Default is 10 seconds.</param>
  /// <param name="cancellationToken">
  ///   A cancellation token to use for the operation, which will also be linked to the client's cancellation token source.
  /// </param>
  /// <returns>
  ///   An array of <see cref="SnapPackage" /> (even if there is only one result) or <see langword="null" />, if there were no results.
  /// </returns>
  Task<SnapPackage[]?> GetSingleSnapAsync ( string snapName, bool includeInactive = false, int timeout = 10000, CancellationToken cancellationToken = default );

  /// <summary>
  ///   Gets the specified packages via the <c>/v2/snaps</c> API call.
  /// </summary>
  /// <param name="snapNames">The names of the packages to return.</param>
  /// <param name="includeInactive">If <see langword="true" />, include inactive snaps in the result set.</param>
  /// <param name="timeout">
  ///   A global timeout for the operation, in milliseconds. If not specified, default is 10 seconds.
  /// </param>
  /// <param name="cancellationToken">
  ///   A <see cref="CancellationToken" /> for the operation to use. If not specified, the default implementation in
  ///   <see cref="SnapdClient" /> uses the client's own <see cref="CancellationTokenSource" /> to provide the token, which is linked
  ///   to the token the client was created with.
  /// </param>
  /// <returns>
  ///   An array of <see cref="SnapPackage" /> objects, deserialized from the <c>result</c> property of the API's JSON response.
  /// </returns>
  Task<List<SnapPackage>?> GetSnapsAsync ( string[] snapNames, bool includeInactive = false, int timeout = 10000, CancellationToken cancellationToken = default );

  /// <summary>
  ///   Installs all of the specified snap packages.
  /// </summary>
  /// <param name="snapNames">The names of the packages to install.</param>
  /// <param name="transactionMode">
  ///   Specifies the transaction mode to use for the installation operation.<br />
  ///   If <see cref="TransactionMode.PerPackage" /> (default for PSnaps as well as snapd), each package installation uses an isolated
  ///   transaction and failures do not affect other installations (barring dependency issues).<br />
  ///   If <see cref="TransactionMode.AllPackage" />, then all packages are installed in one transaction, and all must succeed or else
  ///   all installations will be rolled back.
  /// </param>
  /// <param name="timeout"></param>
  /// <param name="cancellationToken"></param>
  /// <returns>
  ///   A <see cref="SnapApiAsyncResponse" /> containing the change ID, which can be used to retrieve status of the operation.
  /// </returns>
  Task<SnapApiAsyncResponse?> InstallMultipleSnapsAsync ( string[] snapNames, TransactionMode transactionMode = TransactionMode.PerPackage, int timeout = 30000, CancellationToken cancellationToken = default );

  /// <summary>
  ///   Removes multiple snap packages via a POST to the <c>/v2/snaps</c> API endpoint.
  /// </summary>
  /// <param name="snapNames">The names of the packages to remove.</param>
  /// <param name="purge">Whether to purge any snapshot data for the removed snaps.</param>
  /// <param name="timeout">The timeout for the operation in milliseconds. Default is 10 seconds.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> for the operation.</param>
  /// <returns>
  ///   A <see cref="SnapApiResponse" /> containing the change ID, which can be used to retrieve status of the operation.
  /// </returns>
  /// <remarks>
  ///   <para>
  ///     <c>snapd</c> handles resolving the proper order to remove packages with dependencies.<br />
  ///     However, if any of the specified snaps cannot be removed due to dependency constraints, behavior is dependent upon the
  ///     transaction mode.
  ///   </para>
  ///   <para>
  ///     This operation is asynchronous in the snapd API and should return nearly instantly. Lengthy timeouts are not generally
  ///     useful.
  ///   </para>
  /// </remarks>
  Task<SnapApiAsyncResponse?> RemoveMultipleSnapsAsync ( string[] snapNames, bool purge = false, int timeout = 10000, CancellationToken cancellationToken = default );

  /// <summary>
  ///   Removes an individual snap package revision via the <c>/v2/snaps/{name}</c> API call.
  /// </summary>
  /// <param name="name">The name of the package to remove.</param>
  /// <param name="revision">The revision of the package to remove.</param>
  /// <param name="purge">Whether to purge any snapshot data for the removed snap revision.</param>
  /// <param name="timeout">The timeout for the operation in milliseconds. Default is 10 seconds.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> for the operation.</param>
  /// <returns>
  ///   A <see cref="SnapApiResponse" /> containing the change ID, which can be used to retrieve status of the operation.
  /// </returns>
  /// <remarks>
  ///   This operation is asynchronous in the snapd API and should return nearly instantly. Lengthy timeouts are not generally useful.
  /// </remarks>
  Task<SnapApiAsyncResponse?> RemoveSnapAsync ( string name, string revision, bool purge = false, int timeout = 10000, CancellationToken cancellationToken = default );
}
