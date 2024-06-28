use std::net::UdpSocket;


struct GateData {
    address: String,
    port: u16,
    addressAndPort: String,
    socket: UdpSocket,
}


impl GateData {
    fn new(address: String, port: u16) -> Result<Self, std::io::Error> {
        let socket = UdpSocket::bind("0.0.0.0:0").expect("Failed to bind socket");
        let concat = format!("{}:{}", address.clone(), port);
      
        Ok(GateData {
            address: address,
            port: port,
            addressAndPort: concat,
            socket: socket,
        })
    }
}

trait GatePusher {
    fn push_integer_to_gate_udp(&self, int_value: i32);
}

impl GatePusher for GateData {
    fn push_integer_to_gate_udp(&self, int_value: i32) {
        let bytes: [u8; 4] = int_value.to_le_bytes();
        self.socket
            .send_to(&bytes, self.addressAndPort.clone())
            .expect("Failed to send data");
    }
}


fn main() {
    // Replace with your port
    let port= 3617;
    // Replace with your IP if not on the same computer
    let address: String = "127.0.0.1".to_string();
    
    let gate = GateData::new(address, port).expect("Failed to create gate");
    println!("Hello, world!");



   loop_push_random_value_to_test(gate,1.0);
}


fn loop_push_random_value_to_test( socket: GateData, time_between_push: f32) {
    let ms:u64 = (time_between_push * 1000.0)as u64;
    loop {
        let random = rand::random::<u32>();
        
        socket.push_integer_to_gate_udp(random as i32);
        println!("pushed for test value: {:?}", random);
        std::thread::sleep(std::time::Duration::from_millis(ms));
    }
}