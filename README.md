# PampaLeche.Core

Sistema de trazabilidad y control de calidad para lácteos en La Pampa.  
Diseñado para operar en entornos industriales con conectividad limitada (offline-first).

## Características

- Modelo de dominio rico según normativa SENASA y DNI La Pampa  
- Validación automática de parámetros: temperatura, grasa, acidez, tiempo de enfriamiento  
- Generación de certificados de calidad y reportes para SENASA  
- Integración con sensores (Modbus/RS485) o modo simulado  
- CLI para ejecución en planta (Windows/Linux/ARM64)  
- Arquitectura limpia: Domain → Application → Infrastructure → Presentation

## Requisitos

- .NET 8 SDK  
- (Opcional) Dispositivo con puerto serie o Modbus TCP para sensores reales

## Uso

```bash
dotnet run --project src/PampaLeche.Presentation/Cli
```
Licencia
MIT — Uso libre en establecimientos lácteos de La Pampa.
