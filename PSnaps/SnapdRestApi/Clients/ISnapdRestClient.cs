#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using PSnaps.SnapdRestApi.Responses;

namespace PSnaps.SnapdRestApi.Clients;

public interface ISnapdRestClient : IDisposable
{
  public const string DefaultApiBaseUriV2        = "http://localhost/v2/";
  public const string DefaultSnapdUnixSocketPath = "unix:///run/snapd.socket";

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
  Task<SnapPackage[]?> GetAllSnapsAsync ( int timeout = 30000, CancellationToken cancellationToken = default );

  Task<ChangeSet?> GetChangesAsync ( string actionId, int timeout = 30000, CancellationToken cancellationToken = default );

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

  Task<SnapApiResponse?> InstallMultipleSnapsAsync ( string[] snapNames, TransactionMode transactionMode = TransactionMode.PerPackage, bool restartIfRequired = false, int timeout = 30000, CancellationToken cancellationToken = default );

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
