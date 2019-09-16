using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public Canvas canvas;
    public City cityInRange;
    private UiManager uiManager;
    private int gold;
    private int[] stock = new int[5];

    // Start is called before the first frame update
    void Start()
    {
        uiManager = canvas.GetComponent<UiManager>();
        gold = 2000;
    }

    void OnTriggerEnter(Collider other) {
        if (other.name == "Range") {
            addCityInRange(other.GetComponentInParent<City>());
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.name == "Range") {
            removeCityInRange(other.GetComponentInParent<City>());
        }
    }

    public void addCityInRange(City city) {
        if (cityInRange == null) {
            cityInRange = city;
            uiManager?.updateCityInRange(city.GetName());
        }
    }

    public void removeCityInRange(City city) {
        if (city == cityInRange) {
            cityInRange = null;
            uiManager?.updateCityInRange("");
        }
    }

    public string BuyFromCityInRange(Common.Resource resource, int quantity) {
        if (cityInRange == null){
            return "NO CITY IN RANGE";
        }
        int cost = cityInRange.GetCost(resource) * quantity;
        if (gold < cost) {
            return "NOT ENOUGH GOLD";
        } else if (cityInRange.GetStock(resource) < quantity) {
            return "NOT ENOUGH RESOURCE";
        } else {
            // OK
            gold -= cost;
            cityInRange.gold += cost;
            cityInRange.AddResource(resource, -quantity);
            stock[(int) resource] += quantity;
            return "OK";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
