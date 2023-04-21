from opcua import Server
from random import randint
import time


server = Server()
server.set_endpoint("opc.tcp://localhost:4840")

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
    

