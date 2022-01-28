using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Animancer;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class View : MonoBehaviour, IView, IPositionListener, IUnitDestroyedListener, IAnimationListener,
    IRotationListener, IScaleListener, IParentListener, IBoxColliderListener,
    ILayerListener
{
    public Dictionary<string, Transform> Bones = new Dictionary<string, Transform>();

    public void Awake()
    {
        initBones();
    }

    #region Bone

    private void initBones()
    {
        var bones = this.GetComponentsInChildren<Bone>();
        foreach (Bone bone in bones)
        {
            if (!Bones.ContainsKey(bone.name))
            {
                Bones.Add(bone.name, bone.transform);
            }
        }
    }

    public Transform GetBone(string boneName)
    {
        Transform bone;
        Bones.TryGetValue(boneName, out bone);
        return bone;
    }

    #endregion


    public virtual void Link(IEntity entity)
    {
        gameObject.Link(entity);
        var e = (UnitEntity) entity;
        e.AddPositionListener(this);
        e.AddUnitDestroyedListener(this);
        e.AddAnimationListener(this);
        e.AddRotationListener(this);
        e.AddScaleListener(this);
        e.AddParentListener(this);
        e.AddBoxColliderListener(this);
        e.AddLayerListener(this);

       
        if (e.hasParent)
        {
            transform.parent = e.parent.Value;
        }

        if (e.hasPosition)
        { 
            transform.localPosition = new Vector3(e.position.value.x, e.position.value.y, e.position.value.z);
        }
        if (e.hasRotation)
        {
            transform.localEulerAngles = new Vector3(e.rotation.Value.x, e.rotation.Value.y, e.rotation.Value.z);
        }
        if (e.hasScale)
        {
            transform.localScale = new Vector3(e.scale.Value.x, e.scale.Value.y, e.scale.Value.z);
        }

        if (e.isBoxCollider)
        {
            addBoxCollider();
        }

        if (e.hasLayer)
        {
            changeLayer(e.layer.Value);
        }
    }

    public GameObject GetObj()
    {
        return this.gameObject;
    }

    private Vector3 targetPos;
    private Vector3 prePos;
    private bool blerp;
    private float totalTime;
    public virtual void OnPosition(UnitEntity entity, Vector3 value)
    {
        prePos = this.transform.localPosition;
        targetPos = value;
        blerp = true;
        totalTime = 0;
    }

    public void Update()
    {
        if (blerp)
        {
            totalTime += Time.deltaTime;
            var deltaTime = totalTime / 0.15f;
            transform.localPosition = Vector3.Lerp(prePos,new Vector3(targetPos.x, targetPos.y, targetPos.z),deltaTime) ;
            var curPos = transform.localPosition;
            if (Vector3.Distance(curPos,targetPos) <=0)
            {
                blerp = false;
            }
            //Debug.Log($"deltaTime: {deltaTime} curPos: {curPos.x} {curPos.y} {curPos.z} targetPos: {targetPos.x} {targetPos.y} {targetPos.z} Distance: {Vector3.Distance(curPos,targetPos)}");
        }
    }

    public virtual void OnDestroyed(UnitEntity entity)
    {
        destroy();
    }

    protected virtual void destroy()
    {
        gameObject.Unlink();
        Destroy(gameObject);
    }

    public void OnAnimation(UnitEntity entity, string animClipName,float speed,Action onEnd)
    {
        int pre = Time.frameCount;
        var animationClip = Resources.Load<AnimationClip>(animClipName);
        var animancerState = this.GetComponent<AnimancerComponent>().Play(animationClip, 0.25f);
        animancerState.Time = 0;
        animancerState.Speed = speed;
        if (!animancerState.IsLooping)
        {
            animancerState.Events.OnEnd = () =>
            {
                onEnd?.Invoke();
                onEnd = null;
            };
        }
    }

    public void OnRotation(UnitEntity entity, Vector3 value)
    {
        transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles,value,0.5f);
    }

    public void OnScale(UnitEntity entity, Vector3 value)
    {
        transform.localScale = new Vector3(value.x, value.y, value.z);
    }

    public void OnParent(UnitEntity entity, Transform value)
    {
        transform.parent = value;
    }

    public void OnBoxCollider(UnitEntity entity)
    {
        addBoxCollider();
    }

    void addBoxCollider()
    {
        var addComponent = this.GetComponent<BoxCollider>();
        if (addComponent == null)
        {
            addComponent = this.gameObject.AddComponent<BoxCollider>();
            addComponent.isTrigger = true;
            this.gameObject.AddComponent<OnTriggerCallBack>();
        }
    }

    void changeLayer(int layer)
    {
        this.gameObject.layer = layer;
    }

    public void OnLayer(UnitEntity entity, int value)
    {
        changeLayer(value);
    }
}