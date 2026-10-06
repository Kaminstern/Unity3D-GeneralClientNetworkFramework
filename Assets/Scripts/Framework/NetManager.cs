using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace GeneralClientFramework
{
    // 网络模块
    public static class NetManager
    {
        // 定义套接字
        static Socket socket;
        // 定义缓冲区
        static ByteArray readBuff;
        // 写入队列
        static Queue<ByteArray> writeQueue;
    }
}