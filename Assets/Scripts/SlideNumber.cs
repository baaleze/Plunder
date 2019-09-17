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

    public void UpdateCost(int newCost, int available, bool buying) {
        cost = newCost;
        if (buying) {
            // can only buy what i have / what it costs
            slider.maxValue = Mathf.FloorToInt((float)available / newCost);
        } else {
            // can only sell what I have
            slider.maxValue = available;
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.GetComponent<Text>().text = slider.value.ToString("n0") + " for " + (slider.value * cost) + " gold";
    }
}
