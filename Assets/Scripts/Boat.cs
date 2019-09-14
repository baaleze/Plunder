using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{

    public List<City> citiesInRange = new List<City>();
    public Canvas canvas;
    private UiManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = canvas.GetComponent<UiManager>();
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
        if (!citiesInRange.Contains(city)) {
            citiesInRange.Add(city);
            uiManager.updateCityInRange(city.cityName);
        }
    }

    public void removeCityInRange(City city) {
        if (citiesInRange.Contains(city)) {
            citiesInRange.Remove(city);
            uiManager.updateCityInRange("");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
