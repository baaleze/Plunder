using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    Text cityInfo;
    Text citiesInRange;

    // Start is called before the first frame update
    void Start()
    {
        cityInfo = GameObject.Find("CityInfo").GetComponent<Text>();
        citiesInRange = GameObject.Find("CityInRange").GetComponent<Text>();
    }

    public void updateCityInfo(string text) {
        cityInfo.text = text;
    }

    public void updateCityInRange(string cityName) {
        if (cityName != "") {
            citiesInRange.text = "City in range: " + cityName;
        } else {
            citiesInRange.text = "No city in range";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
