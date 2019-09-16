using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuyingMenu : MonoBehaviour
{
    
    public GameObject buyButtonObj;
    Button buyButton;
    public GameObject buyListObj;
    TradeDropDown buyList;
    public GameObject player;
    private Boat boat;
    public GameObject buySliderObj;
    Slider buySlider;

    
    // Start is called before the first frame update
    void Start()
    {
        buyButton = buyButtonObj.GetComponent<Button>();
        buyList = buyListObj.GetComponent<TradeDropDown>();
        boat = player.GetComponent<Boat>();
        buySlider = buySliderObj.GetComponent<Slider>();
        buyButton.onClick.AddListener(() => {
            boat.BuyFromCityInRange(buyList.GetSelectedResource(), (int) buySlider.value);
            buySlider.value = 0;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
