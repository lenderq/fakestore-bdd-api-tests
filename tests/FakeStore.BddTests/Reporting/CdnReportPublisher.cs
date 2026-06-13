using Serilog;

namespace FakeStore.BddTests.Reporting;

public sealed class CdnReportPublisher
{
    private readonly string _cdnBaseUrl;

    public CdnReportPublisher(string cdnBaseUrl)
    {
        _cdnBaseUrl = cdnBaseUrl.TrimEnd('/');
    }

    public string Publish(string reportDirectory)
    {
        if (string.IsNullOrWhiteSpace(reportDirectory))
        {
            throw new ArgumentNullException("Report directory path must not be empty.", nameof(reportDirectory));
        }

        if (!Directory.Exists(reportDirectory))
        {
            throw new ArgumentException($"Report directory is not found: {reportDirectory}");
        }

        var filesCount = Directory.EnumerateFiles(reportDirectory, "*", SearchOption.AllDirectories).Count();

        var publishedUrl = $"{_cdnBaseUrl}/fake-store-bdd-tests/latest/index.html";

        Log.Information("Starting mock CDN upload for report directory: {ReportDirectory}", reportDirectory);
        Log.Information("Report files found: {FilesCount}", filesCount);
        Log.Information("Mock CDN upload completed. Report URL: {PublishedUrl}", publishedUrl);

        return publishedUrl;
    }
}
