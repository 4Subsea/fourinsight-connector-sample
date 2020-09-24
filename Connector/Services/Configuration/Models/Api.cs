namespace Sample.Services.Configuration.Models
{
    public class Api
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scopes { get; set; }
        public string Endpoint { get; set; }
    }
}