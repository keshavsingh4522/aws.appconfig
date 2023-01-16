namespace aws.appconfig.core.Data.Models;

public class TableDetails
{
    /// <summary>
    /// ApiKey
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Used for defining config details as unique
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    ///  Organisation id, which is the unique identifier for organization records.
    /// </summary>
    public string? OrganisationId { get; set; }

    /// <summary>
    /// Referrer id, which is the unique identifier for referrer records
    /// </summary>
    public string? ReferrerId { get; set; }

    /// <summary>
    /// Aws appconfig contains the profile in application.
    /// </summary>
    public string? ConfigProfile { get; set; }

    /// <summary>
    /// Aws appconfig contains the environment in application.
    /// </summary>
    public string? ConfigEnvironment { get; set; }
}
