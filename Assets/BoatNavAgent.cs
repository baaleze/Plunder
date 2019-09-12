using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoatNavAgent : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000)) {
                agent.destination = hit.point;
            }
        }
        if (agent.hasPath) {
            // get angle between the next straight line and direction
            float angleToTurn = Vector3.Angle(agent.steeringTarget - this.transform.position, this.transform.forward);
            if (angleToTurn > 1) {
                float angleSlowRatio = (Mathf.Cos(angleToTurn*Mathf.PI/180) + 1)/2;
                // move forward with current velocity reduced by angle difference
                agent.Move(this.transform.forward * agent.velocity.magnitude * angleSlowRatio);
            }
        }
    }
}
