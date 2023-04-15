namespace DatabaseConfigurations.Models
{
    public class SerilogConfiguration : BaseConfiguration
    {
        public override string Name => "SerilogConfiguration";
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}