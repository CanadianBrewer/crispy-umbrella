namespace DatabaseConfigurations.Models
{
    public class FooConfiguration : BaseConfiguration
    {
        public override string Name => "FooConfiguration";
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}