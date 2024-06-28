import socket
import struct

def start_udp_server(host='localhost', port=3616):
    # Create a socket object
    udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    # Bind the socket to the host and port
    udp_socket.bind((host, port))

    print(f"Listening on {host}:{port}")

    while True:
        # Receive data from the client
        data, addr = udp_socket.recvfrom(1024)
        print(f"Received from {addr}: {data}")
        if len(data) == 16:
            
            int1, int2, ulong = struct.unpack('<iiQ', data)
            print(f'IID: int1={int1}, int2={int2}, ulong={ulong}')
        
        elif len(data) < 50:
            print(f'Received {len(data)} bytes: {data}')
        else:
            print(f'Received {len(data)} bytes')

if __name__ == "__main__":
    start_udp_server()