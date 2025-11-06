namespace PSnaps.SnapCore;

public sealed record AppSnap : SnapPackage
{
  [JsonPropertyName ( "apps" )]
  public required App[] Apps { get; set; }

  [JsonPropertyName ( "base" )]
  public required string Base { get; set; }

  [JsonPropertyName ( "released-at" )]
  public DateTimeOffset? ReleasedAt { get; set; }
}