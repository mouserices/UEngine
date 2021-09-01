using System.Collections.Generic;
using Animancer;
using Entitas;
using UnityEngine;

public sealed class AddViewSystem : ReactiveSystem<GameEntity>
{
    readonly Transform _parent;

    public AddViewSystem(Contexts contexts) : base(contexts.game)
    {
        _parent = new GameObject("Views").transform;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        => context.CreateCollector(GameMatcher.Asset);

    protected override bool Filter(GameEntity entity) => entity.hasAsset && !entity.hasView;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.AddView(instantiateView(e));
        }
    }

    IView instantiateView(GameEntity entity)
    {
        var prefab = Resources.Load<GameObject>(entity.asset.value);
        var gameObject = Object.Instantiate(prefab, _parent);
       

        if (entity.hasMainPlayer && entity.hasUnit)
        {
            if (entity.mainPlayer.ID == entity.unit.ID)
            {
                gameObject = CreateMainPlayerParent(gameObject);
            }
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
