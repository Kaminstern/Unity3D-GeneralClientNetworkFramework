using UnityEngine;

namespace GeneralClientFramework
{
    public class Test : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            NetManager.AddEventListener(NetEvent.ConnectSucc, OnConnectSucc);
            NetManager.AddEventListener(NetEvent.ConnectFail, OnConnectFail);
            NetManager.AddEventListener(NetEvent.Cloas, OnConnectClose);

        }

        // Update is called once per frame
        void Update()
        {

        }

        // 玩家点击连接按钮
        public void OnConnectClick()
        {
            NetManager.Connect("127.0.0.1", 8888);
            // TODO:开始转圈，提示“连接中”
        }

        // 连接成功回调
        void OnConnectSucc(string err)
        {
            Debug.Log("OnConnectSucc");
        }

        // 连接失败回调
        void OnConnectFail(string err)
        {
            Debug.Log("OnConnectFail");
        }

        // 关闭连接
        void OnConnectClose(string err)
        {
            Debug.Log("OnConnectClose");
        }
    }
}