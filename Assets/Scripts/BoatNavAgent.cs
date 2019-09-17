using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class BoatNavAgent : MonoBehaviour
{
    NavMeshAgent agent;
    public Canvas canvas;
    private static UiManager uiManager;

    void Awake() {
        if (uiManager == null){
            uiManager = canvas.GetComponent<UiManager>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    private float WindFactor() {
        int dir = 180 - (int)Vector3.SignedAngle(this.transform.forward, Vector3.back, Vector3.up);
        int angleDiff = Mathf.Abs(uiManager.Wind - dir) % 360;
        angleDiff = angleDiff > 180 ? 360 - angleDiff : angleDiff;
        return -angleDiff*0.014f + 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000)) {
                agent.destination = hit.point;
            }
        }
        // update agent speed depending on wind
        agent.speed = Common.baseBoatSpeed * WindFactor();
        if (agent.hasPath) {
            // get angle between the next straight line and direction
            float angleToTurn = Vector3.SignedAngle(agent.steeringTarget - this.transform.position, this.transform.forward, Vector3.up);
            if (Mathf.Abs(angleToTurn) > 40 && agent.velocity.magnitude < 1) {
                agent.isStopped = true;
                this.transform.Rotate(0, agent.angularSpeed * Time.deltaTime * -Mathf.Sign(angleToTurn), 0);
                agent.Move(this.transform.forward * (agent.speed * Time.deltaTime / 5));
            } else {
                agent.isStopped = false;
            }
        }
    }
}
