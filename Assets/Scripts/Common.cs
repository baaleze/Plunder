using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common
{
    private const float baseCost = 10f;
    private const float idealStock = 200f;
    public const int baseBoatSpeed = 20;

    public const int WEALTH_0 = 0;
    public const int WEALTH_1 = 10;
    public const int WEALTH_2 = 20;
    public const int WEALTH_3 = 50;
    public const int MAX_BOATS_BY_CITY = 5;
    public enum Resource
    {
        NONE = 0, FOOD = 1, WOOD = 2, ORE = 3, CRYSTAL = 4, TOOLS = 5
    }
    public static int ResId(Common.Resource r) {
        return ((int)r)-1;
    }
    ///<summary>Returns true <c>percentage</c>% of the time.</summary>
    public static bool Chance(int percentage) {
        return Random.Range(0,100) < percentage;
    }

    public enum Purpose {
        TRADE, PATROL, INTERCEPT, PIRATE
    }

    public static Resource ParseResource(string s) {
        switch(s.ToUpper()) {
            case "FOOD":
                return Resource.FOOD;
            case "CRYSTAL":
                return Resource.CRYSTAL;
            case "WOOD":
                return Resource.WOOD;
            case "ORE":
                return Resource.ORE;
            case "TOOLS":
                return Resource.TOOLS;
            default:
                throw new System.Exception("Incorrect value for Common.Resource : " +s);
        }
    }
    

    public static int GetCostFromStock(int currentStock) {
        float multiplier = 1f;
        if (currentStock > idealStock && currentStock < idealStock * 2) {
            multiplier = -((float)currentStock * 0.75f)/idealStock + 3f ;
        } else if (currentStock > idealStock && currentStock >= idealStock * 2) {
            multiplier = 0.25f;
        } else {
            multiplier = -((float)currentStock * 2f)/idealStock + 3f ;
        }
        return Mathf.RoundToInt(multiplier * baseCost);
    }
}
