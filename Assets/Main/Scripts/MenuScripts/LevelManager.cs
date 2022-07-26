using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    /*public static List<Level> levels = new List<Level>()
    {
        { new Level(SceneManager.GetSceneByName("Level_0"), true)},
        { new Level(SceneManager.GetSceneByName("A"), true)},
        { new Level(SceneManager.GetSceneByName("Level_2"), false)}
    };*/

    public static List<Level> levels = new List<Level>()
    {
        { new Level("Level_0", true)},
        { new Level("Level_1", false)},
        { new Level("Level_2", false)}
    };

    /*private static Level selectedLevel = levels[0];

    public static Level SelectedLevel
    {
        get { return selectedLevel; }
        set { selectedLevel = value; }
    }*/
    private static Level selectedLevel = null;

    public static Level SelectedLevel
    {
        get { return selectedLevel; }
        set { selectedLevel = value; }
    }
}
