using Opc.Ua;
using Opc.Ua.Client;

public interface IMicroservice {
    Task Run(Session session);
    void Start();
    void Stop();
}

public static class Microservice {
    // Enum that lists all programs
    public enum Type {
        Home,
        Robot1
    }

    // Dictionary to match program against its implementation
    public static readonly Dictionary<Type, IMicroservice> Instances = new Dictionary<Type, IMicroservice> {
        {Type.Home, new Home()},
        {Type.Robot1, new Robot1()},
    };
}

public class Home : IMicroservice {
    private volatile bool stopFlag;

    public async Task Run(Session session) {
        Console.WriteLine("Running Home");

        // Program Loop
        while (!stopFlag) {
            // Polling Rate
            await Task.Delay(2000);
            Console.WriteLine("...");
        }

    }

    public void Start() {
        stopFlag = false;
    }

    public void Stop() {
        stopFlag = true;
    }
}

public class Robot1 : IMicroservice {
    private volatile bool stopFlag;

    public async Task Run(Session session) {
        Console.WriteLine("Running Robot 1");
        Random rnd = new Random();

        // Initalise OPC UA Variables
        NodeId NODE_X = new NodeId("ns=2;i=2");
        NodeId NODE_Y = new NodeId("ns=2;i=3");
        NodeId NODE_Z = new NodeId("ns=2;i=4");

        // Program Loop
        while (!stopFlag) {
            // Polling Rate
            await Task.Delay(500);

            // Print Server Data
            DataValue X_DATA = await session.ReadValueAsync(NODE_X);
            DataValue Y_DATA = await session.ReadValueAsync(NODE_Y);
            DataValue Z_DATA = await session.ReadValueAsync(NODE_Z);
            Console.WriteLine("X = " + X_DATA.ToString() + " Y = " + Y_DATA.ToString() + " Z = " + Z_DATA.ToString());

            // Get Positional Data (Randomised)
            DataValue X = new DataValue(new Variant(rnd.Next(0,9)));
            DataValue Y = new DataValue(new Variant(rnd.Next(0,9)));
            DataValue Z = new DataValue(new Variant(rnd.Next(0,9)));

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

    public void Start() {
        stopFlag = false;
    }

    public void Stop() {
        stopFlag = true;
    }
}

