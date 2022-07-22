using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{
    private static bool isOpen;
    private static int levelPoint;

    public static bool Open
    {
        get { return isOpen; }
        set { isOpen = value; }
    }

    public static int Level
    {
        get { return levelPoint; }
        set { levelPoint = value; }
    }
}
