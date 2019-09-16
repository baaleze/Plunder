using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatStatus : MonoBehaviour
{
    public GameObject player;
    private Boat boat;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        boat = player.GetComponent<Boat>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = boat.gold + " gold | " 
            + boat.GetStock(Common.Resource.FOOD) + " food " 
            + boat.GetStock(Common.Resource.WOOD) + " wood\r\n" 
            + boat.GetStock(Common.Resource.ORE) + " ore " 
            + boat.GetStock(Common.Resource.CRYSTAL) + " crystals " 
            + boat.GetStock(Common.Resource.TOOLS) + " tools"; 
    }
}
