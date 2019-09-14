using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public Canvas canvas;
    private UiManager uiManager;

    public string cityName;
    public int gold;
    public int spices;
    public int silk;
    public int food;
    public int ore;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = canvas.GetComponent<UiManager>();
    }

    
    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            uiManager.updateCityInfo(GenerateCityInfo());
        }
    }

    

    private string GenerateCityInfo()
    {
        return cityName + "\r\nGold: " + gold + "\r\nSpices: " + spices + "\r\nSilk: " + silk + "\r\nFood: " + food + "\r\nOre: " + ore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
