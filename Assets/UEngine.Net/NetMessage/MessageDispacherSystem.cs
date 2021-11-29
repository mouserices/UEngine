using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using ProtoBuf;
using UnityEngine;

namespace UEngine.Net
{
    public class MessageDispacherSystem : ReactiveSystem<NetMessageEntity>
    {
        private MemoryStream _dispacherStream;
        private byte[] _opCodeBytes;
        private byte[] _messageSizeBytes;

        public MessageDispacherSystem() : base(Contexts.sharedInstance.netMessage)
        {
            _dispacherStream = new MemoryStream();
            _opCodeBytes = new byte[4];
            _messageSizeBytes = new byte[4];
        }

        protected override ICollector<NetMessageEntity> GetTrigger(IContext<NetMessageEntity> context)
        {
            return context.CreateCollector(NetMessageMatcher.MessageDispacher);
        }

        protected override bool Filter(NetMessageEntity entity)
        {
            return entity.hasMessageDispacher;
        }

        protected override void Execute(List<NetMessageEntity> entities)
        {
            foreach (var messageEntity in entities)
            {
                _dispacherStream.Seek(0, SeekOrigin.Begin);
                var dispacherBytes = messageEntity.messageDispacher.Bytes;
                _dispacherStream.Write(dispacherBytes, 0, dispacherBytes.Length);
                _dispacherStream.Seek(0, SeekOrigin.Begin);
                _dispacherStream.Read(_opCodeBytes, 0, 4);
                _dispacherStream.Read(_messageSizeBytes, 0, 4);

                var opCode = BitConverter.ToInt32(_opCodeBytes, 0);
                var messageSizeBytes = BitConverter.ToInt32(_messageSizeBytes, 0);

                byte[] readyToDeserialize = new byte[messageSizeBytes];
                _dispacherStream.Read(readyToDeserialize, 0, messageSizeBytes);

                var c2SLogin = Serializer.Deserialize<C2S_Login>(new MemoryStream(readyToDeserialize));
                Debug.Log($"{c2SLogin.UserName} {c2SLogin.Password}");
            }
        }
    }
}