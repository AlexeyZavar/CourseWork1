namespace Tonometer.Core.DataRetrievers.Abstractions;

public interface IDataRetriever
{
    void Start(MeasurementManager manager);
    void Stop();
}
