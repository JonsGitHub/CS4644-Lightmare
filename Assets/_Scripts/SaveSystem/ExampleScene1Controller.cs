using System;
using UnityEngine;

/// <summary>
/// Scene Data relevant to TestScene1
/// </summary>
[Serializable]
public class ExampleScene1Data : SceneData
{
    public Vector3 Box1Position;
    public Quaternion Box1Rotation;
    public Vector3 Box1Scale;
}

class ExampleScene1Controller : SceneController
{
    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    new void Awake()
    {
        base.Awake();
    }

    /// <inheritdoc/>
    public override void Load(object data)
    {
        if (data == null)
            return;

        var testData = (ExampleScene1Data)data;

        var cube1 = GameObject.Find("Cube_1").transform;
        cube1.position = testData.Box1Position;
        cube1.rotation = testData.Box1Rotation;
        cube1.localScale = testData.Box1Scale;
    }

    /// <inheritdoc/>
    public override SceneData Save()
    {
        ExampleScene1Data test = new ExampleScene1Data()
        {
            SceneName = "ExampleScene_1"
        };

        var cube1 = GameObject.Find("Cube_1").transform;
        test.Box1Position = cube1.position;
        test.Box1Rotation = cube1.rotation;
        test.Box1Scale = cube1.localScale;
        return test;
    }

    public override bool SavePosition() => true;
}