

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    
    public static List<City> allCities = new List<City>();
    private static string[] possibleNames = {"Port Royal", "La Barbade", "Grenade", "Port Louis", "L'ile de la Tortue"};
    private const int rate = 1;
    

    private UiManager uiManager;
    private string cityName;

    public bool producesCrystal;
    public bool producesWood;
    public bool producesFood;
    public bool producesOre;
    public bool producesTools;
    public int gold;
    private int[] stock = new int[5];

    // Start is called befstock[(int) Common.Resource.ORE] the first frame update
    void Awake()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        cityName = possibleNames[Random.Range(0, possibleNames.Length)];
        gold = Random.Range(5,10) * 100;
        for (int i = 0; i < stock.Length; i++)
        {
            stock[i] = Random.Range(0,5) * 100;
        }
        allCities.Add(this);
    }

    public string GetName() {
        return cityName;
    }

    
    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            uiManager.SetSelectedCity(this);
        }
    }

    // resolve production and consommation
    private void TickResources() {
        // check needs and if ok produce
        if (producesFood && stock[(int) Common.Resource.CRYSTAL] - rate > 0 && stock[(int) Common.Resource.TOOLS] - rate > 0) {
            stock[(int) Common.Resource.CRYSTAL] -= rate;
            stock[(int) Common.Resource.TOOLS] -= rate;
            stock[(int) Common.Resource.FOOD] += rate*6;
        }
        if (producesOre && stock[(int) Common.Resource.WOOD] - rate > 0 && stock[(int) Common.Resource.FOOD] - rate > 0) {
            stock[(int) Common.Resource.WOOD] -= rate;
            stock[(int) Common.Resource.FOOD] -= rate;
            stock[(int) Common.Resource.ORE] += rate*6;
        }
        if (producesWood && stock[(int) Common.Resource.FOOD] - rate > 0 && stock[(int) Common.Resource.TOOLS] - rate > 0) {
            stock[(int) Common.Resource.FOOD] -= rate;
            stock[(int) Common.Resource.TOOLS] -= rate;
            stock[(int) Common.Resource.WOOD] += rate*6;
        }
        if (producesTools && stock[(int) Common.Resource.WOOD] - rate > 0 && stock[(int) Common.Resource.ORE] - rate > 0 && stock[(int) Common.Resource.CRYSTAL] - rate > 0) {
            stock[(int) Common.Resource.WOOD] -= rate;
            stock[(int) Common.Resource.ORE] -= rate;
            stock[(int) Common.Resource.CRYSTAL] -= rate;
            stock[(int) Common.Resource.TOOLS] += rate*6;
        }
        if (producesCrystal && stock[(int) Common.Resource.TOOLS] - rate > 0 && stock[(int) Common.Resource.ORE] - rate > 0) {
            stock[(int) Common.Resource.TOOLS] -= rate;
            stock[(int) Common.Resource.ORE] -= rate;
            stock[(int) Common.Resource.CRYSTAL] += rate*6;
        }
    }

    internal void AddResource(Common.Resource resource, int v)
    {
        stock[(int) resource] += v;
    }

    internal int GetStock(Common.Resource resource)
    {
        return stock[(int) resource];
    }

    public int GetCost(Common.Resource r) {
        return Common.GetCostFromStock(stock[(int) r]);
    }

    

    public string GenerateCityInfo()
    {
        return cityName 
        + "\r\nGold: " + gold
        + "\r\nCrystals: " + stock[(int) Common.Resource.CRYSTAL]  + " (" + GetCost(Common.Resource.CRYSTAL) + " each)"
        + "\r\nWood: " + stock[(int) Common.Resource.WOOD]  + " (" + GetCost(Common.Resource.WOOD) + " each)"
        + "\r\nFood: " + stock[(int) Common.Resource.FOOD]  + " (" + GetCost(Common.Resource.FOOD) + " each)"
        + "\r\nOre: " + stock[(int) Common.Resource.ORE] + " (" + GetCost(Common.Resource.ORE) + " each)"
        + "\r\nTools: " + stock[(int) Common.Resource.TOOLS] + " (" + GetCost(Common.Resource.TOOLS) + " each)";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
