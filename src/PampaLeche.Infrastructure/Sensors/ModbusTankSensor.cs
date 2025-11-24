using System;
using System.Threading.Tasks;
using EasyModbus;
using PampaLeche.Domain.ValueObjects;

namespace PampaLeche.Infrastructure.Sensors;

public class ModbusTankSensor : ISensorAdapter
{
    private readonly string _ip;
    private readonly int _port;

    public ModbusTankSensor(string ip = "192.168.1.100", int port = 502)
    {
        _ip = ip;
        _port = port;
    }

    public async Task<Temperature> ReadTankTemperatureAsync()
    {
        try
        {
            using var client = new ModbusClient(_ip, _port);
            await Task.Run(() => client.Connect());

            // Registro 40001: temperatura * 10 (ej: 38 = 3.8°C)
            var registers = await Task.Run(() => client.ReadHoldingRegisters(0, 1));
            var temp = registers[0] / 10.0;

            return new Temperature(temp);
        }
        catch
        {
            throw new InvalidOperationException("No se pudo leer el sensor Modbus.");
        }
    }
}
