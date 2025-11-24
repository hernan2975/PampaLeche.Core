using PampaLeche.Domain.Entities;
using PampaLeche.Infrastructure.Compliance;

namespace PampaLeche.Application.Services;

public class ComplianceService
{
    private readonly PampeanRegulationEngine _engine;

    public ComplianceService(PampeanRegulationEngine engine)
    {
        _engine = engine;
    }

    public bool IsBatchCompliant(MilkBatch batch)
    {
        return _engine.IsCompliant(batch);
    }
}
