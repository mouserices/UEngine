using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class CustomShaderEditor : ShaderGUI
{
    //public enum BlendMode
    //{
    //    Opaque,
    //    Transparent
    //}

    public enum CullMode
    {
        Off,
        Front,
        Back
    }

    public enum RenderQueue
    {
        FromShader,
        Custom,
    }

    private static class Styles
    {
        //public static string renderingMode = "Rendering Mode";
        //public static readonly string[] blendNames = Enum.GetNames(typeof(BlendMode));
        public static string cullMode = "Cull Mode";
        public static readonly string[] CullNames = Enum.GetNames(typeof(CullMode));
        public static readonly string[] QueueNames = Enum.GetNames(typeof(RenderQueue));
        public static string renderQueue = "Render Queue";

        public static GUIContent albedoText = new GUIContent("Albedo");
        public static GUIContent albedoColorText = new GUIContent("AlbedoColor");
        public static GUIContent emissionColorText = new GUIContent("Emission Color");
        public static GUIContent emissionText = new GUIContent("Emission power");
        public static GUIContent bloomPowerText = new GUIContent("Bloom Power");
        public static GUIContent fogText = new GUIContent("Enable Fog");
        public static GUIContent customFogText = new GUIContent("Enable Custom Fog");
        public static GUIContent fakeLightText = new GUIContent("Enable Fake Light");
        public static GUIContent fakeLightColorText = new GUIContent("Fake Light Color");
        public static GUIContent fakeLightDirText = new GUIContent("Fake Light Dir");
        public static GUIContent diffuseText = new GUIContent("Enable Diffuse");
        public static GUIContent diffusePowerText = new GUIContent("Diffuse Power");
        public static GUIContent specularText = new GUIContent("Enable Specular");
        public static GUIContent specularColorText = new GUIContent("Specular Color");
        public static GUIContent specularPowerText = new GUIContent("Specular Power");
        public static GUIContent glossText = new GUIContent("Specular Gloss");
        public static GUIContent fresnelText = new GUIContent("Enable Fresnel");
        public static GUIContent fresnelColor = new GUIContent("Fresnel Color");
        public static GUIContent fresnelScale = new GUIContent("Fresnel Scale");
        public static GUIContent fresnelPower = new GUIContent("Fresnel Power");

        public static GUIContent matcapText = new GUIContent("Enable Matcap");
        public static GUIContent metalTexture = new GUIContent("Metal Texture");
        public static GUIContent matcapTexture = new GUIContent("Matcap Texture");
        public static GUIContent reflectivity = new GUIContent("Reflectivity");
    }

    MaterialProperty blendMode = null;
    MaterialProperty cullMode = null;
    MaterialProperty albedoMap = null;
    MaterialProperty albedoColor = null;
    MaterialProperty emissionColor = null;
    MaterialProperty emission = null;
    MaterialProperty bloomPower = null;
    MaterialProperty fog = null;
    MaterialProperty customFog = null;
    MaterialProperty diffusePower = null;
    MaterialProperty fakeLightColor = null;
    MaterialProperty fakeLightDir = null;
    MaterialProperty specularColor = null;
    MaterialProperty specularPower = null;
    MaterialProperty gloss = null;
    MaterialProperty fresnelText = null;
    MaterialProperty fresnelColor = null;
    MaterialProperty fresnelScale = null;
    MaterialProperty fresnelPower = null;

    MaterialProperty matcapText = null;
    MaterialProperty metalTexture = null;
    MaterialProperty matcapTexture = null;
    MaterialProperty reflectivity = null;


    MaterialEditor m_MaterialEditor;

    private const string ENABLE_DIFFUSE_KEYWORD = "ENABLE_DIFFUSE";
    private const string ENABLE_SPECULAR_KEYWORD = "ENABLE_SPECULAR";
    private const string ENABLE_FAKELIGHT_KEYWORD = "ENABLE_FAKE_LIGHTING";
    private const string ENABLE_FRESNEL_KEYWORD = "ENABLE_FRESNEL";
    private const string ENABLE_MATCAP_KEYWORD = "ENABLE_MATCAP";


    public void FindProperties(MaterialProperty[] props)
    {
        //blendMode = FindProperty("_Mode", props);
        cullMode = FindProperty("_Cull", props);
        albedoMap = FindProperty("_MainTex", props);
        albedoColor = FindProperty("_Color", props);
        emissionColor = FindProperty("_EmissionColor", props);
        emission = FindProperty("_Emmission", props);
        bloomPower = FindProperty("_BloomPower", props);
        fog = FindProperty("_EnableFog", props);
        customFog = FindProperty("_EnableCustomFog", props);
        diffusePower = FindProperty("_DiffusePower", props);
        fakeLightColor = FindProperty("_FakeLightColor", props);
        fakeLightDir = FindProperty("_FakeLightDir", props);
        specularColor = FindProperty("_SpecularColor", props);
        specularPower = FindProperty("_SpecularPower", props);
        gloss = FindProperty("_Gloss", props);
        fresnelText = FindProperty("_EnableFresnel", props);
        fresnelColor = FindProperty("_FresnelColor", props);
        fresnelPower = FindProperty("_FresnelPower", props);
        fresnelScale = FindProperty("_FresnelScale", props);

        matcapText = FindProperty("_EnableMatCap", props);
        metalTexture = FindProperty("_MetalTex", props);
        matcapTexture = FindProperty("_EnvTex", props);
        reflectivity = FindProperty("_Spread", props);
    }

    bool m_FirstTimeApply = true;

    public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
    {
        base.AssignNewShaderToMaterial(material, oldShader, newShader);

        material.SetTexture("_BumpMap", null);
        material.SetTexture("_DetailAlbedoMap", null);
        material.SetTexture("_DetailMask", null);
        material.SetTexture("_DetailNormalMap", null);
        material.SetTexture("_EmissionMap", null);
        material.SetTexture("_MetallicGlossMap", null);
        material.SetTexture("_OcclusionMap", null);
        material.SetTexture("_ParallaxMap", null);
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        FindProperties(props);
        m_MaterialEditor = materialEditor;
        Material material = materialEditor.target as Material;

        if (m_FirstTimeApply)
        {
            MaterialChanged(material);
            m_FirstTimeApply = false;
        }

        ShaderPropertiesGUI(material);
    }

    public void ShaderPropertiesGUI(Material material)
    {
        EditorGUIUtility.labelWidth = 0f;
        bool diffus = material.shaderKeywords.Contains(ENABLE_DIFFUSE_KEYWORD);
        bool specular = material.shaderKeywords.Contains(ENABLE_SPECULAR_KEYWORD);
        bool fakeLight = material.shaderKeywords.Contains(ENABLE_FAKELIGHT_KEYWORD);
        bool fresnel = material.shaderKeywords.Contains(ENABLE_FRESNEL_KEYWORD);
        bool matcap = material.shaderKeywords.Contains(ENABLE_MATCAP_KEYWORD);

        EditorGUI.BeginChangeCheck();
        {
            //BlendModePopup();
            EditorGUILayout.Space();
            CullModePopup();
            EditorGUILayout.Space();
            RenderQueuePopup(material);
            EditorGUILayout.Space();
            //m_MaterialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap, albedoColor);
            m_MaterialEditor.ShaderProperty(albedoMap, Styles.albedoText);
            m_MaterialEditor.ShaderProperty(albedoColor, Styles.albedoColorText);
            m_MaterialEditor.ShaderProperty(emissionColor, Styles.emissionColorText);
            m_MaterialEditor.ShaderProperty(emission, Styles.emissionText);
            m_MaterialEditor.ShaderProperty(bloomPower, Styles.bloomPowerText);
            EditorGUILayout.Space();
            m_MaterialEditor.ShaderProperty(fog, Styles.fogText);
            m_MaterialEditor.ShaderProperty(customFog, Styles.customFogText);
            bool diffusNew = EditorGUILayout.Toggle(Styles.diffuseText, diffus);
            if (diffusNew)
            {
                EditorGUI.indentLevel++;
                material.EnableKeyword(ENABLE_DIFFUSE_KEYWORD);
                m_MaterialEditor.ShaderProperty(diffusePower, Styles.diffusePowerText);

                bool fakeLightNew = EditorGUILayout.Toggle(Styles.fakeLightText, fakeLight);
                if (fakeLightNew)
                {
                    EditorGUI.indentLevel++;
                    material.EnableKeyword(ENABLE_FAKELIGHT_KEYWORD);
                    m_MaterialEditor.ShaderProperty(fakeLightColor, Styles.fakeLightColorText);
                    m_MaterialEditor.ShaderProperty(fakeLightDir, Styles.fakeLightDirText);
                    EditorGUI.indentLevel--;
                }
                else
                {
                    material.DisableKeyword(ENABLE_FAKELIGHT_KEYWORD);
                }

                bool specularNew = EditorGUILayout.Toggle(Styles.specularText, specular);
                if (specularNew)
                {
                    EditorGUI.indentLevel++;
                    material.EnableKeyword(ENABLE_SPECULAR_KEYWORD);
                    m_MaterialEditor.ShaderProperty(specularColor, Styles.specularColorText);
                    m_MaterialEditor.ShaderProperty(specularPower, Styles.specularPowerText);
                    m_MaterialEditor.ShaderProperty(gloss, Styles.glossText);
                    EditorGUI.indentLevel--;
                }
                else
                {
                    material.DisableKeyword(ENABLE_SPECULAR_KEYWORD);
                }

                bool fresnelNew = EditorGUILayout.Toggle(Styles.fresnelText, fresnel);
                if (fresnelNew)
                {
                    EditorGUI.indentLevel++;
                    material.EnableKeyword(ENABLE_FRESNEL_KEYWORD);
                    m_MaterialEditor.ShaderProperty(fresnelColor, Styles.fresnelColor);
                    m_MaterialEditor.ShaderProperty(fresnelPower, Styles.fresnelPower);
                    m_MaterialEditor.ShaderProperty(fresnelScale, Styles.fresnelScale);
                    EditorGUI.indentLevel--;
                }
                else
                {
                    material.DisableKeyword(ENABLE_FRESNEL_KEYWORD);
                }
                EditorGUI.indentLevel--;
            }
            else
            {
                material.DisableKeyword(ENABLE_DIFFUSE_KEYWORD);
            }

            bool matcapNew = EditorGUILayout.Toggle(Styles.matcapText, matcap);
            if (matcapNew)
            {
                EditorGUI.indentLevel++;
                material.EnableKeyword(ENABLE_MATCAP_KEYWORD);
                m_MaterialEditor.ShaderProperty(metalTexture, Styles.metalTexture);
                m_MaterialEditor.ShaderProperty(matcapTexture, Styles.matcapTexture);
                m_MaterialEditor.ShaderProperty(reflectivity, Styles.reflectivity);
                EditorGUI.indentLevel--;
            }
            else
            {
                material.DisableKeyword(ENABLE_MATCAP_KEYWORD);
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            //foreach (var obj in blendMode.targets)
            //MaterialChanged((Material)obj);
            MaterialChanged(material);
        }
    }

    //void BlendModePopup()
    //{
    //    EditorGUI.showMixedValue = blendMode.hasMixedValue;
    //    var mode = (BlendMode)blendMode.floatValue;

    //    EditorGUI.BeginChangeCheck();
    //    mode = (BlendMode)EditorGUILayout.Popup(Styles.renderingMode, (int)mode, Styles.blendNames);
    //    if (EditorGUI.EndChangeCheck())
    //    {
    //        m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
    //        blendMode.floatValue = (float)mode;
    //    }
    //    EditorGUI.showMixedValue = false;
    //}

    void CullModePopup()
    {
        EditorGUI.showMixedValue = cullMode.hasMixedValue;
        var mode = (CullMode)cullMode.floatValue;

        EditorGUI.BeginChangeCheck();
        mode = (CullMode)EditorGUILayout.Popup(Styles.cullMode, (int)mode, Styles.CullNames);
        if (EditorGUI.EndChangeCheck())
        {
            m_MaterialEditor.RegisterPropertyChangeUndo("Cull Mode");
            cullMode.floatValue = (float)mode;
        }
        EditorGUI.showMixedValue = false;
    }

    static RenderQueue renderQueueEnum = 0;
    static int renderQueueNum = 2001;

    //这个地方对于是否自定义了渲染队列比较难进行判断
    //目前的逻辑是根据当前渲染队列是否是2000或者3000进行判断的，如果不是这个值，则认为使用了自定义的渲染队列
    //后续考虑新的实现方式
    void RenderQueuePopup(Material material)
    {
        if (material.renderQueue != 2000 && material.renderQueue != 3000)
        {
            renderQueueEnum = RenderQueue.Custom;
            renderQueueNum = material.renderQueue;
        }
        else
        {
            renderQueueEnum = RenderQueue.FromShader;
        }

        EditorGUI.BeginChangeCheck();

        RenderQueue queueEnum = (RenderQueue)EditorGUILayout.Popup(Styles.renderQueue, (int)renderQueueEnum, Styles.QueueNames);

        if (queueEnum == RenderQueue.Custom)
        {
            int queueNum = (int)EditorGUILayout.FloatField(renderQueueNum);
            if (queueNum == 2000 || queueNum == 3000)
            {
                queueNum++;//当渲染队列为这两个值时，会被认为是使用shader默认队列，所以手动加一来避免这种情况
            }
            renderQueueNum = queueNum;
        }
        else
        {
            //    BlendMode blendMode = (BlendMode)material.GetFloat("_Mode");
            //    if (blendMode == BlendMode.Opaque)
            //    {
            //        renderQueueNum = 2000;
            //    }
            //    else
            //    {
            //        renderQueueNum = 3000;
            //    }
            renderQueueNum = 2000;
        }

        if (EditorGUI.EndChangeCheck())
        {
            renderQueueEnum = queueEnum;
            material.renderQueue = renderQueueNum;
        }
    }


    static void MaterialChanged(Material material)
    {
        //SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_Mode"), (CullMode)material.GetFloat("_Cull"));
        SetupMaterialWithBlendMode(material, (CullMode)material.GetFloat("_Cull"));
    }

    public static void SetupMaterialWithBlendMode(Material material, CullMode cullMode)
    //public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode, CullMode cullMode)
    {
        if (material.renderQueue != 2000 && material.renderQueue != 3000)
        {
            renderQueueEnum = RenderQueue.Custom;
        }
        else
        {
            renderQueueEnum = RenderQueue.FromShader;
        }

        //switch (blendMode)
        //{
        //    case BlendMode.Opaque:
        //        material.SetOverrideTag("RenderType", "Opaque");
        //        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        //        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        //        material.SetInt("_ZWrite", 1);
        //        material.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
        //        //material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Back);
        //        if (renderQueueEnum == RenderQueue.FromShader)
        //        {
        //            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        //        }
        //        break;
        //    case BlendMode.Transparent:
        //        material.SetOverrideTag("RenderType", "Transparent");
        //        //material.SetOverrideTag("ForceNoShadowCasting", "true");
        //        //material.SetOverrideTag("IgnoreProjector", "true");
        //        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        //        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //        material.SetInt("_ZWrite", 0);
        //        material.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
        //        //material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Back);
        //        if (renderQueueEnum == RenderQueue.FromShader)
        //        {
        //            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        //        }
        //        break;
        //}

        switch (cullMode)
        {
            case CullMode.Back:
                material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Back);
                break;
            case CullMode.Front:
                material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Front);
                break;
            case CullMode.Off:
                material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                break;
        }
    }

}
