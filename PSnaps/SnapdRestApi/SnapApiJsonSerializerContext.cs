#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

using PSnaps.SnapdRestApi.Requests;
using PSnaps.SnapdRestApi.Responses;

namespace PSnaps.SnapdRestApi;

[JsonSourceGenerationOptions (
                               GenerationMode = JsonSourceGenerationMode.Default,
                               RespectNullableAnnotations = true,
                               RespectRequiredConstructorParameters = true,
                               UseStringEnumConverter = true,
                               AllowOutOfOrderMetadataProperties = true,
                               UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode
                             )]
[JsonSerializable ( typeof( App[] ) )]
[JsonSerializable ( typeof( GetSnapsResponse ) )]
[JsonSerializable ( typeof( IHaveResult<ChangeSet> ) )]
[JsonSerializable ( typeof( IHaveResult<SnapPackage> ) )]
[JsonSerializable ( typeof( IHaveResult<SnapPackage[]> ) )]
[JsonSerializable ( typeof( InstallMultipleSnapsPostData ) )]
[JsonSerializable ( typeof( RemoveSingleSnapPostData ) )]
[JsonSerializable ( typeof( RemoveMultipleSnapsPostData ) )]
[JsonSerializable ( typeof( SnapApiResponse ) )]
[JsonSerializable ( typeof( SnapApiAsyncResponse ) )]
[JsonSerializable ( typeof( SnapApiErrorResponse ) )]
[JsonSerializable ( typeof( SnapApiSyncResponse ) )]
[JsonSerializable ( typeof( SnapPackage ) )]
[JsonSerializable ( typeof( SnapPackage[] ) )]
[JsonSerializable ( typeof( SnapsPostData ) )]
[JsonSerializable ( typeof( ConfinementKind ) )]
[UsedImplicitly]
public partial class SnapApiJsonSerializerContext : JsonSerializerContext;
