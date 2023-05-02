import asyncio
from asyncua import Client

async def main():
    # OPC UA Inputs
    SERVER_URL = "opc.tcp://127.0.0.1:4840/UA"

    async with Client(url = SERVER_URL) as client:

        # Initalise OPC UA Variables
        NODE_TEMP = client.get_node("ns=2;i=2")

        # Program Loop
        while True:
            # Polling Rate
            await asyncio.sleep(2)

            # Read Data
            TEMP_DATA = await NODE_TEMP.read_value()
            print("THE VALUE OF THE DATA = " + str(TEMP_DATA) + " DEGREES")

if __name__ == '__main__':
    asyncio.run(main())





