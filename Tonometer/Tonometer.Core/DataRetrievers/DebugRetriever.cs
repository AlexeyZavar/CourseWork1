#region

using Tonometer.Core.DataRetrievers.Abstractions;
using Tonometer.Core.Models;

#endregion

namespace Tonometer.Core.DataRetrievers;

public sealed class DebugRetriever : IDataRetriever
{
    private MeasurementManager _manager = null!;
    private CancellationTokenSource _cts = null!;

    public void Start(MeasurementManager manager)
    {
        _manager = manager;
        _cts = new CancellationTokenSource();

        Task.Factory.StartNew(GenerateData, TaskCreationOptions.LongRunning);
    }

    private async Task GenerateData()
    {
        var generateWarningIdx = 0;

        while (!_cts.IsCancellationRequested)
        {
            var data = new RetrievedMeasureData
            {
                Time = DateTime.UtcNow,
                Sys = Random.Shared.Next(120, 140),
                Dia = Random.Shared.Next(80, 90),
                Pulse = Random.Shared.Next(60, 80)
            };

            if (generateWarningIdx++ > 5)
            {
                generateWarningIdx = 0;
                data.Pulse = 2000;
            }

            _manager.OnDataReceived(1, 1, data);

            await Task.Delay(3000);
        }
    }

    public void Stop()
    {
        _cts.Cancel();
    }
}
