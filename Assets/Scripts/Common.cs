using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common
{
    private const float baseCost = 10f;
    private const float idealStock = 100f;
    public enum Resource
    {
        FOOD = 0, WOOD = 1, ORE = 2, CRYSTAL = 3, TOOLS = 4
    }
    

    public static int GetCostFromStock(int currentStock) {
        float multiplier = 1f;
        if (currentStock > idealStock && currentStock < idealStock * 2) {
            multiplier = -(currentStock * 2f)/idealStock + 3 ;
        } else if (currentStock > idealStock && currentStock >= idealStock * 2) {
            multiplier = 0.25f;
        } else {
            multiplier = -(currentStock * 0.75f)/idealStock + 3 ;
        }
        return Mathf.RoundToInt(multiplier * baseCost);
    }
}
