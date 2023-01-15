using Amazon.AppConfig;
using Amazon.AppConfigData;

namespace aws.appconfig.core.Services;

public class AppconfigService
{
    private readonly IAmazonAppConfigData _amazonAppConfigData;
    private readonly IAmazonAppConfig _amazonAppConfig;


    public AppconfigService(IAmazonAppConfigData amazonAppConfigData, IAmazonAppConfig amazonAppConfig)
    {
        _amazonAppConfigData = amazonAppConfigData;
        _amazonAppConfig = amazonAppConfig;
    }

    public void CreateApplication()
    {
        //_amazonAppConfig.CreateApplicationAsync();
    }
}
