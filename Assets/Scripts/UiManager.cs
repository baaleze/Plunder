using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject cityInfoObj;
    public GameObject buyMenuObj;

    Text cityInfo;
    BuyingMenu buyingMenu;
    City selected;
    int windDir = 0;
    float timeSinceWindChange = 0;
    private const float windChangeTime = 5;
    
    public int Wind {
        get {return windDir;}
        set {windDir = value;}
    }
    public GameObject compass;
    public GameObject compassTextObj;
    private Text compassText;
    

    // Start is called before the first frame update
    void Start()
    {
        
        cityInfo = cityInfoObj.GetComponent<Text>();
        buyingMenu = buyMenuObj.GetComponent<BuyingMenu>();
        compassText = compassTextObj.GetComponent<Text>();

        windDir = Random.Range(0,359);
        compass.transform.Rotate(0,0, -windDir);
        UpdateWind();
    }

    private void UpdateWind() {
        int diff = Random.Range(5, 25) * (int) Mathf.Sign(Random.Range(-1,1));
        windDir += diff;
        compass.transform.Rotate(0,0, -diff);
        compassText.text = windDir.ToString("N0");
    }

    public void updateCityInfo(string text) {
        cityInfo.text = text;
    }

    public void SetSelectedCity(City c) {
        if (selected == c) {
            selected = null;
        } else {
            selected = c;
        }
    }

    public void updateCityInRange(City city) {
        // TODO make buy / sale menu
        buyingMenu.SetCity(city);
    }

    // Update is called once per frame
    void Update()
    {
        if (selected != null) {
            updateCityInfo(selected.GenerateCityInfo());
        }
        timeSinceWindChange += Time.deltaTime;
        if (timeSinceWindChange > windChangeTime) {
            timeSinceWindChange -= windChangeTime;
            UpdateWind();
        }
    }
}
