#region

using Tonometer.Database;

#endregion

namespace Tonometer.Web.Core.DataExport.Abstractions;

public interface IDataExporter
{
    Task<Stream> Export(TonometerContext context, int patientId);
}
