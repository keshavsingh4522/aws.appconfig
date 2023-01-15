using Amazon.AppConfig.Model;

namespace aws.appconfig.core.Data.Models;

public class GetConfigurationProfileDetails
{
    public List<ConfigurationProfileSummary>? ConfigurationProfiles { get; set; }

    public string? NextToken { get; set; }
}
