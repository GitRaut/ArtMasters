using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int levelIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Hook":
                LevelManager.levels[levelIndex].Active = true;
                transform.gameObject.SetActive(false);
                break;
        }
    }
}
