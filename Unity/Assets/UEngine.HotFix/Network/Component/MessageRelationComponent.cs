using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [Network,Unique]
    public class MessageRelationComponent : IComponent
    {
        public Dictionary<int, Type> Op2Types;
        public Dictionary<Type, int> Type2Ops;
        public Dictionary<Type, Type> Request2Response;
        public Dictionary<Type, IMessageHandler> Request2Handlers;
    }
}