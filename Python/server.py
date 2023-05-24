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

x = param.add_variable(address_space, "X", 0)
x.set_writable()

y = param.add_variable(address_space, "Y", 0)
y.set_writable()

z = param.add_variable(address_space, "Z", 0)
z.set_writable()

server.start()
print("SERVER HAS STARTED...")

while True:
    print(x.get_value(), y.get_value(), z.get_value())
    time.sleep(2)
