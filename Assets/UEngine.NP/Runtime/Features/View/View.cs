using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Animancer;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class View : MonoBehaviour, IView, IPositionListener, IDestroyedListener, IAnimationListener,
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
        var e = (GameEntity) entity;
        e.AddPositionListener(this);
        e.AddDestroyedListener(this);
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

    public virtual void OnPosition(GameEntity entity, Vector3 value)
    {
        transform.localPosition = new Vector3(value.x, value.y, value.z);
    }

    public virtual void OnDestroyed(GameEntity entity)
    {
        destroy();
    }

    protected virtual void destroy()
    {
        gameObject.Unlink();
        Destroy(gameObject);
    }

    public void OnAnimation(GameEntity entity, string animClipName,float speed,Action onEnd)
    {
        int pre = Time.frameCount;
        Debug.Log($"play anim:{animClipName} frameCount: {Time.frameCount}");
        var animationClip = Resources.Load<AnimationClip>(animClipName);
        var animancerState = this.GetComponent<AnimancerComponent>().Play(animationClip, 0.25f);
        animancerState.Time = 0;
        animancerState.Speed = speed;
        if (!animancerState.IsLooping)
        {
            animancerState.Events.OnEnd = () =>
            {
                Debug.Log($"play anim end:{animClipName} frameCount: {Time.frameCount}");
                onEnd?.Invoke();
                onEnd = null;
            };
        }
    }

    public void OnRotation(GameEntity entity, Vector3 value)
    {
        transform.localEulerAngles = value;
    }

    public void OnScale(GameEntity entity, Vector3 value)
    {
        transform.localScale = new Vector3(value.x, value.y, value.z);
    }

    public void OnParent(GameEntity entity, Transform value)
    {
        transform.parent = value;
    }

    public void OnBoxCollider(GameEntity entity)
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

    public void OnLayer(GameEntity entity, int value)
    {
        changeLayer(value);
    }
}