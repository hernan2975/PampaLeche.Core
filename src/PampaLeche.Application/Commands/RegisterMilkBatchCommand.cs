using System;
using MediatR;
using PampaLeche.Domain.Entities;
using PampaLeche.Domain.Enums;
using PampaLeche.Domain.ValueObjects;

namespace PampaLeche.Application.Commands;

public record RegisterMilkBatchCommand(
    string BatchCode,
    DateTime CollectionTime,
    double InitialTemp,
    double Fat,
    double Density,
    double Acidity,
    string ProducerCode,
    double Latitude,
    double Longitude,
    DestinationType Destination,
    DateTime? CoolingTime = null,
    double? StorageTemp = null
) : IRequest<Guid>;

public class RegisterMilkBatchCommandHandler : IRequestHandler<RegisterMilkBatchCommand, Guid>
{
    private readonly IRepository<MilkBatch> _repository;
    private readonly QualityControlService _qc;

    public RegisterMilkBatchCommandHandler(IRepository<MilkBatch> repository, QualityControlService qc)
    {
        _repository = repository;
        _qc = qc;
    }

    public async Task<Guid> Handle(RegisterMilkBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = MilkBatch.Create(
            request.BatchCode,
            request.CollectionTime,
            new Temperature(request.InitialTemp),
            new FatContent(request.Fat),
            request.Density,
            request.Acidity,
            new MilkOrigin(request.ProducerCode),
            new GeoLocation(request.Latitude, request.Longitude),
            request.Destination
        );

        if (request.CoolingTime.HasValue)
            batch.RegisterCooling(request.CoolingTime.Value, new Temperature(request.StorageTemp));

        await _qc.ProcessBatchAsync(batch);
        await _repository.AddAsync(batch);

        return batch.Id;
    }
}
