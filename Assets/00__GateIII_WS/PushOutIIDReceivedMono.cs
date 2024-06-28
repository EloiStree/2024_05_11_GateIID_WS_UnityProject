

using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;

public class PushOutIIDReceivedMono : MonoBehaviour
{
    public string m_ipAddress = "127.0.0.1";
    public int m_port = 3616;

    private UdpClient m_udpClient;
    public ulong m_sent;
    IPEndPoint groupEP;
    void Awake()
    {
        m_udpClient = new UdpClient();
        groupEP = new IPEndPoint(IPAddress.Parse(m_ipAddress), m_port);

    }

    public Queue<byte[]> m_messageToSend = new Queue<byte[]>();
    public int m_messageToSendCount = 0;

    public void SendRawByteArray(byte[] whatToSendAsBytes)
    {
        if (this.isActiveAndEnabled)
        {
            m_messageToSend.Enqueue(whatToSendAsBytes);
            m_messageToSendCount = m_messageToSend.Count;
        }
    }
    public void SendInteger(int integer)
    {
        SendRawByteArray(BitConverter.GetBytes(integer));
    }
    [ContextMenu("Send Random Integer")]
    public void SendRandomInteger()
    {
        SendRawByteArray(BitConverter.GetBytes(UnityEngine.Random.Range(int.MinValue, int.MaxValue)));
    }

    public void PushWithoutWaitingUpdate(byte[] rawByte)
    {

        m_udpClient.Send(rawByte, rawByte.Length, groupEP);
    }
    public void PushWithWaitingUpdate(byte[] rawByte)
    {
        m_messageToSend.Enqueue(rawByte);
    }



    public void Update()
    {
        while (m_messageToSend.Count > 0)
        {
            byte[] data = m_messageToSend.Dequeue();
            m_udpClient.Send(data, data.Length, groupEP);
            m_sent++;
        }
        m_messageToSendCount = 0;
    }



    void OnDestroy()
    {
        if (m_udpClient != null)
        {
            m_udpClient.Close();
        }
    }
}
