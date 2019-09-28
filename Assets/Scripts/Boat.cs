using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boat : MonoBehaviour
{
    public readonly int id = 0;
    public bool npc = false;
    public City cityInRange;
    private UiManager uiManager;
    protected BoatModel model = BoatModel.Default();
    public int gold;
    public GameObject cannonBallPrefab;
    public const int maxHealth = 100;
    protected int health = 100;
    public int Health {
        get {return health;}
        set {health = value;}
    }
    public bool firing = false;
    public float fireCooldownTime = -1;
    public float fireCooldown = 2;
    public BoatNpc target;

    protected int[] stock = new int[5];

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("UiCanvas").GetComponent<UiManager>();
        gold = 2000;
    }

    void OnTriggerEnter(Collider other) {
        if (other.name == "Range") {
            addCityInRange(other.GetComponentInParent<City>());
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.name == "Range") {
            removeCityInRange(other.GetComponentInParent<City>());
        }
    }

    public void addCityInRange(City city) {
        if (cityInRange == null) {
            cityInRange = city;
            if (!npc){
                uiManager?.updateCityInRange(city);
            }
        }
    }

    public void removeCityInRange(City city) {
        if (city == cityInRange) {
            cityInRange = null;
            if (!npc){
                uiManager?.updateCityInRange(null);
            }
        }
    }

    public virtual void Damage(int dmg) {
        health -= dmg;
        // TODO other triggers
    }

    public virtual int GetId() {
        return id;
    }

    public void BuyFromCityInRange(Common.Resource resource, int quantity) {
        if (cityInRange == null){
            throw new System.Exception("NO CITY IN RANGE");
        }
        int cost = cityInRange.GetCost(resource) * quantity;
        if (gold < cost) {
            throw new System.Exception("NOT ENOUGH GOLD");
        } else if (cityInRange.GetStock(resource) < quantity) {
            throw new System.Exception("NOT ENOUGH RESOURCE");
        } else {
            // OK
            gold -= cost;
            cityInRange.AddResource(resource, -quantity);
            stock[Common.ResId(resource)] += quantity;
        }
    }

    public void SellFromCityInRange(Common.Resource resource, int quantity) {
        if (cityInRange == null){
            throw new System.Exception("NO CITY IN RANGE");
        }
        int cost = cityInRange.GetCost(resource) * quantity;
        if (stock[Common.ResId(resource)] < quantity) {
            throw new System.Exception("NOT ENOUGH RESOURCE");
        } else {
            // OK
            gold += cost;
            cityInRange.AddResource(resource, quantity);
            stock[Common.ResId(resource)] -= quantity;
        }
    }

    public int GetStock(Common.Resource r) {
        return stock[Common.ResId(r)];
    }

    private void FireCannons(bool left) {
        for (int i = 0; i < 5; i++)
        {
            int offset = (i - 2) * 5;
            GameObject ball = Instantiate(cannonBallPrefab, transform.position + transform.up * 5 + transform.forward * offset, transform.rotation);
            ball.GetComponent<CannonBall>().BoatId = GetId(); // tell the boal it's mine
            // add an impulsion
            Rigidbody body = ball.GetComponent<Rigidbody>();
            body.velocity =  Quaternion.AngleAxis(-30, left ? ball.transform.forward : -ball.transform.forward) * (left ? -transform.right : transform.right) * 30 + GetComponent<NavMeshAgent>().velocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && firing && fireCooldownTime < 0) {
            float angle = Vector3.SignedAngle(transform.forward, target.transform.position - transform.position, Vector3.up);
            if (Mathf.Abs(angle) > 85 && Mathf.Abs(angle) < 95) {
                // FIRE!!
                FireCannons(angle < 0); // left if -90°, right if 90°
                fireCooldownTime = fireCooldown;
            }
        }
        fireCooldownTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F)) {
            FireCannons(true);
        } else if (Input.GetKeyDown(KeyCode.G)) {
            FireCannons(false);
        }
    }
}
