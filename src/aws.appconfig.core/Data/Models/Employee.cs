using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace aws.appconfig.core.Data.Models;

/// <summary>
/// Contains the employee information.
/// </summary>
public class Employee
{
    /// <summary>
    /// Unique id of employee.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// First name of employee
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name of employee
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Email id of user
    /// </summary>
    [Required]
    public string? EmailId { get; set; }

    /// <summary>
    /// Profile complted or not.
    /// </summary>
    [DefaultValue(false)]
    public bool IsComplete { get; set; }
}