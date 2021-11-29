
using System;
using System.Collections.Generic;
using System.Linq;
using NPBehave;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace UEngine.NP
{
    public class NP_ServiceNodeData: NP_NodeDataBase
    {
        [HideInEditorMode]
        public NPBehave.Service m_Service;

        [LabelText("服务类型")]
        [TypeFilter("GetFilteredTypeList")]
        public NP_BaseService NpBaseService;
        public override Node NP_GetNode()
        {
            return this.m_Service;
        }

        public override Decorator CreateDecoratorNode(Node node,Skill skill)
        {
            NpBaseService.Skill = skill;
            this.m_Service = new NPBehave.Service(NpBaseService.GetInterval(), this.NpBaseService.GetServiceAction(), node);
            return this.m_Service;
        }
        
        public IEnumerable<Type> GetFilteredTypeList()
        {
            var q = typeof(NP_BaseService).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // 不包括 BaseClass
                .Where(x => !x.IsGenericTypeDefinition)                             // 不包括 C1<>
                .Where(x => typeof(NP_BaseService).IsAssignableFrom(x));                 // 排除不从BaseClass继承的类 
            return q;
        }
    }
}