using System;

namespace UEngine.Net
{
    public class MessageResponseAttribute : Attribute
    {
        public Type ResponseType;
        public MessageResponseAttribute(Type t)
        {
            ResponseType = t;
        }
    }
}