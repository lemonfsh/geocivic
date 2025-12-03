using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeCreateScript : MonoBehaviour
{
    [SerializeField] private GameObject NodeCreateUI;

    public Image situationImage;

    public TMP_Dropdown dropDown;

    [SerializeField] private Sprite[] starImages;


    public void ConfirmButton()
    {
        NodeCreateUI.SetActive(false);
    }

    public void CancelButton()
    {
        NodeCreateUI.SetActive(false);
    }

    public void ChangeSituationImage()
    {
        situationImage.sprite = starImages[dropDown.value];
    }
}
