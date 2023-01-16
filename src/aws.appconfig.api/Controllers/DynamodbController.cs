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

    public DynamodbController(IAmazonDynamoDB amazonDynamoDB)
    {
        _amazonDynamoDB = amazonDynamoDB;
    }

    #region get
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
        string tableName= "micr-"+ environment;
        // Get org id from sf
        string orgId = "N/A";

        // get billing scheme details from salesforce
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
