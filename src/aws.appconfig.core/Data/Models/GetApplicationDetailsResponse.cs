using Amazon.AppConfig.Model;

namespace aws.appconfig.core.Data.Models;

public class GetApplicationDetailsResponse
{
    public List<Application>? Applications { get; set; }

    public string? NextToken { get; set; }
}
