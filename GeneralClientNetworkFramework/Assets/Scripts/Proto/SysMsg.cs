using UnityEngine;


namespace GeneralClientFramework
{
    public class MsgPing: MsgBase
    {
        public MsgPing()
        {
            protoName = "MsgPing";
        }
    }

    public class MsgPong: MsgBase
    {
        public MsgPong()
        {
            protoName = "MsgPong";
        }
    }
}
