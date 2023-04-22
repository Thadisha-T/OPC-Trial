from opcua import Server
from random import randint
import time

SERVER_URL = "opc.tcp://127.0.0.1:4840/UA"
server = Server()
server.set_endpoint(SERVER_URL)

name = "OPC_UA_TEST_SERVER"
address_space = server.register_namespace(name)

node = server.get_objects_node()
param = node.add_object(address_space, "Parameters")

temperature = param.add_variable(address_space, "Temperature", 0)
temperature.set_writable()

server.start()
print("SERVER HAS STARTED...")

while True:
    temperature.set_value(randint(0,50))
    print(temperature.get_value())
    time.sleep(2)
    

