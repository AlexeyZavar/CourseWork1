#region

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Tonometer.Database;
using Tonometer.Web.Core.DataExport.Abstractions;

#endregion

namespace Tonometer.Web.Core.DataExport;

public sealed class JsonExporter : IDataExporter
{
    /// <inheritdoc />
    public async Task<Stream> Export(TonometerContext context, int patientId)
    {
        // todo: paginated export
        var measurements = await context.MeasureData
                                        .Where(x => x.PatientId == patientId)
                                        .OrderBy(x => x.Time)
                                        .Select(x => new
                                        {
                                            x.Time,
                                            x.Sys,
                                            x.Dia,
                                            x.Pulse
                                        })
                                        .ToListAsync();

        var ms = new MemoryStream();

        await JsonSerializer.SerializeAsync(ms, measurements);

        return ms;
    }
}
