namespace Play.Catalogue.Service.Settings
{
    public class RabbitMQSettingsOption
    {
        public const string RabbitMQSettings = "RabbitMQSettings";
        //no one should be setting this property after being deserialized from the configuration file
        public string Host { get; init; }
    }
}
