using Amazon.AppConfig;
using Amazon.AppConfig.Model;
using aws.appconfig.core.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace aws.appconfig.api.Controllers;

[ApiController]
[Route("AppConfig/V1")]
[Produces("application/json")]
public class AppConfigController : ControllerBase
{
    private readonly IAmazonAppConfig _amazonAppConfig;

    public AppConfigController(IAmazonAppConfig amazonAppConfig)
    {
        _amazonAppConfig = amazonAppConfig;
    }

    #region Get
    [HttpGet]
    public async Task<IActionResult> ListApplicationsAsync([FromQuery] string? nextToken)
    {
        var response = await _amazonAppConfig.ListApplicationsAsync(new ListApplicationsRequest()
        {
            MaxResults = 10,
            NextToken = nextToken
        });

        GetApplicationDetailsResponse getApplicationDetailsResponse = new()
        {
            Applications = response.Items,
            NextToken = response.NextToken
        };

        Response.StatusCode = (int)response.HttpStatusCode;
        return new ObjectResult(getApplicationDetailsResponse);
    }

    [HttpGet("{applicationId}")]
    public async Task<IActionResult> ListConfigurationProfilesAsync([FromRoute] string applicationId, [FromQuery] string? nextToken)
    {
        var response = await _amazonAppConfig.ListConfigurationProfilesAsync(new ListConfigurationProfilesRequest()
        {
            MaxResults = 50,
            NextToken = nextToken,
            ApplicationId = applicationId
        });

        GetConfigurationProfileDetails getConfigurationProfileDetails = new()
        {
            ConfigurationProfiles = response.Items,
            NextToken = response.NextToken
        };

        Response.StatusCode = (int)response.HttpStatusCode;
        return new ObjectResult(getConfigurationProfileDetails);
    }
    #endregion

    #region Create
    [HttpPost("{applicationName}")]
    public async Task<IActionResult> Create([FromRoute] string applicationName)
    {
        // Check profile exist or not.

        //GetApplicationResponse getApplicationResponse = await _amazonAppConfig.GetApplicationAsync(
        //    new Amazon.AppConfig.Model.GetApplicationRequest()
        // {
        //      ApplicationId= applicationName
        // });

        // if(string.IsNullOrWhiteSpace(getApplicationResponse.Id)) {

        //     Response.StatusCode = (int)HttpStatusCode.Forbidden;
        //     return new ObjectResult("Application name already exist.");
        // }

        var response = await _amazonAppConfig.CreateApplicationAsync(new Amazon.AppConfig.Model.CreateApplicationRequest()
        {
            Description = "--demo--",
            Name = applicationName,
            //Tags = new Dictionary<string, string> { { "", "" } }
        });

        Response.StatusCode = (int)response.HttpStatusCode;
        return new ObjectResult(response);
    }

    [HttpPost("{applicationId}/{profileName}")]
    public async Task<IActionResult> CreateProfile([FromRoute] string applicationId, [FromRoute] string profileName)
    {
        var response = await _amazonAppConfig.CreateConfigurationProfileAsync(new CreateConfigurationProfileRequest()
        {
            Name = profileName,
            Type = "AWS.Freeform",
            ApplicationId = applicationId,
            Description = "--demo-profile--",
            LocationUri = "hosted",
        });

        Response.StatusCode = (int)response.HttpStatusCode;
        return new ObjectResult(response);
    }
    #endregion
}