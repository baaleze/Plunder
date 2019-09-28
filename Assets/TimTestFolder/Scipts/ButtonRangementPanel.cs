using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRangementPanel : MonoBehaviour
{
    public Vector3 PanelRangePosition = new Vector3(10,0,0);
    public Vector3 PanelOutPosition = new Vector3(200, 0, 0);

    public GameObject Panel;
    public bool PannelRange = false;

    public void RangePanel()
    {
        if(PannelRange == false)
        {
            PannelRange = true;
            Panel.GetComponent<RectTransform>().localPosition = PanelRangePosition;
            transform.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, -90);
        }
        else
        {
            PannelRange = false;
            Panel.GetComponent<RectTransform>().localPosition = PanelOutPosition;
            transform.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 90);
        }
    }
}
