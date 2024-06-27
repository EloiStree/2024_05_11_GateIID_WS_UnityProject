use std::net::UdpSocket;

fn main() {
    println!("Hello, world!");
    let int_port = 3617;

   loop_push_random_value_to_test(int_port,1.0);
}


fn loop_push_random_value_to_test( int_port: u32, time_between_push: f32) {
    let ms:u64 = (time_between_push * 1000.0)as u64;
    loop {
        let random = rand::random::<u32>();
        push_integer_to_gate_udp(3617, random);
        std::thread::sleep(std::time::Duration::from_millis(ms));
        println!("pushed for test value: {:?}", random);
    }
}

fn push_integer_to_gate_udp(int_port: u32, int_value: u32) {
    let socket = UdpSocket::bind("0.0.0.0:0").expect("Failed to bind socket");

    let address = "127.0.0.1:3617";
    let bytes: [u8; 4] = int_value.to_le_bytes();

    socket
        .send_to(&bytes, address)
        .expect("Failed to send data");
}
