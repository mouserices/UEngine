using System;

namespace UEngine.Net
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageOpCodeAttribute : Attribute
    {
        public int OpCode;
        public MessageOpCodeAttribute(int opCode)
        {
            OpCode = opCode;
        }
    }
}