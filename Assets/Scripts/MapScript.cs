using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public bool mapActive = false;
    [SerializeField] private GameObject SituationUI;


    // Start is called before the first frame update
    void Start()
    {
        mapActive = false;
        SituationUI.SetActive(false);
    }

    private void Update()
    {
        if (!mapActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Debug.Log(mousePos);
            SituationUI.SetActive(true);
        }
    }


}
