using System;
using UnityEngine;

namespace GeneralClientFramework
{
    // 所有协议类的基类
    public class MsgBase
    {
        // 协议名
        public string protoName = "";

        // 编码
        public static byte[] Encode(ProtoBuf.IExtensible msg)
        {
            using(var memory = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(memory, msg);
                return memory.ToArray();
            }
        }

        // 解码
        public static ProtoBuf.IExtensible Decode(string protoName, byte[] bytes, int offset, int count)
        {
            using(var memory = new System.IO.MemoryStream(bytes, offset, count))
            {
                System.Type t = System.Type.GetType(protoName);
                return (ProtoBuf.IExtensible)ProtoBuf.Serializer.NonGeneric.Deserialize(t, memory);
            }
        }

        // 编码协议名（2字节长度+字符串）
        public static byte[] EncodeName(ProtoBuf.IExtensible msg)
        {
            // 名字bytes和长度
            byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msg.ToString());
            Int16 len = (Int16)nameBytes.Length;
            // 申请bytes数值
            byte[] bytes = new byte[2+len];
            // 组装2字节的长度信息（小端存储，先存低位，再存高位）书上的示意图标注的是大端存储
            bytes[0] = (byte)(len % 256);
            bytes[1] = (byte)(len / 256);
            // 组装名字bytes
            Array.Copy(nameBytes, 0, bytes, 2, len);
            return bytes;
        }

        // 解析协议名
        public static string DecodeName(byte[] bytes, int offset, out int count)
        {
            count = 0;
            // 必须大于2个字节
            if(offset + 2 > bytes.Length)
            {
                return "";
            }
            // 读取长度
            Int16 len = (Int16)(bytes[offset] |  (bytes[offset + 1] << 8));
            if(len <= 0)
            {
                return "";
            }
            if(offset + 2 + len > bytes.Length)
            {
                return "";
            }
            // 解析
            count = 2 + len;  // 协议名信息的字节数，包括前面计数的2个字节和协议名所占字节
            string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
            return name;
        }
    }
}