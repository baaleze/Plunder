using UnityEngine;
using System.Collections.Generic;

public class Faction  {

    private static int nextId = 0;
    public static List<Faction> allFactions = new List<Faction>() {
        new Faction(Color.red, "Red"),
        new Faction(Color.blue, "Blue"),
        new Faction(Color.green, "Green"),
    };

    private Color color;
    public readonly string name;
    public readonly int id;
    
    public Faction(Color c, string n) {
        id = nextId++;
        color = c;
        name = n;
    }
}