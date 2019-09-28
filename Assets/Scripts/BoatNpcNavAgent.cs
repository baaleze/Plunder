using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoatNpcNavAgent : MonoBehaviour
{
    NavMeshAgent agent;
    
    private static UiManager uiManager;
    void Awake() {
        if (uiManager == null){
            uiManager = GameObject.Find("UiCanvas").GetComponent<UiManager>();
        }
        agent = GetComponent<NavMeshAgent>();        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GoToCity(City c) {
        // find nearest point on nav mesh
        NavMeshHit hit;
        NavMesh.SamplePosition(c.gameObject.transform.position, out hit, 4, -1);
        agent.destination = hit.position;
    }

    public void GoTo(Vector3 pos) {
        agent.destination = pos;
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
        agent.speed = Common.baseBoatSpeed * WindFactor();
        if (agent.hasPath) {
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
