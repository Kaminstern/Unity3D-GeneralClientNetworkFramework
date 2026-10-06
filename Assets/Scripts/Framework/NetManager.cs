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
    }
}