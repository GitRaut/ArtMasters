using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level
{
    public Scene level;
    private bool isActive;

    public Level(Scene scene, bool condition)
    {
        level = scene;
        isActive = condition;
    }

    public bool Active
    {
        get { return isActive; }
        set { isActive = value; }
    }

    public Scene Scene
    {
        get { return level; }
        set { level = value; }
    }
}
