using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIReferenceCollector : SerializedMonoBehaviour
{
    private static readonly string UICONFIGPATH = "Assets/UEngine.UI/Resources/UIConfig.prefab";
    private static readonly string SYS_CODE_PATH = "UEngine.UI/Example/System";
    private static readonly string COM_CODE_PATH = "UEngine.UI/Example/Component";
    private static readonly string SYS_TEMPLETE_PATH = "UEngine.UI/Editor/UISystemTemplete.txt";

    
    [BoxGroup("请直接拖住UI节点到下方的位置，自动识别类型以及名称装配到引用列表中")] 
    [OnValueChanged("OnValueChanged")]
    public GameObject UIObj;

    [BoxGroup("下面是所有UI节点的引用列表，自动生成，一般不需要动，如果发现引用类型错误，可以手动修改")]
    [DictionaryDrawerSettings(KeyLabel = "节点名称", ValueLabel = "节点信息", DisplayMode = DictionaryDisplayOptions.OneLine)]
    public Dictionary<String, RefObj> References = new Dictionary<String, RefObj>();

    [BoxGroup("下面是UI窗口配置,需要手动按需选择")]
    [LabelText("UIType->UI位置类型")]
    [GUIColor(0,1,0)]
    public UIType UIType;
    
    [BoxGroup("下面是UI窗口配置,需要手动按需选择")]
    [LabelText("UIShowMode->UI显示类型")]
    [GUIColor(0,1,0)]
    public UIShowMode UIShowMode;
    
    [BoxGroup("下面是UI窗口配置,需要手动按需选择")]
    [GUIColor(0,1,0)]
    [LabelText("UILucenyType->UI遮罩类型")]
    public UILucenyType UILucenyType;

    [Button("点击此按钮生成UI配置文件(当UI窗口配置信息改变时必须点击)")]
    private void GenerateConfig()
    {
        var loadAssetAtPath = AssetDatabase.LoadAssetAtPath<GameObject>(UICONFIGPATH);
        var uiWindowConfig = loadAssetAtPath.GetComponent<UIWindowConfig>();

        var uiConfig = uiWindowConfig.UIConfigs.Find((p) => { return p.UIWindowName == this.GetWindowName(); });
        if (uiConfig == null)
        {
            uiConfig = new UIConfig();
            uiWindowConfig.UIConfigs.Add(uiConfig);
        }
        
        uiConfig.UIWindowName = this.GetWindowName();
        uiConfig.NodeComponentName = $"{this.GetNodeComponentName()}NodeComponent";
        uiConfig.UIType = this.UIType;
        uiConfig.UIShowMode = this.UIShowMode;
        uiConfig.UILucenyType = this.UILucenyType;
    }
    
    [Button("点击此按钮生成UINodeComponent脚本(当UI窗口引用的节点改变时必须点击)")]
    private void GenerateUINodeComponentCode()
    {
        var uiDir = $"{Application.dataPath}/{COM_CODE_PATH}/{this.GetNodeComponentName()}";
        if (!Directory.Exists(uiDir))
        {
            Directory.CreateDirectory(uiDir);
        }
        
        var filePath = $"{Application.dataPath}/{COM_CODE_PATH}/{this.GetNodeComponentName()}/{this.GetNodeComponentName()}NodeComponent.cs";
        FileStream fs = File.Create(filePath);
        fs.Dispose();
        
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("using UnityEngine.UI;");
        builder.AppendLine();
        builder.AppendLine("[UINode]");
        builder.AppendLine($"public struct {this.GetNodeComponentName()}NodeComponent");
        builder.AppendLine("{");
        foreach (var reference in this.References)
        {
            builder.AppendLine($"   public {reference.Value.RefType.Name} {reference.Key};");
        }
        builder.AppendLine("}");
            
        File.WriteAllText(filePath,builder.ToString());
    }
    [Button("点击此按钮生成UISystem脚本(基本上只需要点击一次，以后不需要再次点击)")]
    private void GenerateUISystemCode()
    {
        var uiDir = $"{Application.dataPath}/{SYS_CODE_PATH}/{this.GetNodeComponentName()}";
        if (!Directory.Exists(uiDir))
        {
            Directory.CreateDirectory(uiDir);
        }
        
        var filePath = $"{Application.dataPath}/{SYS_CODE_PATH}/{this.GetNodeComponentName()}/{this.GetNodeComponentName()}System.cs";
        FileStream fs = File.Create(filePath);
        fs.Dispose();

        var systemName = $"{this.GetNodeComponentName()}System";
        var nodeComponentName = $"{this.GetNodeComponentName()}NodeComponent";
        //读取模版
        var templeteFilePath = $"{Application.dataPath}/{SYS_TEMPLETE_PATH}";
        string code = File.ReadAllText(templeteFilePath);
        var replace = code.Replace("UISystemTemplete", systemName).Replace("UINodeTemplete", nodeComponentName);

        File.WriteAllText(filePath,replace);
    }

    private string GetNodeComponentName()
    {
        return this.gameObject.name.Replace("Window", "");
    }

    private string GetWindowName()
    {
        return this.gameObject.name;
    }

    public Object GetObject(string nodeName)
    {
        if (References.ContainsKey(nodeName))
        {
            return References[nodeName].Obj;
        }

        return null;
    }

    public void OnValueChanged()
    {
        if (UIObj == null)
        {
            return;
        }
        foreach (var refType in RefObj.RefTypes)
        {
            Component component = UIObj.GetComponent(refType);
            if (component != null)
            {
                var refObj = new RefObj() {RefType = component.GetType(), Obj = component};
                if (!References.ContainsKey(UIObj.name))
                {
                    References.Add(UIObj.name, refObj);
                    break;
                }
            }
        }

        UIObj = null;
    }
}

public struct RefObj
{
    [ValueDropdown("RefTypes")] public Type RefType;
    public Object Obj;

    public static Type[] RefTypes = new[]
    {
        typeof(Scrollbar),
        typeof(Slider),
        typeof(Dropdown),
        typeof(Toggle),
        typeof(Button),
        typeof(InputField),
        typeof(Image),
        typeof(Text),
        typeof(Transform),
        typeof(GameObject),
    };
}

public enum RefType
{
    Transfrom,
    Button,
    Sprite,
    Text
}