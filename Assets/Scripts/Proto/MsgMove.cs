using UnityEngine;

namespace GeneralClientFramework
{
    public class MsgMove : MsgBase
    {
        public MsgMove()
        {
            protoName = "MsgMove";
        }

        public float x = 0;
        public float y = 0;
        public float z = 0;
    }
}
