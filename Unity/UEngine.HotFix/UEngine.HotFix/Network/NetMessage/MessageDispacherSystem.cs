using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using ProtoBuf;

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
                var connectionId = messageEntity.messageDispacher.ConnectionId;
                _dispacherStream.Write(dispacherBytes, 0, dispacherBytes.Length);
                _dispacherStream.Seek(0, SeekOrigin.Begin);
                _dispacherStream.Read(_opCodeBytes, 0, 4);
                _dispacherStream.Read(_messageSizeBytes, 0, 4);

                var opCode = BitConverter.ToInt32(_opCodeBytes, 0);
                var messageSizeBytes = BitConverter.ToInt32(_messageSizeBytes, 0);

                byte[] readyToDeserialize = new byte[messageSizeBytes];
                _dispacherStream.Read(readyToDeserialize, 0, messageSizeBytes);
                messageEntity.Destroy();
                
                var networkContext = Contexts.sharedInstance.network;
                if (networkContext.messageRelation.Op2Types.TryGetValue(opCode, out var messageType))
                {
                    var message =
                        Serializer.Deserialize(messageType, new MemoryStream(readyToDeserialize));
                    MetaContext.Get<ILogService>().Log($"Message Recevied: {opCode} type: {message.ToString()}");
                    switch (message)
                    {
                        case ISimpleMessage iSimpleMessage:
                            if (networkContext.messageRelation.Request2Handlers.TryGetValue(iSimpleMessage.GetType(),out IMessageHandler handler))
                            {
                                handler.Handle(connectionId,iSimpleMessage);
                            }
                            else
                            {
                                MetaContext.Get<ILogService>().LogError($"can not find MessageHandler,you need register it opCode: {opCode}");
                            }

                            break;
                        case IRequest iRequest:

                            if (networkContext.messageRelation.Request2Handlers.TryGetValue(iRequest.GetType(),out IMessageHandler messageHandler))
                            {
                                messageHandler.Handle(connectionId,iRequest);
                            }
                            else
                            {
                                MetaContext.Get<ILogService>().LogError($"can not find MessageHandler,you need register it opCode: {opCode}");
                            }

                            break;
                        case IResponse iReponse:
                            if (networkContext.networkRequest.Requests.TryGetValue(iReponse.RpcID,
                                out NetworkRequest request))
                            {
                                networkContext.networkRequest.Requests.Remove(iReponse.RpcID);
                                request.SetResult(iReponse);
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    MetaContext.Get<ILogService>().LogError($"can not find IMessage with bind opcode,you need add attribute to bind it opCode: {opCode}");
                }
            }
        }
    }
}