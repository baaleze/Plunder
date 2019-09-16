using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeDropDown : MonoBehaviour
{
    public GameObject buySliderObj;
    SlideNumber buySlider;
    private City city;
    public GameObject playerBoat;
    private Boat boat;
    private Dropdown dropDown;
    // Start is called before the first frame update
    void Start()
    {
        buySlider = buySliderObj.GetComponent<SlideNumber>();
        boat = playerBoat.GetComponent<Boat>();
        dropDown = GetComponent<Dropdown>();
        dropDown.options.Clear();
        dropDown.options.Add(new Dropdown.OptionData("Food"));
        dropDown.options.Add(new Dropdown.OptionData("Wood"));
        dropDown.options.Add(new Dropdown.OptionData("Ore"));
        dropDown.options.Add(new Dropdown.OptionData("Crystal"));
        dropDown.options.Add(new Dropdown.OptionData("Tools"));
        dropDown.onValueChanged.AddListener(newValue => {
            buySlider.UpdateCost(city.GetCost(Common.ParseResource(dropDown.options[newValue].text)), boat.gold);
        });
    }

    public Common.Resource GetSelectedResource() {
        return Common.ParseResource(dropDown.options[dropDown.value].text);
    }

    public void SetCity(City c) {
        city = c;
        buySlider.UpdateCost(city.GetCost(Common.ParseResource(dropDown.options[dropDown.value].text)), boat.gold);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
