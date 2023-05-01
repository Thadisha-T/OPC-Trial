using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System;

namespace TestClient
{
    class Program {
        private static readonly string SERVER_URL = "opc.tcp://172.31.1.236:4840/server/";
        private static readonly ApplicationConfiguration CONFIG = new ApplicationConfiguration() {
            ApplicationName = "Test-Client",
            ApplicationType = ApplicationType.Client,
            SecurityConfiguration = new SecurityConfiguration { ApplicationCertificate = new CertificateIdentifier() },
            TransportConfigurations = new TransportConfigurationCollection(),
            TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
            ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 }
        };

        public static async Task Main(string[] args) {
            // Validate Configuration
            await CONFIG.Validate(ApplicationType.Client);

            // Start Session
            using (Session session = await Session.Create(CONFIG, new ConfiguredEndpoint(null, new EndpointDescription(SERVER_URL)), true, "", 6000, null, null)) {
                // Initalise OPC UA Variables
                NodeId NODE_TEMP = new NodeId("ns=11;s=P1c_Extrude");

                // Program Loop
                while (true) {
                    // Polling Rate
                    await Task.Delay(500);

                    // Read Data
                    DataValue TEMP_DATA = await session.ReadValueAsync(NODE_TEMP);
                    Console.WriteLine("THE VALUE OF THE DATA = " + TEMP_DATA.ToString() + " DEGREES");
                }
            }
        }
    }
}
