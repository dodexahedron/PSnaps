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

/// <summary>
///   Represents a high-level API "task," which typically corresponds to a single high-level user command, and may contain one or
///   more "actions," with <see cref="Progress" /> reporting available per action.
/// </summary>
[PublicAPI]
public record ApiTask
{
  [JsonPropertyName ( "data" )]
  public TaskData? Data { get; set; }

  [JsonPropertyName ( "id" )]
  public required string Id { get; set; }

  [JsonPropertyName ( "kind" )]
  public string? Kind { get; set; }

  [JsonPropertyName ( "progress" )]
  public TaskProgress? Progress { get; set; }

  [JsonPropertyName ( "ready-time" )]
  public DateTimeOffset ReadyTime { get; set; }

  [JsonPropertyName ( "spawn-time" )]
  public DateTimeOffset SpawnTime { get; set; }

  [JsonPropertyName ( "status" )]
  public string? Status { get; set; }

  [JsonPropertyName ( "summary" )]
  public string? Summary { get; set; }

  /// <summary>
  ///   Indicates the progress of an <see cref="ApiTask" />, including total number of actions and how many are completed at the time
  ///   of the request.
  /// </summary>
  [PublicAPI]
  public class TaskProgress
  {
    /// <summary>
    ///   The number of actions in the task which have been completed.
    /// </summary>
    [JsonPropertyName ( "done" )]
    public int Done { get; set; }

    [JsonPropertyName ( "label" )]
    public string? Label { get; set; }

    /// <summary>
    ///   The percentage (as a <see langword="double" /> from 0 to 1) of the actions of the task which have been completed.
    /// </summary>
    /// <remarks>This property is not serialized to or from JSON.</remarks>
    [JsonIgnore]
    public double PercentComplete => Done * 1d / ( Total * 1d );

    /// <summary>
    ///   The total number of actions in the task.
    /// </summary>
    [JsonPropertyName ( "total" )]
    public int Total { get; set; }
  }

  [PublicAPI]
  public class TaskData
  {
    [JsonPropertyName ( "affected-snaps" )]
    public string[]? AffectedSnaps { get; set; }
  }
}
