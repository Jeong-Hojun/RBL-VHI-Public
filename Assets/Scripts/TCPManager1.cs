using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Receives GSR (galvanic skin response) data from a TCP server and saves it to CSV on quit.
/// Set savePath in the Inspector to the target directory before running.
/// K key state is logged alongside GSR data to mark threat stimulus onset.
/// </summary>
public class TCPManager1 : MonoBehaviour
{
    [Header("TCP Settings")]
    int port = 6175;

    [Header("Data Save")]
    [Tooltip("Directory path where the CSV will be saved (must already exist).")]
    public string savePath = @"D:\RESEARCH\EEG_NFTsystem\Data\GSR_AOMIvsVHI\";

    TcpClient client;
    string serverIP;
    byte[] receivedBuffer;
    byte[] timeBuffer;
    bool socketReady = false;
    NetworkStream stream;

    public double timedata;
    public double gsrdata;

    Thread myreader;
    List<string> csvList = new List<string>();
    private int isK;
    GameObject IPIF;

    void Start()
    {
        isK = 0;

        // Validate save path immediately so the researcher knows before data is collected
        if (!Directory.Exists(savePath))
        {
            Debug.LogError($"[TCPManager1] Save path does not exist: \"{savePath}\"\n" +
                           "Please set a valid directory in the Inspector before running. Data will NOT be saved.");
        }

        IPIF = GameObject.FindGameObjectWithTag("IPAddress");
        if (IPIF == null)
            Debug.LogError("[TCPManager1] Could not find UI object tagged 'IPAddress'.");
    }

    void Update()
    {
        // K key toggles threat stimulus marker (logged in CSV)
        if (Input.GetKeyDown(KeyCode.K))
            isK = (isK == 0) ? 1 : 0;
    }

    /// <summary>
    /// Background thread: continuously reads 16-byte packets (8B timestamp + 8B GSR) from TCP stream.
    /// </summary>
    void mainthread()
    {
        if (!socketReady) return;

        while (socketReady && stream.DataAvailable)
        {
            receivedBuffer = new byte[16];
            stream.Read(receivedBuffer, 0, receivedBuffer.Length);

            timeBuffer = receivedBuffer.Take(8).ToArray();
            byte[] gsrBuffer = receivedBuffer.Skip(8).Take(8).ToArray();

            double newTime = BitConverter.ToDouble(timeBuffer, 0);
            if (timedata != newTime)
            {
                timedata = newTime;
                gsrdata = BitConverter.ToDouble(gsrBuffer, 0);
                csvList.Add($"{timedata},{gsrdata},{isK}");
            }
        }
    }

    void CheckReceive()
    {
        if (socketReady) return;
        try
        {
            client = new TcpClient(serverIP, port);
            if (client.Connected)
            {
                stream = client.GetStream();
                socketReady = true;
                Debug.Log("[TCPManager1] TCP connection successful.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("[TCPManager1] TCP connection failed: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        // Stop receiving thread
        socketReady = false;
        myreader?.Join(500);

        // Validate save path before writing
        if (!Directory.Exists(savePath))
        {
            Debug.LogError($"[TCPManager1] Cannot save CSV — directory does not exist: \"{savePath}\"\n" +
                           $"Collected {csvList.Count} rows of data were lost.");
            CloseSocket();
            return;
        }

        string fileName = Path.Combine(savePath, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv");
        try
        {
            using (StreamWriter wr = new StreamWriter(fileName))
            {
                wr.WriteLine("timedata,gsrdata,isK");
                foreach (string row in csvList)
                    wr.WriteLine(row);
            }
            Debug.Log($"[TCPManager1] GSR data saved: {fileName} ({csvList.Count} rows)");
        }
        catch (Exception e)
        {
            Debug.LogError($"[TCPManager1] Failed to write CSV: {e.Message}");
        }

        CloseSocket();
    }

    void CloseSocket()
    {
        stream?.Close();
        client?.Close();
        socketReady = false;
    }

    /// <summary>
    /// Called by the START button in the UI. Reads IP from the input field and begins TCP connection.
    /// </summary>
    public void OnClickStartButton()
    {
        serverIP = IPIF.GetComponent<Text>().text;
        Debug.Log("[TCPManager1] Connecting to " + serverIP + ":" + port);
        CheckReceive();
        myreader = new Thread(mainthread);
        myreader.Start();
    }
}
