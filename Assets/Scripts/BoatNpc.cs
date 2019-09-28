using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatNpc : Boat
{
    private static int nextId = 1;
    public new readonly int id = nextId++;
    City currentTarget;
    BoatNpcNavAgent navAgent;
    
    private Purpose _purpose;
    // Start is called before the first frame update
    void Awake()
    {
        navAgent = GetComponent<BoatNpcNavAgent>();
    }

    public Purpose purpose {
        get { return _purpose;}
        set {
            this._purpose = value;
            // new purpose!!
            if (_purpose.purpose == Common.Purpose.TRADE) {
                // fill storage with resource
                stock[Common.ResId(_purpose.resourceTarget)] += model.maxStorage;
                // depart!
                navAgent.GoToCity(_purpose.cityTarget);
            }
        }
    }

    public override int GetId() {
        return id;
    }

    public override void Damage(int dmg) {
        health -= dmg;
        if (health < 0) {
            // destroyed!!
            if (purpose.purpose == Common.Purpose.TRADE) {
                // add a shortage on target city
                purpose.cityTarget.AddShortage(purpose.resourceTarget);
                // owner doesn't get gold in return so decrease wealth
                purpose.owner.AddWealth(-1);
            }
            // unspawn
            purpose.owner.UnSpawnBoat(this);
        }
    }

    public void GoBackToOwner() {
        Debug.Log("Going back");
        navAgent.GoToCity(purpose.owner);
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(1)) {
            // target this boat if right click on it FIX THIS
            GameObject.Find("Boat_Player").GetComponent<Boat>().target = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cityInRange != null) {
            // a city is in range
            if (cityInRange == purpose.cityTarget && purpose.purpose == Common.Purpose.TRADE && !purpose.done) {
                // trade successful !! wait 10 sec around
                navAgent.GoTo(transform.position + transform.forward);
                purpose.done = true;
                Invoke("GoBackToOwner", 10);
                // the city gets a bonus to wealth!
                cityInRange.AddWealth(1);
            } else if (cityInRange == purpose.owner && purpose.purpose == Common.Purpose.TRADE && purpose.done) {
                // came back to owner and DONE with task, DIE
                cityInRange.UnSpawnBoat(this);                
            }
        }
    }
}
