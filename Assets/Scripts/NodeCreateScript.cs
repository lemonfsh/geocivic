using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreateScript : MonoBehaviour
{
    [SerializeField] private GameObject NodeCreateUI;

    public void ConfirmButton()
    {
        NodeCreateUI.SetActive(false);
    }

    public void CancelButton()
    {
        NodeCreateUI.SetActive(false);
    }
}
