using Amazon.AppConfig;
using Amazon.AppConfig.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using aws.appconfig.core.Data.Models;
using aws.appconfig.core.Helper;
using Microsoft.AspNetCore.Mvc;

namespace aws.appconfig.api.Controllers;

[Route("dynamodb/v1")]
[ApiController]
public class DynamodbController : ControllerBase
{
    private readonly IAmazonDynamoDB _amazonDynamoDB;
    private readonly IAmazonAppConfig _amazonAppConfig;


    public DynamodbController(IAmazonDynamoDB amazonDynamoDB,IAmazonAppConfig amazonAppConfig)
    {
        _amazonDynamoDB = amazonDynamoDB;
        _amazonAppConfig= amazonAppConfig;
    }

    #region Get
    [HttpPost("{tableName}/{apiKey}")]
    public async Task<IActionResult> GetItemAsync([FromRoute] string tableName, [FromRoute] string apiKey)
    {
        var response = await _amazonDynamoDB.GetItemAsync(new GetItemRequest()
        {
            TableName = tableName,
            AttributesToGet = new List<string> { "ClientName", "OrganisationId", "ReferrerId", "ConfigProfile", "ConfigEnvironment" },
            Key = new Dictionary<string, AttributeValue>
                {
                    {"ApiKey", new AttributeValue {S = apiKey}}
                }
        });

        Response.StatusCode = (int)response.HttpStatusCode;
        return new ObjectResult(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrg([FromQuery]string orgName,[FromQuery]string environment)
    {
        string clientName = orgName.Replace(" ", "_");
        string profileName = orgName.ConvertToProfileName(environment);
        string apiKey = Guid.NewGuid().ToString("N").ToUpper();
        string tableName= "micr-"+ environment.ToLower();
        // Get org id from sf
        string orgId = "N/A";

        // Get billing scheme details from salesforce
        string referrid = "N/A";

        var response = await _amazonDynamoDB.PutItemAsync(new PutItemRequest()
        {
            TableName = tableName,
            Item = new Dictionary<string, AttributeValue> {
                { nameof(TableDetails.ApiKey), new AttributeValue() { S = apiKey } },
                { nameof(TableDetails.ConfigProfile), new AttributeValue() { S = profileName } },
                { nameof(TableDetails.OrganisationId), new AttributeValue() { S = orgId } },
                { nameof(TableDetails.ClientName), new AttributeValue() { S = clientName } },
                { nameof(TableDetails.ConfigEnvironment), new AttributeValue() { S = environment } },
                { nameof(TableDetails.ReferrerId), new AttributeValue() { S = referrid } },
            }
        });
        var applicationLists = await _amazonAppConfig.ListApplicationsAsync(new ListApplicationsRequest()
        {
             MaxResults= 50,
        });
        var applicationId = applicationLists.Items.Where(x => x.Name=="Microservice").FirstOrDefault()?.Id;
        
        var envList = await _amazonAppConfig.ListEnvironmentsAsync(new ListEnvironmentsRequest()
        {
           ApplicationId= applicationId,
          MaxResults= 50,
        });
        string? envId = envList.Items.Where(x=>x.Name==environment).FirstOrDefault()?.Id;
        
        var response2 = await _amazonAppConfig.CreateConfigurationProfileAsync(new CreateConfigurationProfileRequest()
        {
            Name = profileName,
            Type = "AWS.Freeform",
            ApplicationId = applicationId,
            Description = "--demo-profile--",
            LocationUri = "hosted",
        });

        string content = "{\"FirstName\":\"Keshav\",\"LastName\":\"Singh\",\"Branch\":\"CSE\",\"Batch\":\"B-1\",\"Country\":\"India\",\"State\":\"Raj\",\"District\":\"Dholpur\",\"Village\":\"Dharapura\",\"Post\":\"Khudila\"}";
        await _amazonAppConfig.CreateHostedConfigurationVersionAsync(new CreateHostedConfigurationVersionRequest()
        {
             ApplicationId= applicationId,
             ConfigurationProfileId= response2.Id,
             ContentType= "application/json",
             Content= content.ConvertToMemoryStream(),
             LatestVersionNumber=1
        });

        var response4 = await _amazonAppConfig.ListDeploymentStrategiesAsync(new ListDeploymentStrategiesRequest() { MaxResults=50 });
        string? deploymentId = response4.Items.Where(x=>x.Name== "MyTestDeploymentStrategy").FirstOrDefault()?.Id;

        // Deploy the application
        var response3 = await _amazonAppConfig.StartDeploymentAsync(new StartDeploymentRequest()
        {
             ApplicationId= applicationId,
              ConfigurationProfileId=response2.Id,
             ConfigurationVersion= "1",
             DeploymentStrategyId=deploymentId,
             EnvironmentId= envId,
             Description="--------testing------",
        });

        Response.StatusCode = (int)response.HttpStatusCode;


        return new ObjectResult(response);
    }
    #endregion

    #region Create
    [HttpPost("{tableName}")]
    public async Task<IActionResult> CreateTableAsync([FromRoute] string tableName)
    {
        var response = await _amazonDynamoDB.CreateTableAsync(new CreateTableRequest()
        {
            TableName = tableName,
            KeySchema = new List<KeySchemaElement>()
             {
                 new KeySchemaElement(){ AttributeName="ApiKey",KeyType=KeyType.HASH},
             },
            AttributeDefinitions = new List<AttributeDefinition>()
              {
                  new AttributeDefinition()
                  {
                       AttributeName= "ApiKey",
                     AttributeType =ScalarAttributeType.S
                  }//,
                  //new AttributeDefinition()
                  //{
                  //     AttributeName= "Profile",
                  //   AttributeType =ScalarAttributeType.S
                  //},
                  //new AttributeDefinition()
                  //{
                  //     AttributeName= "Env",
                  //   AttributeType =ScalarAttributeType.S
                  //}
              },
            BillingMode = BillingMode.PROVISIONED,
            ProvisionedThroughput = new ProvisionedThroughput()
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
            },
        });

        Response.StatusCode = (int)response.HttpStatusCode;
        return new ObjectResult(response);
    }
    #endregion
}