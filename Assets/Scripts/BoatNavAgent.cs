using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class BoatNavAgent : MonoBehaviour
{
    NavMeshAgent agent;
    public float v;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();        
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
        if (agent.hasPath) {
            v = agent.velocity.magnitude;
            // get angle between the next straight line and direction
            float angleToTurn = Vector3.SignedAngle(agent.steeringTarget - this.transform.position, this.transform.forward, Vector3.up);
            if (Mathf.Abs(angleToTurn) > 40 && agent.velocity.magnitude < 1) {
                agent.isStopped = true;
                this.transform.Rotate(0, agent.angularSpeed * Time.deltaTime * -Mathf.Sign(angleToTurn), 0);
                agent.Move(this.transform.forward * (agent.speed * Time.deltaTime / 10));
            } else {
                agent.isStopped = false;
            }
        }
    }
}
