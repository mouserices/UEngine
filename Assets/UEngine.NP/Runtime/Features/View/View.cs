using System;
using System.Threading.Tasks;
using Animancer;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class View : MonoBehaviour, IView, IPositionListener, IDestroyedListener,IAnimationListener,IRotationListener
{
    //private AnimancerComponent m_AnimancerComponent;
    
    // public void Awake()
    // {
    //     // m_AnimancerComponent = this.GetComponent<AnimancerComponent>();
    //     // if (m_AnimancerComponent == null)
    //     // {
    //     //     m_AnimancerComponent = this.gameObject.AddComponent<AnimancerComponent>();
    //     // }
    // }
    
    public virtual void Link(IEntity entity)
    {
        gameObject.Link(entity);
        var e = (GameEntity) entity;
        e.AddPositionListener(this);
        e.AddDestroyedListener(this);
        e.AddAnimationListener(this);
        e.AddRotationListener(this);
        
        var pos = e.position.value;
        var rotation = e.rotation.Value;
        transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
        transform.localPosition = new Vector3(rotation.x, rotation.y, rotation.z);
    }
    
    public virtual void OnPosition(GameEntity entity, Vector3 value)
    {
        transform.localPosition = new Vector3(value.x, value.y,value.z);
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
    
    public void OnAnimation(GameEntity entity, string animClipName)
    {
        var animationClip = Resources.Load<AnimationClip>(animClipName);
        this.GetComponent<AnimancerComponent>().Play(animationClip, 0.25f);
    }

    public async void ShortDelay(AnimationClip animationClip)
    {
        await Task.Delay(100);
        Debug.Log("OnAnimation "+Time.frameCount);
        this.GetComponent<AnimancerComponent>().InitializePlayable();
        this.GetComponent<AnimancerComponent>().Play(animationClip, 0.25f);
    }

    public void OnRotation(GameEntity entity, Vector3 value)
    {
        transform.localEulerAngles = value;
    }
}