namespace PampaLeche.Domain.ValueObjects;

public record FatContent(double Value)
{
    public static implicit operator double(FatContent f) => f.Value;
    public static implicit operator FatContent(double v) => new(v);
}
