using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using UnityEngine.UI;
using Ardunity;
using System.Threading;

// http://answers.unity3d.com/questions/15422/unity-project-and-3rd-party-apps.html
public class s_TCP_thread : MonoBehaviour
{
    internal Boolean socketReady = false;
    TcpClient mySocket;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;
    public String Host = "192.168.2.124";
    public Int32 Port = 5678;
    public int samplingFreq = 500;

    public bool testHeader = true;
    public bool testSignal = false;
    public bool isString = false;
    public int testSampleChannelSize;
    public int testSampleCount;
    public int testChannelCount;

    public double[,] lastMatrix;
    public double[,] signalMat;

    public float power;

    public RawOpenVibeSignal lastSignal;

    private GameObject gameObjectforisA;
    private CurveOutput1 curveOutput;
    private bool isA_temp;
    public bool isA_bool;
    private int isA_count;
    public int isA_count_for_bool;

    List<List<double>> listWaitTrigger = new List<List<double>>();
    private double ref0;
    private double ref1;
    private double ref2;
    private double ref3;
    private double ref4;
    private double ref5;

    private double[] rp_temp;
    public double[] rp;

    private Thread thread_comm;
    private Thread thread_score;
    private Queue<double[]> queue = new Queue<double[]>();
    private Queue<double[,]> queuematrix = new Queue<double[,]>();

    Rigidbody2D rb;
    public class RawOpenVibeSignal
    {

        public int channels;
        public int samples;

        public double[,] signalMatrix;
    }
    void Start()
    {
        thread_comm = new Thread(Run);
        thread_comm.Start();

        thread_score = new Thread(scoreCalculation);
        thread_score.Start();

        gameObjectforisA = GameObject.Find("AnalogOutput");
        curveOutput = gameObjectforisA.GetComponent<CurveOutput1>();
        isA_bool = curveOutput.isA;

        setupSocket();

    }


    void FixedUpdate()
    {
        //isA_bool = fingerRotation_cs.isA;     

        //power = ((float)readSocket());
        //scoreCalculation();
        rp = rp_temp;
        //rp = queue.Dequeue();

    }
    // **********************************************
    public void setupSocket()
    {
        try
        {
            mySocket = new TcpClient(Host, Port);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);
            theReader = new StreamReader(theStream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
        }
    }
    public void writeSocket(string theLine)
    {
        if (!socketReady)
            return;
        String foo = theLine + "\r\n";
        theWriter.Write(foo);
        theWriter.Flush();
    }

    private void Run()
    {
        while (true)
        {
                // raw signal data
                // [nSamples x nChannels]
                // all channels for one sample are sent in a sequence, then all channels of the next sample

                // create a signal object to send it to another
            RawOpenVibeSignal newSignal = new RawOpenVibeSignal();
            newSignal.samples = testSampleCount;
            newSignal.channels = testChannelCount;

            double[,] newMatrix = new double[testSampleCount, testChannelCount];

            byte[] buffer = new byte[testSampleChannelSize];
            theStream.Read(buffer, 0, testSampleChannelSize);

            int row = 0;
            int col = 0;
            for (int i = 0; i < testSampleCount * testChannelCount * (sizeof(double)); i = i + (sizeof(double) * testChannelCount))
            {
                for (int j = 0; j < testChannelCount * sizeof(double); j = j + sizeof(double))
                {

                    byte[] temp = new byte[8];

                    for (int k = 0; k < 8; k++)
                    {
                        temp[k] = buffer[i + j + k];
                    }

                    if (BitConverter.IsLittleEndian)
                    {
                        // Array.Reverse(temp);
                        double test = BitConverter.ToDouble(temp, 0);
                        
                        // TODO TEST THIS
                        //newMatrix[i / (8 * testChannelCount), j / 8] = test;
                        newMatrix[row, col] = test;
                    }
                    col++;

                }
                row++;
                col = 0;
            }
            queuematrix.Enqueue(newMatrix);
            Debug.Log(newMatrix[0, 0]);
            newSignal.signalMatrix = newMatrix;
            lastSignal = newSignal;
            lastMatrix = newMatrix;
            
        }
    }

    //public double readSocket()
    //{
    //    if (!socketReady)
    //        return 0; // TODO
    //    if (theStream.DataAvailable)
    //    {



    //        // read header once
    //        if (testHeader)
    //        {
    //            readHeader();

    //        }

    //        if (testSignal)
    //        {
    //            // raw signal data
    //            // [nSamples x nChannels]
    //            // all channels for one sample are sent in a sequence, then all channels of the next sample

    //            // create a signal object to send it to another
    //            RawOpenVibeSignal newSignal = new RawOpenVibeSignal();
    //            newSignal.samples = testSampleCount;
    //            newSignal.channels = testChannelCount;

    //            double[,] newMatrix = new double[testSampleCount, testChannelCount];


    //            byte[] buffer = new byte[testSampleChannelSize];

    //            theStream.Read(buffer, 0, testSampleChannelSize);


    //            int row = 0;
    //            int col = 0;
    //            for (int i = 0; i < testSampleCount * testChannelCount * (sizeof(double)); i = i + (sizeof(double) * testChannelCount))
    //            {
    //                for (int j = 0; j < testChannelCount * sizeof(double); j = j + sizeof(double))
    //                {

    //                    byte[] temp = new byte[8];

    //                    for (int k = 0; k < 8; k++)
    //                    {
    //                        temp[k] = buffer[i + j + k];
    //                    }

    //                    if (BitConverter.IsLittleEndian)
    //                    {
    //                        // Array.Reverse(temp);
    //                        double test = BitConverter.ToDouble(temp, 0);

    //                        // TODO TEST THIS
    //                        //newMatrix[i / (8 * testChannelCount), j / 8] = test;
    //                        newMatrix[row, col] = test;
    //                    }
    //                    col++;

    //                }
    //                row++;
    //                col = 0;
    //            }

    //            newSignal.signalMatrix = newMatrix;
    //            lastSignal = newSignal;
    //            lastMatrix = newMatrix;


                
    //            //displaySignalText();

    //            return newMatrix[0, 0];
    //        }
    //        else if (isString)
    //        {
    //            Debug.Log(theReader.ReadLine());
    //        }

    //    }
    //    return 0;

    //}

    private void readHeader()
    {
        // size of header is 8 * size of unit = 32 byte

        int variableSize = sizeof(UInt32);
        int variableCount = 8;

        int headerSize = variableCount * variableSize;

        byte[] buffer = new byte[headerSize];

        theStream.Read(buffer, 0, headerSize);

        // version number (in network byte order)
        // endianness of the stream (in network byte order, 0==unknown, 1==little, 2==big, 3==pdp)
        // sampling frequency of the signal, 
        //  number of channels, 
        // number of samples per chunk and 
        // three variables of padding


        UInt32 version, endiannes, frequency, channels, samples;

        byte[] v = new byte[4] { buffer[0], buffer[1], buffer[2], buffer[3] };
        byte[] e = new byte[4] { buffer[4], buffer[5], buffer[6], buffer[7] };
        byte[] f = new byte[4] { buffer[8], buffer[9], buffer[10], buffer[11] };
        byte[] c = new byte[4] { buffer[12], buffer[13], buffer[14], buffer[15] };
        byte[] s = new byte[4] { buffer[16], buffer[17], buffer[18], buffer[19] };
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(e);
            Array.Reverse(v);
            version = BitConverter.ToUInt32(v, 0);
            endiannes = BitConverter.ToUInt32(e, 0);
            frequency = BitConverter.ToUInt32(f, 0);
            channels = BitConverter.ToUInt32(c, 0);
            samples = BitConverter.ToUInt32(s, 0);
        }
        else
        {

            version = 999;
            endiannes = 0;
            frequency = 0;
            channels = 0;
            samples = 0;
        }

        Debug.Log("Version: " + version + "\n" + "Endiannes: " + endiannes + "\n" + "sampling frequency of the signal: " + frequency + "\n" + "number of channels: " + channels + "\n" + "number of samples per chunk: " + samples + "\n");

        testHeader = false;
        testSampleCount = buffer[16];
        testChannelCount = buffer[12];
        testSampleChannelSize = buffer[12] * buffer[16] * sizeof(double);

        testSignal = true;

        rb = GetComponent<Rigidbody2D>();
    }

    public void closeSocket()
    {
        if (!socketReady)
            return;
        theWriter.Close();
        theReader.Close();
        mySocket.Close();
        socketReady = false;
    }


    private void scoreCalculation()
    {
        signalMat = queuematrix.Dequeue();

        List<double> listtemp0 = new List<double>();
        List<double> listtemp1 = new List<double>();
        List<double> listtemp2 = new List<double>();
        List<double> listtemp3 = new List<double>();
        List<double> listtemp4 = new List<double>();
        List<double> listtemp5 = new List<double>();

        listWaitTrigger.Add(listtemp0);
        listWaitTrigger.Add(listtemp1);
        listWaitTrigger.Add(listtemp2);
        listWaitTrigger.Add(listtemp3);
        listWaitTrigger.Add(listtemp4);
        listWaitTrigger.Add(listtemp5);

        //Debug.Log(lastSignal.signalMatrix[0, 0] == lastSignal.signalMatrix[0, 5]);
        if (listWaitTrigger[0].Count <= samplingFreq * 4)
        {

            listWaitTrigger[0].Add(signalMat[0, 0]);
            listWaitTrigger[1].Add(signalMat[0, 1]);
            listWaitTrigger[2].Add(signalMat[0, 2]);
            listWaitTrigger[3].Add(signalMat[0, 3]);
            listWaitTrigger[4].Add(signalMat[0, 4]);
            listWaitTrigger[5].Add(signalMat[0, 5]);
        }
        else if (listWaitTrigger[0].Count > samplingFreq * 4)
        {
            listWaitTrigger[0].RemoveAt(0);
            listWaitTrigger[1].RemoveAt(0);
            listWaitTrigger[2].RemoveAt(0);
            listWaitTrigger[3].RemoveAt(0);
            listWaitTrigger[4].RemoveAt(0);
            listWaitTrigger[5].RemoveAt(0);

            listWaitTrigger[0].Add(signalMat[0, 0]);
            //Debug.Log(listWaitTrigger[0][0]);
            listWaitTrigger[1].Add(signalMat[0, 1]);
            listWaitTrigger[2].Add(signalMat[0, 2]);
            listWaitTrigger[3].Add(signalMat[0, 3]);
            listWaitTrigger[4].Add(signalMat[0, 4]);
            listWaitTrigger[5].Add(signalMat[0, 5]);
        }
        else
        {
            Debug.LogError("There is an error in listWaitTrigger");
        }

        Debug.Log(listWaitTrigger[0].Count);
        //Debug.Log(listWaitTrigger[0][0] == listWaitTrigger[5][0]);

        //
        //foreach (var item in myList)
        //{
        //    Debug.Log(item.ToString());
        //}
        //

        isA_temp = (curveOutput._curveValue > 0);

        if (isA_temp)
        {
            isA_count += 1;
        }
        else
        {
            isA_count = 0;
        }

        if (isA_count == 1)
        {
            isA_bool = true;
        }
        else
        {
            isA_bool = false;
        }


        //Debug.Log(listWaitTrigger[0][samplingFreq*4-1]);
        if (isA_bool)
        {
            isA_count_for_bool += 1;
            ref0 = listWaitTrigger[0].GetRange(0, samplingFreq * 2).Average();
            ref1 = listWaitTrigger[1].GetRange(0, samplingFreq * 2).Average();
            ref2 = listWaitTrigger[2].GetRange(0, samplingFreq * 2).Average();
            ref3 = listWaitTrigger[3].GetRange(0, samplingFreq * 2).Average();
            ref4 = listWaitTrigger[4].GetRange(0, samplingFreq * 2).Average();
            ref5 = listWaitTrigger[4].GetRange(0, samplingFreq * 2).Average();

        }

        //Debug.Log(listWaitTrigger[0].Count);
        //Debug.Log(listWaitTrigger[5].Count);
        //Debug.Log(listWaitTrigger[6].Count);
        var sqAvg0 = listWaitTrigger[0].GetRange(samplingFreq * 2 - 1, samplingFreq * 2).Average(x => x * x);
        var sqAvg1 = listWaitTrigger[1].GetRange(samplingFreq * 2 - 1, samplingFreq * 2).Average(x => x * x);
        var sqAvg2 = listWaitTrigger[2].GetRange(samplingFreq * 2 - 1, samplingFreq * 2).Average(x => x * x);
        var sqAvg3 = listWaitTrigger[3].GetRange(samplingFreq * 2 - 1, samplingFreq * 2).Average(x => x * x);
        var sqAvg4 = listWaitTrigger[4].GetRange(samplingFreq * 2 - 1, samplingFreq * 2).Average(x => x * x);
        var sqAvg5 = listWaitTrigger[5].GetRange(samplingFreq * 2 - 1, samplingFreq * 2).Average(x => x * x);
        
        //average filter is needeed.

        //Debug.Log(sqAvg0);
        //Debug.Log(ref0);
        rp_temp[0] = sqAvg0 / ref0;
        rp_temp[1] = sqAvg1 / ref1;
        rp_temp[2] = sqAvg2 / ref2;
        rp_temp[3] = sqAvg3 / ref3;
        rp_temp[4] = sqAvg4 / ref4;
        rp_temp[5] = sqAvg5 / ref5;

        queue.Enqueue(rp_temp);
    }


    void displaySignalText()
    {
        RawOpenVibeSignal s = lastSignal;

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < lastSignal.samples; i++)
        {
            sb.AppendLine("Sample" + i + "\t");
            for (int j = 0; j < lastSignal.channels; j++)
            {
                try
                {
                    sb.AppendLine("Channel" + j).Append(lastSignal.signalMatrix[i, j]);
                }
                catch
                {
                    Debug.Log("i:" + i + "-j:" + j);
                }
            }
        }

        //Debug.Log(sb.ToString());

    }
    void OnApplicationQuit()
    {
        closeSocket();
    }
} // end class s_TCP