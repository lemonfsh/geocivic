using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public bool mapActive = false;
    [SerializeField] private GameObject SituationUI;

    [SerializeField] private TextMeshProUGUI xText;
    [SerializeField] private TextMeshProUGUI yText;


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
            xText.text = mousePos.x.ToString();
            yText.text = mousePos.y.ToString();
            SituationUI.SetActive(true);

        }
    }


}
