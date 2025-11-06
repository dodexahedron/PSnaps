#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Responses;

public interface IHaveStatus<out TStatusCode, out TStatus> : IHaveStatus<TStatus>
  where TStatusCode : unmanaged
  where TStatus : notnull
{
  TStatusCode StatusCode { get; }
}

public interface IHaveStatus<out TStatus>
  where TStatus : notnull
{
  TStatus Status { get; }
}
