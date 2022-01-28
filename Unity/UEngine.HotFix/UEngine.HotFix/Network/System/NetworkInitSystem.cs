using System;
using System.Collections.Generic;
using Entitas;

namespace UEngine.Net.System
{
    public class NetworkInitSystem : IInitializeSystem
    {
        public void Initialize()
        {
            var networkContext = Contexts.sharedInstance.network;
            if (!networkContext.hasMessageRelation)
            {
                networkContext.SetMessageRelation(new Dictionary<int, Type>(), new Dictionary<Type, int>(),
                    new Dictionary<Type, Type>(),new Dictionary<Type, IMessageHandler>());
            }

            var op2Types = networkContext.messageRelation.Op2Types;
            var type2Ops = networkContext.messageRelation.Type2Ops;
            var request2Response = networkContext.messageRelation.Request2Response;
            var request2Handlers = networkContext.messageRelation.Request2Handlers;


            var types = this.GetType().Assembly.GetTypes();
            foreach (var type in types)
            {
                if (typeof(IMessageHandler).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    if (!request2Handlers.ContainsKey(type))
                    {
                        var messageHandler = Activator.CreateInstance(type) as IMessageHandler;
                        request2Handlers.Add(messageHandler.GetRequestType(),messageHandler);
                    }
                }
                
                var attMessageOpCodes = type.GetCustomAttributes(typeof(MessageOpCodeAttribute), false);
                if (attMessageOpCodes.Length == 0)
                {
                    continue;
                }

                var messageOpCodeAttribute = attMessageOpCodes[0] as MessageOpCodeAttribute;
                if (messageOpCodeAttribute == null)
                {
                    continue;
                }

                var opCode = messageOpCodeAttribute.OpCode;
                if (!op2Types.ContainsKey(opCode))
                {
                    op2Types.Add(opCode,type);
                }

                if (!type2Ops.ContainsKey(type))
                {
                    type2Ops.Add(type,opCode);
                }

                if (typeof(IRequest).IsAssignableFrom(type))
                {
                    var attMessageResponses = type.GetCustomAttributes(typeof(MessageResponseAttribute), false);
                    if (attMessageResponses.Length == 0)
                    {
                        continue;
                    }

                    var messageResponseAttribute = attMessageResponses[0] as MessageResponseAttribute;
                    if (!request2Response.ContainsKey(type))
                    {
                        request2Response.Add(type,messageResponseAttribute.ResponseType);
                    }
                }
            }
        }
    }
}