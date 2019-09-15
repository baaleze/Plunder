using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoatNpcNavAgent : MonoBehaviour
{
    NavMeshAgent agent;
    public float v;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();        
    }

    public void GoToCity(City c) {
        // find nearest point on nav mesh
        NavMeshHit hit;
        NavMesh.SamplePosition(c.gameObject.transform.position, out hit, 4, -1);
        agent.destination = hit.position;
    }

    // Update is called once per frame
    void Update()
    {
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
