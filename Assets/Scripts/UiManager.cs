using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    Text cityInfo;
    TradeDropDown buyDropdown;
    TradeDropDown sellDropdown;
    City selected;
    

    // Start is called before the first frame update
    void Start()
    {
        cityInfo = GameObject.Find("CityInfo").GetComponent<Text>();
        buyDropdown = GameObject.Find("BuyList").GetComponent<TradeDropDown>();
        sellDropdown = GameObject.Find("SellList").GetComponent<TradeDropDown>();
    }

    public void updateCityInfo(string text) {
        cityInfo.text = text;
    }

    public void SetSelectedCity(City c) {
        if (selected == c) {
            selected = null;
        } else {
            selected = c;
        }
    }

    public void updateCityInRange(City city) {
        // TODO make buy / sale menu
        buyDropdown.SetCity(city);
        sellDropdown.SetCity(city);
    }

    // Update is called once per frame
    void Update()
    {
        if (selected != null) {
            updateCityInfo(selected.GenerateCityInfo());
        }
    }
}
