using System.Collections.Generic;
using Animancer;
using Entitas;
using UnityEngine;

public sealed class ViewSystem : ReactiveSystem<UnitEntity>
{
    readonly Transform _parent;

    public ViewSystem(Contexts contexts) : base(contexts.unit)
    {
        _parent = new GameObject("Views").transform;
    }

    protected override ICollector<UnitEntity> GetTrigger(IContext<UnitEntity> context)
        => context.CreateCollector(UnitMatcher.Asset);

    protected override bool Filter(UnitEntity entity) => entity.hasAsset && !entity.hasView;

    protected override void Execute(List<UnitEntity> entities)
    {
        foreach (var e in entities)
        {
            e.AddView(instantiateView(e));
        }
    }

    IView instantiateView(UnitEntity entity)
    {
        var gameObject = string.IsNullOrEmpty(entity.asset.value)
            ? new GameObject()
            : Object.Instantiate(Resources.Load<GameObject>(entity.asset.value), _parent);

        if (entity.isMainPlayer && entity.hasUnit)
        {
            gameObject = CreateMainPlayerParent(gameObject);
        }

        gameObject.AddComponent<AnimancerComponent>();
        var view = gameObject.GetComponent<View>();
        if (view == null)
        {
            view = gameObject.AddComponent<View>();
        }

        view.Link(entity);

        return view;
    }

    private GameObject CreateMainPlayerParent(GameObject gameObject)
    {
        var parent = GameObject.Instantiate(Resources.Load<GameObject>("PlayerArmature"));
        // parent.GetComponent<Animator>().avatar = gameObject.GetComponent<Animator>().avatar;
        // GameObject.Destroy(gameObject.GetComponent<Animator>());
        gameObject.transform.SetParent(parent.transform.Find("Geometry"));
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = Vector3.one;

        return parent;
    }
}