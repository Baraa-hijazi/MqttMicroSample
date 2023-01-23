namespace MqttMicroSample;

public class MqttOptions
{
    public const string Mqtt = "Mqtt";

    public UserCredentials UserCredentials { get; set; } = null!;

    public int Port { get; set; }
}

public class UserCredentials
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}