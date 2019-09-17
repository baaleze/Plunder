using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideNumber : MonoBehaviour
{
    public GameObject text;
    private Slider slider;
    private int cost;
    private int goldAvailable;
    public int value{
        get {return (int)slider.value;}
        set {slider.value = value;}
    }
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateCost(int newCost, int newGoldAvailable) {
        cost = newCost;
        goldAvailable = newGoldAvailable;
        slider.maxValue = Mathf.FloorToInt((float)newGoldAvailable / newCost);
    }

    // Update is called once per frame
    void Update()
    {
        text.GetComponent<Text>().text = slider.value.ToString("n0") + " for " + (slider.value * cost) + " gold";
    }
}
