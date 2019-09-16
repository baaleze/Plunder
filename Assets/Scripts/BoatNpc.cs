using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatNpc : Boat
{
    City city1;
    City city2;
    City currentTarget;
    BoatNpcNavAgent navAgent;
    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<BoatNpcNavAgent>();
        // choose 2 cities
        city1 = City.allCities[Random.Range(0, City.allCities.Count)];
        while(city2 == null) {
            City c = City.allCities[Random.Range(0, City.allCities.Count)];
            if (c != city1) {
                city2 = c;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null) {
            currentTarget = city1;
            navAgent.GoToCity(city1);
        }
        if (cityInRange != null) {
            // a city is in range, swap!
            if (cityInRange == city1 && currentTarget == city1) {
                currentTarget = city2;
                navAgent.GoToCity(city2);
            } else if (cityInRange == city2 && currentTarget == city2){
                currentTarget = city1;
                navAgent.GoToCity(city1);
            }
        }
    }
}
