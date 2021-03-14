using System;
using UnityEngine;

/// <summary>
/// Scene Data relevant to TestScene1
/// </summary>
[Serializable]
public class TestScene1Data : SceneData
{
    public Vector3    Box1Position;
    public Quaternion Box1Rotation;
    public Vector3    Box1Scale;

    public Vector3    Box2Position;
    public Quaternion Box2Rotation;
    public Vector3    Box2Scale;

    public Vector3    Box3Position;
    public Quaternion Box3Rotation;
    public Vector3    Box3Scale;
}

/// <summary>
/// Test Scene 1 Controller implementing a scene controller
/// </summary>
public class TestScene1Controller : SceneController
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
        var testData = (TestScene1Data)data;

        var cube1 = GameObject.Find("Cube_1").transform;
        cube1.position =    testData.Box1Position;
        cube1.rotation =    testData.Box1Rotation;
        cube1.localScale =  testData.Box1Scale;

        var cube2 = GameObject.Find("Cube_2").transform;
        cube2.position =    testData.Box2Position;
        cube2.rotation =    testData.Box2Rotation;
        cube2.localScale =  testData.Box2Scale;

        var cube3 = GameObject.Find("Cube_3").transform;
        cube3.position =    testData.Box3Position;
        cube3.rotation =    testData.Box3Rotation;
        cube3.localScale =  testData.Box3Scale;
    }

    /// <inheritdoc/>
    public override SceneData Save()
    {
        TestScene1Data test = new TestScene1Data()
        {
            SceneName = "TestScene_1"
        };

        var cube1 = GameObject.Find("Cube_1").transform;
        test.Box1Position = cube1.position;
        test.Box1Rotation = cube1.rotation;
        test.Box1Scale    = cube1.localScale;

        var cube2 = GameObject.Find("Cube_2").transform;
        test.Box2Position = cube2.position;
        test.Box2Rotation = cube2.rotation;
        test.Box2Scale    = cube2.localScale;

        var cube3 = GameObject.Find("Cube_3").transform;
        test.Box3Position = cube3.position;
        test.Box3Rotation = cube3.rotation;
        test.Box3Scale    = cube3.localScale;

        return test;
    }
}
