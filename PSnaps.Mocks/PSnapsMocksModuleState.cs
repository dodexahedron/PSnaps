#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using PSnaps.SnapdRestApi.Clients;

namespace PSnaps.Mocks;

/// <summary>
///   Module-global state container for the PSnaps.Mocks module.
/// </summary>
[PublicAPI]
public static class PSnapsMocksModuleState
{
  private static ISnapdRestClient? _previousSnapdClient;

  /// <summary>
  ///   Instructs the PSnaps module to use the provided <paramref name="newClient" /> as its snapd REST client, and stores the previous
  ///   client to enable reset.
  /// </summary>
  /// <param name="newClient">
  ///   A reference to an <see cref="ISnapdRestClient" /> object that is fully constructed and ready for use.
  /// </param>
  /// <returns>The previous client object, if any, or <see langword="null" />.</returns>
  internal static ISnapdRestClient? ExchangeSnapdClient ( ISnapdRestClient newClient )
  {
    return _previousSnapdClient = PSnapsModuleState.ExchangeSnapdClient ( newClient );
  }
}
