using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject cityInfoObj;
    public GameObject buyMenuObj;

    Text cityInfo;
    BuyingMenu buyingMenu;
    City selected;
    

    // Start is called before the first frame update
    void Start()
    {
        cityInfo = cityInfoObj.GetComponent<Text>();
        buyingMenu = buyMenuObj.GetComponent<BuyingMenu>();
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
        buyingMenu.SetCity(city);
    }

    // Update is called once per frame
    void Update()
    {
        if (selected != null) {
            updateCityInfo(selected.GenerateCityInfo());
        }
    }
}
