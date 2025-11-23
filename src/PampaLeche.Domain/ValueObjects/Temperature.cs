namespace PampaLeche.Domain.ValueObjects;

public record Temperature(double Value)
{
    public static implicit operator double(Temperature t) => t.Value;
    public static implicit operator Temperature(double v) => new(v);
}
