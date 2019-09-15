
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    
    public static List<City> allCities = new List<City>();
    private UiManager uiManager;

    private string[] possibleNames = {"Port Royal", "La Barbade", "Grenade", "Port Louis", "L'ile de la Tortue"};
    private string cityName;
    private int gold;
    private int spices;
    private int silk;
    private int food;
    private int ore;

    // Start is called before the first frame update
    void Awake()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        cityName = possibleNames[Random.Range(0, possibleNames.Length)];
        gold = Random.Range(0,5) * 100;
        spices = Random.Range(0,5) * 100;
        silk = Random.Range(0,5) * 100;
        food = Random.Range(0,5) * 100;
        ore = Random.Range(0,5) * 100;
        allCities.Add(this);
    }

    public string GetName() {
        return cityName;
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
