import asyncio
from asyncua import Client

async def main():
    # OPC UA Inputs
    URL_OPC_UA = "opc.tcp://localhost:4840"

    async with Client(url = URL_OPC_UA) as client:

        # Initalise OPC UA Variables
        NODE_TEMP = client.get_node("ns=2;i=2")

        # Program Loop
        while True:
            # Polling Rate
            await asyncio.sleep(2)

            # Read Data
            temp_data = await NODE_TEMP.read_value()
            print("THE VALUE OF THE DATA = " + str(temp_data) + " DEGREES")








if __name__ == '__main__':
    asyncio.run(main())





