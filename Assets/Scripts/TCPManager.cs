using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

/// <summary>
/// Connects to a TCP server (e.g., OpenViBE) and receives a stream of double-precision
/// floating-point values representing the biofeedback score.
/// The latest value is stored in <see cref="test"/> and read by ScoreManager.
/// </summary>
public class TCPManager : MonoBehaviour
{
    TcpClient client;
    string serverIP = "192.168.2.124";
    int port = 5678;

    byte[] receivedBuffer;
    NetworkStream stream;
    bool socketReady = false;

    public double test; // Latest score received from the TCP server

    void Start()
    {
        ConnectToServer();
    }

    void Update()
    {
        if (!socketReady) return;

        if (stream.DataAvailable)
        {
            // Read one double (8 bytes) from the stream
            receivedBuffer = new byte[8];
            stream.Read(receivedBuffer, 0, receivedBuffer.Length);
            test = BitConverter.ToDouble(receivedBuffer, 0);
            Debug.Log(test);
        }
    }

    void ConnectToServer()
    {
        if (socketReady) return;
        try
        {
            client = new TcpClient(serverIP, port);
            if (client.Connected)
            {
                stream = client.GetStream();
                socketReady = true;
                Debug.Log("TCP connection successful.");
            }
        }
        catch (Exception e)
        {
            Debug.Log("TCP connection failed: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        CloseSocket();
    }

    void CloseSocket()
    {
        if (!socketReady) return;
        stream?.Close();
        client?.Close();
        socketReady = false;
    }
}
