using Entitas;
using UnityEngine;

public interface IView
{
    void Link(IEntity entity);
    GameObject GetObj();
}
