import asyncio
from asyncua import Client, Node, ua 
import logging
from turtle import delay
import time


logging.basicConfig(level=logging.INFO)
_logger = logging.getLogger('asyncua')


async def main():
    # OPC UA Inputs
    URL_OPC_UA = "opc.tcp://localhost:4840"
    print("TEST")

    async with Client(url = URL_OPC_UA) as client:
        # Initalise OPCUA Variables
        index = await client.get_namespace_index('OPC_UA_TEST_SERVER')
    

if __name__ == '__main__':
    asyncio.run(main())





