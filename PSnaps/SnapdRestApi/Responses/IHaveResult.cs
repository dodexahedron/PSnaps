#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi.Responses;

/// <summary>
///   A generic interface enabling type-specific handling of implementing responses that have a <see cref="Result" /> property.
/// </summary>
/// <typeparam name="T">
///   The type of the <see cref="Result" /> property.<br />
///   This type parameter is covariant.
/// </typeparam>
public interface IHaveResult<out T>
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
  [JsonPropertyName ( "result" )]
  public T? Result { get; }
}
