using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HologramEditor : ShaderGUI
{
    Material target;
    MaterialEditor editor;
    MaterialProperty[] properties;
    static GUIContent staticLabel = new GUIContent();
    // 折叠参数
    bool expand_mask;
    bool expand_ver_gliter1;
    bool expand_ver_gliter2;
    bool expand_line1;
    bool expand_line2;
    bool expand_fresnel;
    bool expand_grain;
    bool expand_color_glitch;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        // base.OnGUI(materialEditor, properties);
        this.target = materialEditor.target as Material;
        this.editor = materialEditor;
        this.properties = properties;
        DoBase();
        DoHgMask();
        DoVertexGliter();
        DoScanLine();
        DoFresnel();
        DoGrain();
        DoColorGlitch();
    }

    // 基础参数
    void DoBase()
    {
        // 全息颜色
        MaterialProperty hg_color = FindProperty("_HologramColor");
        editor.ShaderProperty(hg_color, MakeLabel("全息整体颜色"));
        MaterialProperty hg_alpha = FindProperty("_HologramAlpha");
        editor.ShaderProperty(hg_alpha, MakeLabel("全息整体透明度"));
        MaterialProperty glitch_map = FindProperty("_HologramNoiseMap");
        editor.TextureProperty(glitch_map, "噪声纹理", false);
        GUILayout.Space(10);
    }

    // 蒙版
    void DoHgMask()
    {
        // 这个是Editor的折叠UI，当这个值为true时便会渲染下面的属性词条UI出来
        expand_mask = EditorGUILayout.Foldout(expand_mask, "整体蒙版");
        if(expand_mask) 
        {
            EditorGUI.indentLevel += 1;
            // 获取材质属性
            MaterialProperty mask = FindProperty("_HologramMaskMap");
            MaterialProperty mask_affect = FindProperty("_HologramMaskAffect");
            // 绘制EditorGUI属性词条
            editor.TextureProperty(mask, "蒙版");
            editor.ShaderProperty(mask_affect, "蒙版强度");
            EditorGUI.indentLevel -= 1;
        }
    }

    // 顶点故障效果
    void DoVertexGliter()
    {
        expand_ver_gliter1 = EditorGUILayout.Foldout(expand_ver_gliter1, "顶点故障1");
        if(expand_ver_gliter1)
        {
            EditorGUI.indentLevel += 1;
            MaterialProperty gliter1 = FindProperty("_HologramGliterData1");
            GUILayout.Label("x：速度，y：抖动范围，z：抖动偏移量(正负区分左右抖动)，w代表频率(0~0.99)", EditorStyles.centeredGreyMiniLabel);
            editor.VectorProperty(gliter1, "故障参数1");
            EditorGUI.indentLevel -= 1;
        }
        expand_ver_gliter2 = EditorGUILayout.Foldout(expand_ver_gliter2, "顶点故障2");
        if(expand_ver_gliter2)
        {
            EditorGUI.indentLevel += 1;
            MaterialProperty gliter2 = FindProperty("_HologramGliterData2");
            GUILayout.Label("x：速度，y：抖动范围，z：抖动偏移量(正负区分左右抖动)，w代表频率(0~0.99)", EditorStyles.centeredGreyMiniLabel);
            editor.VectorProperty(gliter2, "故障参数2");
            EditorGUI.indentLevel -= 1;
        }
    }

    // 扫描线属性
    void DoScanLine()
    {
        expand_line1 = EditorGUILayout.Foldout(expand_line1, "扫描线效果1");
        if(expand_line1)
        {
            EditorGUI.indentLevel += 1;
            MaterialProperty line1_map = FindProperty("_HologramLine1");
            MaterialProperty line1_speed = FindProperty("_HologramLine1Speed");
            MaterialProperty line1_tilling = FindProperty("_HologramLine1Frequency");
            MaterialProperty line1_alpha = FindProperty("_HologramLine1Alpha");
            editor.TextureProperty(line1_map, "扫描线1纹理", false);
            editor.ShaderProperty(line1_speed, "扫描线1速度");
            editor.ShaderProperty(line1_tilling, "扫描线1tilling");
            editor.ShaderProperty(line1_alpha, "扫描线1透明度");
            EditorGUI.indentLevel -= 1;
        }

        expand_line2 = EditorGUILayout.Foldout(expand_line2, "扫描线效果2");
        if(expand_line2)
        {
            EditorGUI.indentLevel += 1;
            EditorGUI.BeginChangeCheck();
            MaterialProperty line2_tog = FindProperty("_HologramLine2Tog");
            editor.ShaderProperty(line2_tog, "使用扫描线2");
            // 当扫描线2的选项勾选之后才会绘制下方的属性GUI
            bool line2_enabled = GetToggleEnabled(line2_tog);
            if(line2_enabled)
            {
                MaterialProperty line2_map = FindProperty("_HologramLine2");
                MaterialProperty line2_speed = FindProperty("_HologramLine2Speed");
                MaterialProperty line2_tilling = FindProperty("_HologramLine2Frequency");
                MaterialProperty line2_alpha = FindProperty("_HologramLine2Alpha");
                editor.TextureProperty(line2_map, "扫描线2纹理", false);
                editor.ShaderProperty(line2_speed, "扫描线2速度");
                editor.ShaderProperty(line2_tilling, "扫描线2tilling");
                editor.ShaderProperty(line2_alpha, "扫描线2透明度");
            }
            EditorGUI.indentLevel -= 1;
            if(EditorGUI.EndChangeCheck())
            {
                SetKeyword("_USE_SCANLINE2", line2_enabled);
            }
        }
    }

    // 颜色故障效果
    void DoColorGlitch()
    {
        expand_color_glitch = EditorGUILayout.Foldout(expand_color_glitch, "全息颜色故障效果");
        if(expand_color_glitch)
        {
            EditorGUI.indentLevel += 1;
            MaterialProperty c_g_tog = FindProperty("_HologramColorGlitchTog");
            editor.ShaderProperty(c_g_tog, "使用颜色故障");
            if(GetToggleEnabled(c_g_tog))
            {
                MaterialProperty glitch_val = FindProperty("_HologramColorGlitch");
                MaterialProperty glitch_data = FindProperty("_HologramColorGlitchData");
                MaterialProperty glitch_min = FindProperty("_HologramColorGlitchMin");
                editor.ShaderProperty(glitch_val, "颜色故障强度");
                editor.VectorProperty(glitch_data, "噪声速度(使用XY分量)");
                editor.ShaderProperty(glitch_min, "颜色故障最小值");
            }
            EditorGUI.indentLevel -= 1;
        }
    }

    // 菲涅尔反射
    void DoFresnel()
    {
        expand_fresnel = EditorGUILayout.Foldout(expand_fresnel, "全息Fresnel效果");
        if(expand_fresnel)
        {
            EditorGUI.indentLevel += 1;
            MaterialProperty fresnel_scale = FindProperty("_FresnelScale");
            MaterialProperty fresnel_power = FindProperty("_FresnelPower");
            editor.ShaderProperty(fresnel_scale, "Fresnel大小");
            editor.ShaderProperty(fresnel_power, "Fresnel指数");
            EditorGUI.indentLevel -= 1;
        }
    }

    // 颗粒效果
    void DoGrain()
    {
        expand_grain = EditorGUILayout.Foldout(expand_grain, "颗粒效果");
        if(expand_grain)
        {
            EditorGUI.indentLevel += 1;
            MaterialProperty grain_data = FindProperty("_HologramGrainData");
            MaterialProperty grain_speed = FindProperty("_HologramGrainSpeed");
            MaterialProperty grain_affect = FindProperty("_HologramGrainAffect");
            GUILayout.Label("xy：噪声采样tilling(需要噪声图)，zw：噪声颜色区间(0~1)", EditorStyles.centeredGreyMiniLabel);
            editor.VectorProperty(grain_data, "颗粒参数");
            editor.ShaderProperty(grain_speed, "颗粒效果速度");
            editor.ShaderProperty(grain_affect, "颗粒效果强度");
            EditorGUI.indentLevel -= 1;
        }
    }


    // 利用方法优化GUI代码结构，使得代码更容易看
    MaterialProperty FindProperty(string name)
    {
        return FindProperty(name, properties);
    }

    static GUIContent MakeLabel(string text, string tooltip = null)
    {
        staticLabel.text = text;
        staticLabel.tooltip = tooltip;
        return staticLabel;
    }

    static GUIContent MakeLabel(MaterialProperty property, string tooltip = null)
    {
        staticLabel.text = property.displayName;
        staticLabel.tooltip = tooltip;
        return staticLabel;
    }
    // 设置shader关键字
    void SetKeyword(string keyword, bool state)
    {
        if(state)
        {
            foreach(Material m in editor.targets)
            {
                m.EnableKeyword(keyword);
            }
        }
        else{
            foreach(Material m in editor.targets)
            {
                m.DisableKeyword(keyword);
            }
        }
    }

    bool GetToggleEnabled(MaterialProperty property)
    {
        return (int)property.floatValue == 1;
    }

}
