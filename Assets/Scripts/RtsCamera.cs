using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class RtsCamera : MonoBehaviour
{
    public float ScreenEdgeBorderThickness = 5.0f; // distance from screen edge. Used for mouse movement

    public GameObject player;

    [Header("Camera Mode")]
    [Space]
    public bool RTSMode = true;
    public bool FlyCameraMode = false;

    [Header("Movement Speeds")]
    [Space]
    public float minPanSpeed;
    public float maxPanSpeed;
    public float secToMaxSpeed; //seconds taken to reach max speed;
    public float zoomSpeed;


    private float height = 500f;
    private float panSpeed;
    private Vector3 panMovement;
    private float panIncrease = 0.0f;



    // Use this for initialization
    void Start () {
        ReCenter();
	}
	
	
	void Update () {

        # region Camera Mode

        //check that ony one mode is choosen
        if (RTSMode == true) FlyCameraMode = false;
        if (FlyCameraMode == true) RTSMode = false;

        # endregion

        #region Movement

            panMovement = Vector3.zero;

            if (Input.GetKey(KeyCode.Z) || Input.mousePosition.y >= Screen.height - ScreenEdgeBorderThickness)
            {
                panMovement += Vector3.forward * panSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= ScreenEdgeBorderThickness)
            {
                panMovement -= Vector3.forward * panSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.Q) || Input.mousePosition.x <= ScreenEdgeBorderThickness)
            {
                panMovement += Vector3.left * panSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - ScreenEdgeBorderThickness)
            {
                panMovement += Vector3.right * panSpeed * Time.deltaTime;
                //pos.x += panSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                panMovement += Vector3.up * panSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.E))
            {
                panMovement += Vector3.down * panSpeed * Time.deltaTime;
            }

            if(RTSMode) transform.Translate(panMovement, Space.World);
            else if(FlyCameraMode) transform.Translate(panMovement, Space.Self);


        //increase pan speed
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.S) 
            || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q)
            || Input.mousePosition.y >= Screen.height - ScreenEdgeBorderThickness
            || Input.mousePosition.y <= ScreenEdgeBorderThickness
            || Input.mousePosition.x <= ScreenEdgeBorderThickness
            || Input.mousePosition.x >= Screen.width - ScreenEdgeBorderThickness)
        {
            panIncrease += Time.deltaTime / secToMaxSpeed;
            panSpeed = Mathf.Lerp(minPanSpeed, maxPanSpeed, panIncrease);
        }
        else
        {
            panIncrease = 0;
            panSpeed = minPanSpeed;
        }

        #endregion

        #region recenter
        if (Input.GetKey(KeyCode.Space)) {
            this.ReCenter();
        }

        #endregion

    }

    void ReCenter() {
        if (player != null) {
            transform.position = new Vector3(
                player.transform.position.x,
                transform.position.y,
                player.transform.position.z - transform.position.y * Mathf.Tan(Mathf.PI/8));
        }
    }

}
