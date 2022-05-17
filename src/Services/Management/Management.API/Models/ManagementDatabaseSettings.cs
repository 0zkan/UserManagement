namespace UserManagement.Services.Management.API.Models
{
    public class ManagementDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ManagementCollectionName { get; set; } = null!;
    }
}