namespace PampaLeche.Domain.ValueObjects;

public record GeoLocation(double Latitude, double Longitude)
{
    public bool IsWithinPampa() =>
        Latitude is >= -39.5 and <= -35.5 &&
        Longitude is >= -68.5 and <= -63.5;
}
