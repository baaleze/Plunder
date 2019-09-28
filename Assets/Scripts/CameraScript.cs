using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {


   public  static CameraScript camhandler;

	private static readonly float PanSpeed = -400f;
    public static readonly float RotateSpeed = -300;
	private static readonly float ZoomSpeedTouch = 0.1f;
	private static readonly float ZoomSpeedMouse = 100f;


	public static readonly float[] BoundsX = new float[]{-1000, 5000};
	public static readonly float[] BoundsZ = new float[]{-1000, 5000};
	public static readonly float[] ZoomBounds = new float[]{1000, 6000};

	private Camera cam;

	public bool panActive;
	private Vector3 lastPanPosition;
	private int panFingerId; // Touch mode only

    public bool rotateActive;


	private bool zoomActive;
	private Vector2[] lastZoomPositions; // Touch mode only

	void Awake() {
        camhandler = this;
		cam = GetComponent<Camera>();

		#if UNITY_ANDROID || UNITY_IOS 
		cam.fieldOfView = 60f;
		#endif
	}

	void Update() {
		// If there's an open menu, or the clicker is being pressed, ignore the touch.
		/*if (GameManager.Instance.MenuManager.HasOpenMenu || GameManager.Instance.BitSpawnManager.IsSpawningBits) {
			return;
		}*/

		
			if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer) {
				HandleTouch ();
			} else {
				HandleMouse ();
			}
		
	}

	void HandleTouch() {
		switch(Input.touchCount) {

		case 1: // Panning
			zoomActive = false;

			// If the touch began, capture its position and its finger ID.
			// Otherwise, if the finger ID of the touch doesn't match, skip it.
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				lastPanPosition = touch.position;
				panFingerId = touch.fingerId;
				panActive = true;
			} else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved) {
				PanCamera(touch.position);
			}
			break;

		case 2: // Zooming
			panActive = false;

			Vector2[] newPositions = new Vector2[]{Input.GetTouch(0).position, Input.GetTouch(1).position};
			if (!zoomActive) {
				lastZoomPositions = newPositions;
				zoomActive = true;
			} else {
				// Zoom based on the distance between the new positions compared to the 
				// distance between the previous positions.
				float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
				float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
				float offset = newDistance - oldDistance;

				ZoomCamera(offset, ZoomSpeedTouch);

				lastZoomPositions = newPositions;
			}
			break;

		default:
			panActive = false;
			zoomActive = false;
			break;
		}
	}

	void HandleMouse() {
        // On mouse down, capture it's position.
        // On mouse up, disable panning.
        // If there is no mouse being pressed, do nothing.

       // if (Master.mastersing.isTuilePanelActive == false)
        {
            if (Input.GetMouseButtonDown(0))
            {

                lastPanPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                panActive = false;
            }
            else if (Input.GetMouseButton(0))
            {
                if ((lastPanPosition - Input.mousePosition).magnitude > 1)
                {
                    panActive = true;
                    PanCamera(Input.mousePosition);
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            rotateActive = true;
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rotateActive = false;
        }
        else if (Input.GetMouseButton(1))
        {
            RotateCam(Input.mousePosition);
        
    }

		// Check for scrolling to zoom the camera
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		zoomActive = true;
		ZoomCamera(scroll, ZoomSpeedMouse);
		zoomActive = false;
	}

    public Transform TransCamY;
	void PanCamera(Vector3 newPanPosition) {
		if (!panActive) {
			return;
		}

		// Translate the camera position based on the new input position
		Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 camyrot = new Vector3(0, transform.rotation.y, 0);
		Vector3 move =  new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);

        TransCamY.rotation.eulerAngles.Set(0, transform.rotation.eulerAngles.y, 0);
        TransCamY.position = transform.position;
		transform.Translate(-move, TransCamY);  
		ClampToBounds();

		lastPanPosition = newPanPosition;
	}

    void RotateCam(Vector3 newPanPosition)
    {
        if (!rotateActive)
        {
            return;
        }

        // Translate the camera position based on the new input position
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(0, offset.x * RotateSpeed, 0);
        transform.Rotate(-move, Space.World);
        ClampToBounds();

        lastPanPosition = newPanPosition;
    }
    private Vector3 TempVect3;

    void ZoomCamera(float offset, float speed) {
		if (!zoomActive || offset == 0) {
			return;
		}

        if (transform.position.y < 1200 && offset < 0 || transform.position.y > 2 && offset > 0)
        {

            TempVect3.Set(transform.position.x, transform.position.y + offset * speed, transform.position.z);
            //cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);

            transform.Translate(Vector3.forward * offset * speed, Space.Self);

            //transform.position = TempVect3;
        }
	}

	void ClampToBounds() {
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
		pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);

		transform.position = pos;
	}
}