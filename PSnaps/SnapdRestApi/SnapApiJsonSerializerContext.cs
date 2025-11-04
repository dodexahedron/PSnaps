#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.SnapdRestApi;

using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Requests;
using Responses;
using SnapCore;

[JsonSourceGenerationOptions (
                               GenerationMode = JsonSourceGenerationMode.Default,
                               RespectNullableAnnotations = true,
                               UseStringEnumConverter = true
                             )]
[JsonSerializable ( typeof( GetSnapsResponse ) )]
[JsonSerializable ( typeof( GetSnapsResponseResult[] ) )]
[JsonSerializable ( typeof( App[] ) )]
[JsonSerializable ( typeof( SnapsPostData ) )]
[JsonSerializable ( typeof( RemoveSnapsPostData ) )]
[JsonSerializable ( typeof( SnapApiResponse ) )]
[JsonSerializable ( typeof( SnapApiAsyncResponse ) )]
[JsonSerializable ( typeof( SnapApiSyncResponse ) )]
[JsonSerializable ( typeof( RemoveSnapResult ) )]
[JsonSerializable ( typeof( GetChangesResponse ) )]
[UsedImplicitly]
public partial class SnapApiJsonSerializerContext : JsonSerializerContext;
