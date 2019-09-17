using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeDropDown : MonoBehaviour
{
    private Dropdown dropDown;
    // Start is called before the first frame update
    void Awake()
    {
        dropDown = GetComponent<Dropdown>();
        dropDown.options.Clear();
        dropDown.options.Add(new Dropdown.OptionData("Food"));
        dropDown.options.Add(new Dropdown.OptionData("Wood"));
        dropDown.options.Add(new Dropdown.OptionData("Ore"));
        dropDown.options.Add(new Dropdown.OptionData("Crystal"));
        dropDown.options.Add(new Dropdown.OptionData("Tools"));
        
    }

    public Common.Resource GetSelectedResource() {
        return Common.ParseResource(dropDown.options[dropDown.value].text);
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
