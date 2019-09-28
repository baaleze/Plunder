using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private int boatId;
    public int BoatId {
        get {return boatId;}
        set {boatId = value;}
    }

    void OnTriggerEnter(Collider other) {
        if (other.name.StartsWith("Boat_NPC")) {
            // get boat comp
            Boat b = other.GetComponent<Boat>();
            if (b.GetId() != boatId) { // we ignore the boat that fired the ball
                b.Damage(10);
                GameObject.Destroy(gameObject); // my job here is done
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0) {
            // went under sea, destroy
            GameObject.Destroy(gameObject);
        }
    }
}
