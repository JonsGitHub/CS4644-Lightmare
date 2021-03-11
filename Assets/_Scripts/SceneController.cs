using System;
using UnityEngine;

public class SceneController : MonoBehaviour, ISceneController
{
    protected void Awake()
    {
        tag = "SceneController";
    }

    public virtual void Load(object data)
    {
        throw new System.NotImplementedException();
    }

    public virtual SceneData Save()
    {
        throw new System.NotImplementedException();
    }
}

public interface ISceneController
{
    SceneData Save();

    void Load(object data);
}
