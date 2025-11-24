using Microsoft.AspNetCore.Mvc;
using PampaLeche.Application.Services;
using PampaLeche.Domain.Entities;
using PampaLeche.Domain.Enums;
using PampaLeche.Domain.ValueObjects;
using PampaLeche.Infrastructure.Messaging;

namespace PampaLeche.Presentation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BatchesController : ControllerBase
{
    private readonly QualityControlService _qc;
    private readonly IEventPublisher _eventPublisher;

    public BatchesController(QualityControlService qc, IEventPublisher eventPublisher)
    {
        _qc = qc;
        _eventPublisher = eventPublisher;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBatchRequest request)
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
        await _eventPublisher.PublishAsync(new DomainEvents.BatchCreatedEvent(batch.Id, batch.BatchCode));

        return CreatedAtAction(nameof(Get), new { id = batch.Id }, new
        {
            batch.Id,
            batch.BatchCode,
            batch.Status,
            batch.CollectionTime
        });
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        // Placeholder: en producción se obtendría de repositorio
        return Ok(new { id, status = "Accepted" });
    }
}

public record CreateBatchRequest(
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
);
