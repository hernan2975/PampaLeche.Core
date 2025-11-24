using System.Threading.Tasks;
using PampaLeche.Domain.ValueObjects;

namespace PampaLeche.Infrastructure.Sensors;

public interface ISensorAdapter
{
    Task<Temperature> ReadTankTemperatureAsync();
}
