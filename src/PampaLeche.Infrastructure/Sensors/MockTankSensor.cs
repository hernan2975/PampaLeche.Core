using System.Threading.Tasks;
using PampaLeche.Domain.ValueObjects;

namespace PampaLeche.Infrastructure.Sensors;

public class MockTankSensor : ISensorAdapter
{
    public Task<Temperature> ReadTankTemperatureAsync() =>
        Task.FromResult(new Temperature(3.8));
}
