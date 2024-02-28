#region

using System.Xml;
using Microsoft.EntityFrameworkCore;
using Tonometer.Database;
using Tonometer.Web.Core.DataExport.Abstractions;

#endregion

namespace Tonometer.Web.Core.DataExport;

public sealed class XmlExporter : IDataExporter
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
        var settings = new XmlWriterSettings
        {
            Async = true,
            Indent = true
        };

        await using var writer = XmlWriter.Create(ms, settings);

        await writer.WriteStartDocumentAsync();
        await writer.WriteStartElementAsync(null, "measurements", null);

        foreach (var measurement in measurements)
        {
            await writer.WriteStartElementAsync(null, "measurement", null);
            await writer.WriteElementStringAsync(null, "time", null, measurement.Time.ToString("O"));
            await writer.WriteElementStringAsync(null, "sys", null, measurement.Sys.ToString());
            await writer.WriteElementStringAsync(null, "dia", null, measurement.Dia.ToString());
            await writer.WriteElementStringAsync(null, "pulse", null, measurement.Pulse.ToString());
            await writer.WriteEndElementAsync();
        }

        await writer.WriteEndElementAsync();
        await writer.WriteEndDocumentAsync();

        return ms;
    }
}
