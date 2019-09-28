using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class BoatNavAgent : MonoBehaviour
{
    private Boat boat;
    NavMeshAgent agent;
    public bool useAINav;
    private static UiManager uiManager;
    private float timeUntilResettingCourse = -1;
    private Vector3 newDir;
    private Vector3 storedDestination;
    private List<Vector3> wayPoints = new List<Vector3>();
    private List<GameObject> wayPointMarkers = new List<GameObject>();
    public GameObject markersPrefab;
    private const int FIRING_DIST = 80;
    private const int FIRING_DIST_GO_NEAR_AGAIN = 120;
    private const float MIN_REMAINING_DIST = 6;

    public GameObject debugPillar;

    void Awake() {
        if (uiManager == null){
            uiManager = GameObject.Find("UiCanvas").GetComponent<UiManager>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        boat = GetComponentInParent<Boat>();
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
        // BOAT TARGETING
        // Do I have a boat as target (and not on an alternative path) ?
        if (boat.target != null && timeUntilResettingCourse < 0 && useAINav) {
            // check range
            float dist = Vector3.Distance(transform.position, boat.target.transform.position);
            if (dist > (boat.firing ? FIRING_DIST_GO_NEAR_AGAIN : FIRING_DIST)) {
                // go nearer
                agent.destination = boat.target.transform.position;
                // stop firing
                boat.firing = false;
            } else {
                // near enough
                boat.firing = true;
                // go 90° to fire
                NavMeshHit actualPoint;
                NavMesh.SamplePosition(
                    transform.position + (Quaternion.AngleAxis(90, Vector3.up) * (boat.target.transform.position - transform.position)).normalized * agent.speed,
                    out actualPoint, 2, -1);
                agent.destination = actualPoint.position;
            }
        }

        // RIGHT CLICK TO MOVE
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000)) {
                // WAYPOINTS
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl)) {
                    // add a new waypoint
                    Debug.Log("New waypoint");
                    wayPoints.Add(hit.point);
                    wayPointMarkers.Add(GameObject.Instantiate(markersPrefab, hit.point, Quaternion.identity));
                    if (wayPoints.Count == 1) { // first one ?
                        agent.destination = hit.point;
                    }
                } else {
                    Debug.Log("Clear waypoints");
                    // clear all
                    wayPoints.Clear();
                    wayPointMarkers.ForEach(o => GameObject.Destroy(o));
                    wayPointMarkers.Clear();
                    // add a new waypoint
                    wayPoints.Add(hit.point);
                    wayPointMarkers.Add(GameObject.Instantiate(markersPrefab, hit.point, Quaternion.identity));
                    agent.destination = hit.point;
                }
            }
        }

        // Check to see if we're near of a waypoint and there remaining waypoints after that
        if (agent.remainingDistance < MIN_REMAINING_DIST && wayPoints.Count > 1) {
            // remove current one
            wayPoints.RemoveAt(0);
            GameObject.Destroy(wayPointMarkers[0]);
            wayPointMarkers.RemoveAt(0);
            // next!
            agent.destination = wayPoints[0];
        }


        // update agent speed depending on wind
        agent.speed = Common.baseBoatSpeed * WindFactor();

        // ZIGZAG WHEN FACING WIND
        // check if we're on alternate course
        if (timeUntilResettingCourse > 0){
            timeUntilResettingCourse -= Time.deltaTime;
            // time to get back on original course
            if (timeUntilResettingCourse < 0) {
                agent.destination = storedDestination;
            } else {
                // go in the newDir direction
                // try to find a point on the navmesh
                NavMeshHit actualPoint;
                NavMesh.SamplePosition(this.transform.position + newDir, out actualPoint, 5, -1);
                agent.destination = actualPoint.position;
                debugPillar.transform.position = actualPoint.position;
            }
        }

        // TURNING AND WIND
        if (agent.hasPath) {
            // get angle between the next straight line and direction
            float angleToTurn = Vector3.SignedAngle(agent.steeringTarget - this.transform.position, this.transform.forward, Vector3.up);
            if (Mathf.Abs(angleToTurn) > 60 && agent.velocity.magnitude < 1) {
                agent.isStopped = true;
                this.transform.Rotate(0, agent.angularSpeed * Time.deltaTime * -Mathf.Sign(angleToTurn), 0);
                agent.Move(this.transform.forward * (agent.speed * Time.deltaTime / 5));
            } else {
                agent.isStopped = false;
                // are we already on alternate course AND using AI to avoid facing the wind ?
                if (timeUntilResettingCourse < 0 && useAINav) {
                    // NO, then check if the boat is facing the wind
                    int dir = 180 - (int)Vector3.SignedAngle(agent.steeringTarget - this.transform.position, Vector3.back, Vector3.up);
                    int angleDiff = Mathf.Abs(uiManager.Wind - dir) % 360;
                    if (angleDiff > 160 && angleDiff < 200) { // lest than 20° from facing the wind
                        // change goal for 2sec
                        // first find new angle
                        int newAngle = angleDiff < 180 ? uiManager.Wind -140 : uiManager.Wind + 140; // new angle diff is 140° 
                        // get new direction
                        newDir =  Quaternion.AngleAxis(newAngle, Vector3.up) * Vector3.forward * Common.baseBoatSpeed;
                        storedDestination = agent.destination;
                        timeUntilResettingCourse = 1;
                    }
                }

            }
        }
    }
}
