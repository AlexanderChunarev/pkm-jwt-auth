using System.Text.Json.Serialization;

namespace PKM.Model
{
    public class UserView
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}