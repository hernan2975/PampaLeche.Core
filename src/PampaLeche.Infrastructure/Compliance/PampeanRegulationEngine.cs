using PampaLeche.Domain.Entities;
using PampaLeche.Domain.ValueObjects;

namespace PampaLeche.Infrastructure.Compliance;

public class PampeanRegulationEngine
{
    public bool IsCompliant(MilkBatch batch)
    {
        if (!batch.FarmLocation.IsWithinPampa()) return false;
        if (!batch.Origin.ProducerCode.StartsWith("LP-")) return false;
        return true;
    }
}
