using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System;

namespace TestClient
{
    class Program {
        private static readonly string SERVER_URL = "opc.tcp://127.0.0.1:4840/UA";
        private static readonly ApplicationConfiguration CONFIG = new ApplicationConfiguration() {
            ApplicationName = "Test-Client",
            ApplicationType = ApplicationType.Client,
            SecurityConfiguration = new SecurityConfiguration { ApplicationCertificate = new CertificateIdentifier() },
            TransportConfigurations = new TransportConfigurationCollection(),
            TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
            ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 }
        };
        private static readonly Random rnd = new Random();

        public static async Task Main(string[] args) {
            // Validate Configuration
            await CONFIG.Validate(ApplicationType.Client);

            // Start Session
            using (Session session = await Session.Create(CONFIG, new ConfiguredEndpoint(null, new EndpointDescription(SERVER_URL)), true, "", 6000, null, null)) {
                // Initalise OPC UA Variables
                NodeId NODE_X = new NodeId("ns=2;i=2");
                NodeId NODE_Y = new NodeId("ns=2;i=3");
                NodeId NODE_Z = new NodeId("ns=2;i=4");

                // Program Loop
                while (true) {
                    // Polling Rate
                    await Task.Delay(500);

                    // Print Server Data
                    DataValue X_DATA = await session.ReadValueAsync(NODE_X);
                    DataValue Y_DATA = await session.ReadValueAsync(NODE_Y);
                    DataValue Z_DATA = await session.ReadValueAsync(NODE_Z);
                    Console.WriteLine("X = " + X_DATA.ToString() + " Y = " + Y_DATA.ToString() + " Z = " + Z_DATA.ToString());

                    // Get Positional Data (Randomised)
                    DataValue X = new DataValue(new Variant(rnd.Next(0,100)));
                    DataValue Y = new DataValue(new Variant(rnd.Next(0,100)));
                    DataValue Z = new DataValue(new Variant(rnd.Next(0,100)));

                    // Package Data
                    WriteValueCollection data = new WriteValueCollection();
                    data.Add(new WriteValue() {NodeId = NODE_X, AttributeId = Attributes.Value, Value = X});
                    data.Add(new WriteValue() {NodeId = NODE_Y, AttributeId = Attributes.Value, Value = Y});
                    data.Add(new WriteValue() {NodeId = NODE_Z, AttributeId = Attributes.Value, Value = Z});

                    // Send Data
                    CancellationTokenSource cts = new CancellationTokenSource();
                    CancellationToken ct = cts.Token;
                    await session.WriteAsync(null, data, ct);
                }
            }
        }
    }
}
