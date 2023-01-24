using aws.appconfig.core.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace AWSServerless1.Controllers;

/// <summary>
/// Api details
/// </summary>
[Route("api")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
public class ValuesController : ControllerBase
{
    /// <summary>
    /// Get whole records of employee.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Employee
    ///     {
    ///       "firstName": "Test",
    ///       "lastName": "Name",
    ///       "emailId": "Test.Name@gmail.com"
    ///     }
    /// </remarks>
    /// <returns>List of employee.</returns>
    /// <response code="200">Returns the items</response>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">If the item is null</response>
    [HttpGet]
    public IEnumerable<Employee> Get()
    {
        return new List<Employee>() {
            new Employee() { Id = 1, EmailId="k@gamil.com", FirstName="FName",LastName="LName"}
        };
    }
}