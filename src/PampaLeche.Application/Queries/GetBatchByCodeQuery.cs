using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PampaLeche.Domain.Entities;

namespace PampaLeche.Application.Queries;

public record GetBatchByCodeQuery(string BatchCode) : IRequest<MilkBatch?>;

public class GetBatchByCodeQueryHandler : IRequestHandler<GetBatchByCodeQuery, MilkBatch?>
{
    private readonly IRepository<MilkBatch> _repository;

    public GetBatchByCodeQueryHandler(IRepository<MilkBatch> repository)
    {
        _repository = repository;
    }

    public async Task<MilkBatch?> Handle(GetBatchByCodeQuery request, CancellationToken cancellationToken)
    {
        // Placeholder: en implementación real se buscaría por código
        return null;
    }
}
