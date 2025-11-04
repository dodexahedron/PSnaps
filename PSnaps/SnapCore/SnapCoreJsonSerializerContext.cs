#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapCore;

using System.Text.Json.Serialization;

/// <summary>
///   Provides compile-time source generated serialization implementations for types in the PSnaps.SnapCore namespace.
/// </summary>
/// <remarks>
///   This context is meant for use in scenarios that don't care about the REST client request- and response-related types.
/// </remarks>
[JsonSourceGenerationOptions (
                               GenerationMode = JsonSourceGenerationMode.Default,
                               RespectNullableAnnotations = true,
                               UseStringEnumConverter = true
                             )]
[JsonSerializable ( typeof( App ) )]
[JsonSerializable ( typeof( Confinement ) )]
[JsonSerializable ( typeof( DaemonKind ) )]
[JsonSerializable ( typeof( Links ) )]
[JsonSerializable ( typeof( PackageStatus ) )]
[JsonSerializable ( typeof( Publisher ) )]
public partial class SnapCoreJsonSerializerContext : JsonSerializerContext;
