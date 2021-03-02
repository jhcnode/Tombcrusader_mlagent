using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;


namespace DeepGes { 
public class GNetwork : MonoBehaviour {

        public bool CollectGestureData;
        //서버 IP주소 및 포트번호
        public string serverIP = "127.0.0.1";
        public string port = "12345";    
    
        //Thread - Lock(Critical Section) 관련 객체
        private object lockObject = new object();
        private volatile static bool ConnectFlag = false;
        private static Socket client;
        [HideInInspector] public Thread thread;
        //Volatile 변수
        [HideInInspector] public volatile string flag = "none";
        [HideInInspector] public volatile List<Vector3> raw_data = null;
        [HideInInspector] public volatile byte[] _data = null;
        public volatile bool popFlag = false;
        public volatile string result = null;
        //데이터 크기 
        [HideInInspector] public int gestureSize = GConfig.MaxPoint;
        //VR
        //private GameObject left, right;



        ///실험 변수 
        public float start_time;
        public float end_time;
        public List<float> timeList;


        public void SaveData(int label)
        {
            string path = Application.dataPath + "/CollectExp/"+label.ToString()+"/";

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            DateTime date = System.DateTime.Now;
            string name = path + date.ToString("yyyy_MM_dd_hh_mm_ss.fffffff_tt") + ".csv";
            StreamWriter w = File.CreateText(name);
            w.WriteLine("step" + "," + "time");
            for (int i = 0; i < timeList.Count; ++i)
            {
                w.WriteLine((i+1).ToString() + "," + timeList[i].ToString());
            }
            Debug.Log(name);
            w.Close();
            timeList.Clear();
        }


        //프로그램 시작 시
        void Awake()
        {
            //Unity Engine - Thread 설정
            Application.runInBackground = true;
            Application.backgroundLoadingPriority = UnityEngine.ThreadPriority.Normal;
        }

        //스크립트 시작 시(OnEnable)
        void Start ()
        {

            //게임 시작(스크립트 시작) 시 네트워크 연결 시작
            StartNetwork();
        }
	
	    //게임 업데이트
	    void Update ()
        {
        }

        //게임 종료 시
        void OnDestroy()
        {
            //쓰레드 종료 처리
            CloseNetwork();
        }

        //게임 내부 - Send 요청 함수
        public void SendGesture(ref List<Vector3> point,GConfig.Hand hand)
        {
            lock (lockObject)
            {
                popFlag = false;

                if (CollectGestureData == true)
                {
                    raw_data.Clear();
                    for (int i = 0; i < point.Count; ++i)
                        raw_data.Add(point[i]);

                    _data = GUtil.ExportGesture(ref raw_data, hand);
                }
                else
                    _data = GUtil.ExportGesture(ref point, hand);




                flag = "send";
            }
        }

        //게임 내부 - Result 요청 함수
        public int GetResult()
        {
            int tmp;
            lock(lockObject)
            {
                if (popFlag == true)
                {
                    tmp = int.Parse(result);
                    popFlag = false;
                    if (CollectGestureData == true) GUtil.SaveData(ref raw_data, tmp);
                }
                else tmp = -1;
            }
            return tmp;
        }

        //네트워크 시작 함수(Thread Start)
        public void StartNetwork()
        {
            //Thread 시작
            thread = new Thread(NetThread);
            thread.Start();
        }

        //네트워크 종료 함수(Thread Close)
        public void CloseNetwork()
        {
            lock(lockObject)
            {
                flag = "exit";
                ConnectFlag = false;
            }
        }

        //네트워크 쓰레드
        private void NetThread()
        {
            //Client Socket 생성
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IP Address 파싱 및 연결(Connect)
            IPAddress localIPAddress = IPAddress.Parse(serverIP);
            IPEndPoint localEndPoint = new IPEndPoint(localIPAddress, int.Parse(port));
            try
            {
                client.Connect(localEndPoint);
                client.NoDelay = true;
            }
            catch (Exception err) { }
            //Flag 초기화
            flag = "none";
            ConnectFlag = true;
            //Network Thread - Loop 시작
            while (client != null)
            {
                try
                {
                    string msg = "";
                    lock (lockObject)
                    {
                        msg = flag;
                    }
                    //Message - send 요청
                    if (msg == "send")
                    {
                        //"send" 플래그 보내기
                        Send(client, msg);
                        lock (lockObject)
                        {
                            //_data(입력 데이터) 보내기 
                            Send(client, _data);
                        }
                   
                        //서버에서 send 요청 완료 받기
                        byte[] complete_msg = null;
                        Recieve(client, ref complete_msg);
                        string complete = Encoding.Default.GetString(complete_msg);
                        //receive 상태로 전환
                        lock (lockObject)
                        {
                            if (complete == "complete_send")
                            {
                                flag = "receive";
                                msg = flag;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    //Message - receive 요청
                    else if (msg == "receive")
                    {
                        //"receive" 플래그 보내기 
                        Send(client, msg);
                        //보낸 데이터의 결과 받기
                        byte[] data = null;
                        Recieve(client, ref data);
                        lock (lockObject)
                        {
                            result = Encoding.Default.GetString(data);
                        }
                        lock (lockObject)
                        {

                            popFlag = true;
                            flag = "none";
                            msg = flag;
                        }

                    }
                    //Message - exit 요청
                    else if (msg == "exit")
                    {
                        Send(client, msg);
                        break;
                    }
                }
                catch (Exception err) { }
                if (ConnectFlag == false) break;
                Thread.Sleep(1);
            }
            //Loop 종료 시 Socket Close, Thread 종료 처리
            if (client != null) client.Close();
            client = null;
        }

        //Send 함수(byte)
        public bool Send(Socket sock, byte[] msg)
        {
            byte[] data = msg;
            byte[] data_size = new byte[4];
            data_size = BitConverter.GetBytes(data.Length);
            sock.Send(data_size);
            sock.Send(data, 0, data.Length, SocketFlags.None);
            return true;
        }

        //Send 함수(String)
        public bool Send(Socket sock, String msg)
        {
            byte[] data = Encoding.Default.GetBytes(msg);
            byte[] data_size = new byte[4];
            data_size = BitConverter.GetBytes(data.Length);
            sock.Send(data_size);
            sock.Send(data, 0, data.Length, SocketFlags.None);
            return true;
        }

        //Recieve 함수
        public bool Recieve(Socket sock, ref byte[] msg)
        {
            int totalBytesRecvd = 0;
            byte[] data_size = new byte[4];
            totalBytesRecvd = sock.Receive(data_size, 0, 4, SocketFlags.None);
            Recieve_Check(sock, ref data_size, totalBytesRecvd);

            int size = BitConverter.ToInt32(data_size, 0);
            msg = new byte[size];
            totalBytesRecvd = sock.Receive(msg, 0, size, SocketFlags.None);
            Recieve_Check(sock, ref msg, totalBytesRecvd);
            return true;
        }

        //Recieve_Check 함수(패킷 크기체크, Buffer Stacked)
        private void Recieve_Check(Socket sock, ref byte[] msg, int totalBytesRecvd)
        {
            while (totalBytesRecvd < msg.Length)
            {
                int rest = msg.Length - totalBytesRecvd;
                byte[] buff = new byte[rest];
                int byteBytesRecvd = sock.Receive(buff, 0, rest, SocketFlags.None);
                for (int i = 0; i < byteBytesRecvd; ++i)
                {
                    msg[totalBytesRecvd + i] = buff[i];
                }
                totalBytesRecvd += byteBytesRecvd;
            }
        }


    }
}