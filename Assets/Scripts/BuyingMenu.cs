using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class BuyingMenu : MonoBehaviour
{
    
    public GameObject buyButtonObj;
    Button buyButton;
    public GameObject buyListObj;
    TradeDropDown buyList;
    Dropdown buyDropdown;
    public GameObject player;
    private Boat boat;
    public GameObject buySliderObj;
    SlideNumber buySlider;

    public GameObject sellButtonObj;
    Button sellButton;
    public GameObject sellListObj;
    TradeDropDown sellList;
    Dropdown sellDropdown;
    public GameObject sellSliderObj;
    SlideNumber sellSlider;

    private City city;
    private UnityAction tickCallback;

    
    // Start is called before the first frame update
    void Start()
    {
        tickCallback = () => UpdateSliders(false);
        // BUY
        buyButton = buyButtonObj.GetComponent<Button>();
        buyList = buyListObj.GetComponent<TradeDropDown>();
        boat = player.GetComponent<Boat>();
        buySlider = buySliderObj.GetComponent<SlideNumber>();
        buyDropdown = buyList.GetComponent<Dropdown>();
        buyDropdown.onValueChanged.AddListener(newValue => {
            buySlider.UpdateCost(
                city.GetCost(Common.ParseResource(buyDropdown.options[newValue].text)),
                boat.gold,
                true);
            buySlider.value = 0;
        });
        buyButton.onClick.AddListener(() => {
            boat.BuyFromCityInRange(buyList.GetSelectedResource(), (int) buySlider.value);
            buySlider.value = 0;
        });

        // SELL
        sellButton = sellButtonObj.GetComponent<Button>();
        sellList = sellListObj.GetComponent<TradeDropDown>();
        boat = player.GetComponent<Boat>();
        sellSlider = sellSliderObj.GetComponent<SlideNumber>();
        sellDropdown = sellList.GetComponent<Dropdown>();
        sellDropdown.onValueChanged.AddListener(newValue => {
            sellSlider.UpdateCost(
                city.GetCost(Common.ParseResource(sellDropdown.options[newValue].text)),
                boat.GetStock(Common.ParseResource(sellDropdown.options[newValue].text)),
                false);
            sellSlider.value = 0;
        });
        sellButton.onClick.AddListener(() => {
            boat.SellFromCityInRange(sellList.GetSelectedResource(), (int) sellSlider.value);
            sellSlider.value = 0;
        });

        // begin as inactive
        gameObject.SetActive(false);
    }

    public void SetCity(City c) {
        if (c != null) {
            city = c;
            c.ListenToResourceTick(tickCallback);
            gameObject.SetActive(true);
            // select first resource as default
            UpdateSliders(true);
        } else {
            city.StopListeningToResourceTick(tickCallback);
            gameObject.SetActive(false);
            city = null;
        }
    }

    private void UpdateSliders(bool reset) {
        buySlider.UpdateCost(city.GetCost(Common.ParseResource(buyDropdown.options[buyDropdown.value].text)), boat.gold, true);
        sellSlider.UpdateCost(
            city.GetCost(Common.ParseResource(sellDropdown.options[sellDropdown.value].text)),
            boat.GetStock(Common.ParseResource(sellDropdown.options[sellDropdown.value].text)),
            false);
        if (reset) {
            buySlider.value = 0;
            sellSlider.value = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
