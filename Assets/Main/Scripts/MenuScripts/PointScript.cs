using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image pointImage;
    private Button point;
    public int index;
    public Color activeColor;
    public Color inActiveColor;

    private void Start()
    {
        point = transform.gameObject.GetComponent<Button>();
        pointImage = transform.gameObject.GetComponent<Image>();
        if (LevelManager.levels[index].Active)
        {
            pointImage.color = activeColor;
            point.interactable = true;
        }
        else
        {
            pointImage.color = inActiveColor;
            point.interactable = false;
        }
    }

    public void OnClick()
    {
        LevelManager.SelectedLevel = LevelManager.levels[index];
        point.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        Debug.Log(index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(LevelManager.SelectedLevel != LevelManager.levels[index] && point.interactable)
        {
            point.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (LevelManager.SelectedLevel != LevelManager.levels[index] && point.interactable)
        {
            point.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}
