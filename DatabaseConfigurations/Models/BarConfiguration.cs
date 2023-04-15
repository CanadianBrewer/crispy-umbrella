namespace DatabaseConfigurations.Models
{
    public class BarConfiguration : BaseConfiguration
    {
        public override string Name => "BarConfiguration";
        public string BaseUrl { get; set; }
        public string UserId { get; set; }
        public string ApiKey { get; set; }
        public string CustomHeaderName { get; set; }
        public string CustomHeaderSalt { get; set; }
    }
}