using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace MqttMicroSample;

public static class DefaultInfrastructureModule
{
    private static readonly MqttOptions Options = new();

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.GetSection(MqttOptions.Mqtt).Bind(Options);

        services.AddOptions<MqttOptions>().Bind(configuration.GetSection(MqttOptions.Mqtt));

        var optionsBuilder = new MqttServerOptionsBuilder()
            .WithDefaultEndpoint()
            .WithDefaultEndpointPort(Options.Port);

        var mqttServer = new MqttFactory().CreateMqttServer(optionsBuilder.Build());
        mqttServer.ValidatingConnectionAsync += ValidateConnectionAsync;
        mqttServer.StartAsync();
    }

    private static Task ValidateConnectionAsync(ValidatingConnectionEventArgs args)
    {
        try
        {
            var currentUser = Options.UserCredentials;

            if (args.UserName == null)
            {
                args.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                return Task.CompletedTask;
            }

            if (args.UserName != currentUser.UserName)
            {
                args.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                return Task.CompletedTask;
            }

            if (args.Password != currentUser.Password)
            {
                args.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                return Task.CompletedTask;
            }

            args.ReasonCode = MqttConnectReasonCode.Success;
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);
        }
    }
}