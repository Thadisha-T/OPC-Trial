using System;
using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;
using Workstation.ServiceModel.Ua.Channels;
					
public class Program
{
    public static async Task Main() {
        // Description of client application
        var clientDescription = new ApplicationDescription {
            ApplicationName = "Workstation.UaClient.FeatureTests",
            ApplicationUri = $"urn:{System.Net.Dns.GetHostName()}:Workstation.UaClient.FeatureTests",
            ApplicationType = ApplicationType.Client
        };

        // Create a client-side channel
        var channel = new ClientSessionChannel(
            clientDescription,
            null,
            new AnonymousIdentity(),
            "opc.tcp://localhost:4840",
            SecurityPolicyUris.None
        );

        try {
            await channel.OpenAsync();
            await channel.CloseAsync();
        }

        catch (Exception e) {
            await channel.AbortAsync();
            Console.WriteLine(e.Message);
        }
    }
}