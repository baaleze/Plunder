using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common
{
    private const float baseCost = 10f;
    private const float idealStock = 200f;
    public const int baseBoatSpeed = 20;
    public enum Resource
    {
        FOOD = 0, WOOD = 1, ORE = 2, CRYSTAL = 3, TOOLS = 4
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
