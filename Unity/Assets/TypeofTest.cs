using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Profiling;

public class Player
{
    
}

public struct strc
{
    
}

public class TypeofTest : MonoBehaviour
{
    private Player p;
    private int inta;

    private strc Strc;
    // Start is called before the first frame update
    void Start()
    {
        p = new Player();
        Strc = new strc();
    }

    // Update is called once per frame
    void Update()
    {
        Profiler.BeginSample("============引用类型 typeof begin");
        var type = typeof(Player);
        Profiler.EndSample();
        
        Profiler.BeginSample("============引用类型 GetType begin");
        var type1 = p.GetType();
        Profiler.EndSample();
        
        Profiler.BeginSample("============int typeof begin");
        var type2 = typeof(int);
        Profiler.EndSample();
        
        Profiler.BeginSample("============int GetType begin");
        var type3 = inta.GetType();
        Profiler.EndSample();
        
        Profiler.BeginSample("============struct typeof begin");
        var type4 = typeof(strc);
        Profiler.EndSample();
        
        Profiler.BeginSample("============struct GetType begin");
        var type5 = Strc.GetType();
        Profiler.EndSample();
    }
}
