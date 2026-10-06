using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace GeneralClientFramework
{
    // 事件
    public enum NetEvent
    {
        ConnectSucc = 1,
        ConnectFail = 2,
        Cloas = 3
    }


    // 网络模块
    public static class NetManager
    {
        // 定义套接字
        static Socket socket;
        // 定义缓冲区
        static ByteArray readBuff;
        // 写入队列
        static Queue<ByteArray> writeQueue;


        // 是否正在连接
        static bool isConnecting = false;

        // 事件委托类型
        public delegate void EventListener(string err);

        // 事件监听列表
        public static Dictionary<NetEvent, EventListener> eventListeners = new Dictionary<NetEvent, EventListener>();

        // 添加事件监听
        public static void AddEventListener(NetEvent netEvent, EventListener listener)
        {
            // 添加事件
            if (eventListeners.ContainsKey(netEvent))
            {
                eventListeners[netEvent] += listener;
            }
            else // 新增事件
            {
                eventListeners[netEvent] = listener;
            }
        }

        // 删除监听事件
        public static void RemoveEventListener(NetEvent netEvent, EventListener listener)
        {
            if (eventListeners.ContainsKey(netEvent))
            {
                eventListeners[netEvent] -= listener;
                // 删除，否则eventListeners.ContainsKey(netEvent)还是返回true
                if (eventListeners[netEvent] == null)
                {
                    eventListeners.Remove(netEvent);
                }
            }
        }

        // 分发事件
        public static void FireEvent(NetEvent netEvent, string err)
        {
            if (eventListeners.ContainsKey(netEvent))
            {
                eventListeners[netEvent](err);
            }
        }

        // 连接
        public static void Connect(string ip, int port)
        {
            // 判断状态
            if (socket != null && socket.Connected)
            {
                Debug.Log("Connect fail, already connected");
                return;
            }
            if (isConnecting)
            {
                Debug.Log("Connect fail, isConnecting");
                return;
            }
            // 初始化成员
            InitState();
            // 参数设置
            socket.NoDelay = true;     // 不使用Nagle算法，不将小数据量的tcp整合为一个大的再发送
            // connect
            isConnecting = true;
            socket.BeginConnect(ip, port, ConnectCallback, socket);
        }

        // 重置缓冲区，防止客户端再次重连时读取到上一次还未处理的readbuff数据
        private static void InitState()
        {
            // Socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 接收缓冲区
            readBuff = new ByteArray();
            // 写入队列
            writeQueue = new Queue<ByteArray>();
            // 重置连接状态
            isConnecting = false;
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndConnect(ar);
                Debug.Log("Socket Connect success");
                FireEvent(NetEvent.ConnectSucc, "");
                isConnecting = false;
            }
            catch (SocketException ex)
            {
                Debug.Log($"Socket connect fail, {ex.Message}");
                FireEvent(NetEvent.ConnectFail, ex.Message);
                isConnecting = false;
            }

        }
    }
}