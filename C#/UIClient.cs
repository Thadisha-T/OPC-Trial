using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;

namespace UIClient
{
    class Program {
        // OPC UA Server Endpoint
        private static readonly string SERVER_URL = "opc.tcp://127.0.0.1:4840/UA";

        // Application Configuration
        private static readonly ApplicationConfiguration CONFIG = new ApplicationConfiguration() {
            ApplicationName = "Test-Client",
            ApplicationType = ApplicationType.Client,
            SecurityConfiguration = new SecurityConfiguration { ApplicationCertificate = new CertificateIdentifier() },
            TransportConfigurations = new TransportConfigurationCollection(),
            TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
            ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 }
        };

        // Current Microprogram
        static IMicroservice current = Microservice.Instances[Microservice.Type.Home];

        // ConcurrentQueue to store user input
        static ConcurrentQueue<Microservice.Type> inputQueue = new ConcurrentQueue<Microservice.Type>();

        // Main
        static async Task Main() {
            // Start Input Thread
            Thread inputThread = new Thread(SwitchMicroservice);
            inputThread.Start();

            // Validate Configuration
            await CONFIG.Validate(ApplicationType.Client);

            // Start Session
            using (Session session = await Session.Create(CONFIG, new ConfiguredEndpoint(null, new EndpointDescription(SERVER_URL)), true, "", 6000, null, null)) {

                while (true) {
                    current.Start();
                    current.Run(session).Wait();
                }
            }
        }

        // User Input
        static void SwitchMicroservice() {
            while (true) {
                // Ask for microservice
                string input = Console.ReadLine();

                // If input is valid, add to queue
                if (Enum.TryParse<Microservice.Type>(input, true, out Microservice.Type parsed)) {
                    inputQueue.Enqueue(parsed);
                }

                if (inputQueue.TryDequeue(out Microservice.Type next)) {
                    // Stop current microservice
                    current.Stop();

                    // Set new microservice
                    current = Microservice.Instances[next];
                }
            }
        }
    }
}
