using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level
{
    public string name;
    public bool isActive;

    public Level(string name, bool condition)
    {
        this.name = name;
        isActive = condition;
    }

    public bool Active
    {
        get { return isActive; }
        set { isActive = value; }
    }

    public string LevelName
    {
        get { return name; }
        set { name = value; }
    }
    /*public Scene level;
    public bool isActive;

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
    }*/
}
