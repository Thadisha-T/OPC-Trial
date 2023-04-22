using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System;

namespace TestClient
{
    class Program
    {
        private static readonly string SERVER_URL = "opc.tcp://127.0.0.1:4840/UA";
        private static readonly ApplicationConfiguration CONFIG = new ApplicationConfiguration() {
            ApplicationName = "TestClient",
            ApplicationUri = $"urn:{System.Net.Dns.GetHostName()}:TestClient",
            ApplicationType = ApplicationType.Client,
            SecurityConfiguration = new SecurityConfiguration {
                ApplicationCertificate = new CertificateIdentifier { StoreType = @"Directory", StorePath = @"${HOME}/.config/OPC Foundation/CertificateStores/MachineDefault", SubjectName = "Test Client" },
                TrustedIssuerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"${HOME}/.config/OPC Foundation/CertificateStores/UA Certificate Authorities" },
                TrustedPeerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"${HOME}/.config/OPC Foundation/CertificateStores/UA Applications" },
                RejectedCertificateStore = new CertificateTrustList { StoreType = @"Directory", StorePath = @"${HOME}/.config/OPC Foundation/CertificateStores/RejectedCertificates" },
                AutoAcceptUntrustedCertificates = true
            },
            TransportConfigurations = new TransportConfigurationCollection(),
            TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
            ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 },
            TraceConfiguration = new TraceConfiguration()
        };

        public static async Task Main(string[] args) {
            // Validate Configuration
            CONFIG.Validate(ApplicationType.Client).GetAwaiter().GetResult();
            
            // Create Application Instance
            ApplicationInstance application = new ApplicationInstance {
                ApplicationName = "Test Client",
                ApplicationType = ApplicationType.Client,
                ApplicationConfiguration = CONFIG
            };
            application.CheckApplicationInstanceCertificate(false, 2048).GetAwaiter().GetResult();

            // Get Endpoint
            EndpointDescription selectedEndpoint = CoreClientUtils.SelectEndpoint(SERVER_URL, useSecurity: true);

            // Start Session
            using (Session session = Session.Create(CONFIG, new ConfiguredEndpoint(null, selectedEndpoint), false, "", 60000, null, null).GetAwaiter().GetResult())
            {
                // Initalise OPC UA Variables
                NodeId NODE_TEMP = new NodeId("ns=2;i=2");

                // Program Loop
                while (true) {
                    // Polling Rate
                    await Task.Delay(2000);

                    // Read Data
                    DataValue TEMP_DATA = await session.ReadValueAsync(NODE_TEMP);
                    Console.WriteLine("THE VALUE OF THE DATA = " + TEMP_DATA.ToString() + " DEGREES");
                }
            }
        }
    }
}
