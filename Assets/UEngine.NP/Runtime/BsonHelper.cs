using System;
using System.Collections.Generic;
using System.Numerics;
using MongoDB.Bson.Serialization;

#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UEngine.NP
{
    public class BsonDeserializerRegisterAttribute: Attribute
    {
    }

    /// <summary>
    /// Bson序列化反序列化辅助类
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class BsonHelper
    {
        static BsonHelper()
        {
            RegisterStructSerializer();
            RegisterAllSubClassForDeserialize();
        }

        /// <summary>
        /// 注册所有需要使用Bson序列化反序列化的结构体
        /// </summary>
        public static void RegisterStructSerializer()
        {
            BsonSerializer.RegisterSerializer(typeof (Vector2), new StructBsonSerialize<Vector2>());
            BsonSerializer.RegisterSerializer(typeof (Vector3), new StructBsonSerialize<Vector3>());
            BsonSerializer.RegisterSerializer(typeof (VTD_Id), new StructBsonSerialize<VTD_Id>());
            BsonSerializer.RegisterSerializer(typeof (VTD_EventId), new StructBsonSerialize<VTD_EventId>());
        }

        /// <summary>
        /// 注册所有供反序列化的子类
        /// </summary>
        public static void RegisterAllSubClassForDeserialize()
        {
            List<Type> parenTypes = new List<Type>();
            Type[] allTypes = typeof (BsonHelper).Assembly.GetTypes();
            // registe by BsonDeserializerRegisterAttribute Automatically
            foreach (Type type in allTypes)
            {
                BsonDeserializerRegisterAttribute[] bsonDeserializerRegisterAttributes = 
                        type.GetCustomAttributes(typeof (BsonDeserializerRegisterAttribute), false) as BsonDeserializerRegisterAttribute[];
                if (bsonDeserializerRegisterAttributes.Length > 0)
                {
                    parenTypes.Add(type);
                }
            }
            
            foreach (Type type in allTypes)
            {
                foreach (var parentType in parenTypes)
                {
                    if (parentType.IsAssignableFrom(type))
                    {
                        try
                        {
                            BsonClassMap.LookupClassMap(type);
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }
                }
            }

            //技能配置反序列化相关(manually because these type cannot Automatically register)
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_Int));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_Bool));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_Float));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_String));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_Vector3));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_Long));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_List_Long));
        }

        /// <summary>
        /// 初始化BsonHelper
        /// </summary>
        public static void Init()
        {
            //调用这个是为了执行静态构造方法
        }
    }
}