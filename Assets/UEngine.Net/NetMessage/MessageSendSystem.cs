using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using ProtoBuf;
using UnityEngine;

namespace UEngine.Net
{
    public class MessageSendSystem : ReactiveSystem<NetMessageEntity>
    {
        private byte[] _tempBytes = new byte[4];

        public MessageSendSystem() : base(Contexts.sharedInstance.netMessage)
        {
        }

        protected override ICollector<NetMessageEntity> GetTrigger(IContext<NetMessageEntity> context)
        {
            return context.CreateCollector(NetMessageMatcher.MessageSend);
        }

        protected override bool Filter(NetMessageEntity entity)
        {
            return entity.hasMessageSend;
        }

        protected override void Execute(List<NetMessageEntity> entities)
        {
            foreach (var messageEntity in entities)
            {
                var opCode = messageEntity.messageSend.OpCode;
                var message = messageEntity.messageSend.Message;

                //package = opcode + msg size + message
                MemoryStream sendStream = new MemoryStream();
                
                _tempBytes.WriteTo(0,opCode);
                sendStream.Write(_tempBytes,0,_tempBytes.Length);

                MemoryStream serialStream = new MemoryStream();
                Serializer.Serialize(serialStream,message);
                _tempBytes.WriteTo(0,(int)serialStream.Length);
                
                sendStream.Write(_tempBytes,0,_tempBytes.Length);
                sendStream.Write(serialStream.GetBuffer(),0,(int)serialStream.Length);
                
                Contexts.sharedInstance.network.networkClient.Client.Send(
                    new ArraySegment<byte>(sendStream.GetBuffer()));
            }
        }
    }
}