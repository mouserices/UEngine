using System;
using System.IO;
using ProtoBuf;
using UnityEngine;

namespace UEngine.Net
{
    public class ProbufTest : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("=====begin Serialize");
            using (var file = File.Create("Person.bin"))
            {
                var player = new Player() {ID = 1,Name = "zs",Age = 18};
                Serializer.Serialize(file,player);
            }
            Debug.Log("======Serialize player finished");
            
            Debug.Log("======begin Deserialize player");
            using (var file = File.Open("Person.bin",FileMode.Open))
            {
                Player player = Serializer.Deserialize<Player>(file);
                Debug.Log($"player {player.ID} {player.Name} {player.Age}");
            }
            Debug.Log("======Deserialize player finished");
            
            Debug.Log("=====begin Serialize NPC");
            using (var file = File.Create("Npc.bin"))
            {
                var npc = new NPC() {ID = 1,Name = "zs",Age = 18,NPCID = 100,NPCType = 2};
                Serializer.Serialize(file,npc);
            }
            Debug.Log("======Serialize npc finished");
            
            Debug.Log("======begin Deserialize npc");
            using (var file = File.Open("Npc.bin",FileMode.Open))
            {
                NPC npc = Serializer.Deserialize<NPC>(file);
                Debug.Log($"npc {npc.ID} {npc.Name} {npc.Age} {npc.NPCID} {npc.NPCType}");
            }
            Debug.Log("======Deserialize npc finished");
            
            MemoryStream ms = new MemoryStream();
            ms.Write(BitConverter.GetBytes(11),0,4);
            Debug.Log($"======ms size: {ms.Length} position: {ms.Position}");
            var player2 = new Player() {ID = 1,Name = "zs",Age = 18};
            Serializer.Serialize(ms,player2);
            Debug.Log($"======ms size: {ms.Length} position: {ms.Position}");

            ms.Position = 0;
            ms.WriteByte(2);
            var i = BitConverter.ToInt32(ms.ToArray(), 0);
            Debug.Log($"======i: {i}");

        }
    }
    [ProtoContract]
    [ProtoInclude(4,typeof(NPC))]
    public class Player
    {
        [ProtoMember(1)]
        public int ID;
        [ProtoMember(2)]
        public string Name;
        [ProtoMember(3)]
        public int Age;
    }
    [ProtoContract]
    public class NPC : Player
    {
        [ProtoMember(1)]
        public int NPCID;
        [ProtoMember(2)]
        public int NPCType;
    }
}