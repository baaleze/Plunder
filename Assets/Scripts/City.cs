

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
public class City : MonoBehaviour
{
    public GameObject npcBoatPrefab;
    public GameObject debugFlag;
    public static List<City> allCities = new List<City>();
    private List<City> citiesByNearest;
    private static string[] possibleNames = {"Port Royal", "La Barbade", "Grenade", "Port Louis", "L'ile de la Tortue"};
    private const int rate = 1;
    private const int CITY_RANGE = 4;
    private Vector3 posOnNavMesh;
    

    private UiManager uiManager;
    private string cityName;
    private UnityEvent resourceTick = new UnityEvent();
    private float timeSinceLastTick = 0;
    // tick every 10 sec
    private const float tickTime = 10;
    private float fleetProdtimeSinceLastTick = 0;
    // check fleet production every minute
    private const float tickFleetProd = 60;
    private Vector3 spawnPoint;
    private Faction faction;

    // Abstract indicator of the state of the city.
    // Can be impacted by needed resource availablity, attacks, gifts, etc.
    // A high value will mean better ships produced and better production
    // A low value will sometime mean resorting to piratery
    private int wealth;

    private List<BoatNpc> currentSpawnedBoats = new List<BoatNpc>();
    private List<Production> productions = new List<Production>();
    private List<Common.Resource> needs = new List<Common.Resource>();
    private List<Common.Resource> shortages = new List<Common.Resource>();
    private int[] stock = new int[5];
    

    // Start is called before the first frame update
    void Awake()
    {
        uiManager = GameObject.Find("UiCanvas").GetComponent<UiManager>();
        cityName = possibleNames[Random.Range(0, possibleNames.Length)];
        // init stocks
        for (int i = 0; i < stock.Length; i++)
        {
            stock[i] = Random.Range(0,5) * 100;
        }
        // add to allCities list
        allCities.Add(this);
        // init navMesh position
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 3, -1);
        posOnNavMesh = hit.position;
        // choose faction
        faction = Faction.allFactions[Random.Range(0, Faction.allFactions.Count)];
        wealth = 10; // base wealth
        // choose 2 productions at random
        while(productions.Count < 2) {
            Production choice = Production.prods[Random.Range(0,Production.prods.Length)];
            if (!productions.Contains(choice)) {
                productions.Add(choice);
            }
        }
        // compute needs right now forever
        foreach (Production p in productions)
        {
            foreach (Common.Resource need in p.needs)
            {
                if (productions.Find(prod => prod.produces == need) == null && !needs.Contains(need)) {
                    // a need that the city doesn't produce, add it if not already here
                    needs.Add(need);
                }
            }
        }
        // spawn point
        spawnPoint = transform.GetChild(0).transform.position;

    }

    public void Start() {
        CheckFleetProduction();
    }

    public void AddShortage(Common.Resource r) {
        if (!shortages.Contains(r)) {
            shortages.Add(r);
        }
    }
    public void ResolveShortage(Common.Resource r) {
        if (shortages.Contains(r)) {
            shortages.Remove(r);
        }
    }
    public void AddWealth(int v)
    {
        wealth += v;
        // TODO maybe some triggers ?
    }
    public Vector3 GetPositionOnNavMesh() {
        return posOnNavMesh;
    }

    
    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            uiManager.SetSelectedCity(this);
        }
    }

    public void ListenToResourceTick(UnityAction callback) {
        resourceTick.AddListener(callback);
    }

    public void StopListeningToResourceTick(UnityAction callback) {
        resourceTick.RemoveListener(callback);
    }

    // resolve production and consommation
    private void TickResources() {
        // TODO Regenerate new stocks

        // notify everyone
        resourceTick.Invoke();
    }

    internal void AddResource(Common.Resource resource, int v)
    {
        stock[Common.ResId(resource)] += v;
    }

    internal int GetStock(Common.Resource resource)
    {
        return stock[Common.ResId(resource)];
    }

    public int GetCost(Common.Resource r) {
        return Common.GetCostFromStock(stock[Common.ResId(r)]);
    }

    /// <summary>Called once in a while to spawn new boats</summary>
    private void CheckFleetProduction() {
        if (citiesByNearest == null) { // if it was not init
            CreateListOfCitiesSortedByNearest();
        }

        // Do we need to produce TRADE boats ?
        if (currentSpawnedBoats.Count < PossibleBoatsNumber()) {
            // PATROL or TRADE ?
            NewTradeBoat();
            if (Random.Range(0,2) == 1) {
                
            } else {
                // patrol
                // NewPatrolBoat();
            }
        }
    }

    private void NewPatrolBoat() {
        BoatNpc b = SpawnBoat();
        // TODO
    }

    private void NewTradeBoat() {
        // New trade boat! Choose the destination city, using nearest first
        foreach (City c in citiesByNearest)
        {
            if (currentSpawnedBoats.Find(tb => tb.purpose.cityTarget == c)) {
                // already one going there
                continue;
            }
            // find a resource in the needs of this city and the production of here
            Common.Resource res = c.needs.Find(r => productions.Find(p => p.produces == r) != null);
            if (res != Common.Resource.NONE) {
                // A resource to deliver!!
                // need to create one boat!
                BoatNpc b = SpawnBoat();
                // set its purpose
                b.purpose = new Purpose(Common.Purpose.TRADE, this, c, res);
                // DONE
                return;
            }
        }
    }

    private BoatNpc SpawnBoat() {
        // TODO in the future type of boat is dependent on city wealth and faction
        GameObject boat = GameObject.Instantiate(npcBoatPrefab, spawnPoint, Quaternion.identity);
        BoatNpc boatNpc = boat.GetComponent<BoatNpc>();
        currentSpawnedBoats.Add(boatNpc);
        return boatNpc;
    }

    public void UnSpawnBoat(BoatNpc boat) {
        currentSpawnedBoats.Remove(boat);
        GameObject.Destroy(boat.gameObject);
    }

    private int PossibleBoatsNumber() {
        return Mathf.Min(Mathf.CeilToInt(wealth / 10), Common.MAX_BOATS_BY_CITY);
    }

    private void CreateListOfCitiesSortedByNearest() {
        // copy list
        citiesByNearest = new List<City>(allCities);
        // compute distances
        Dictionary<City, float> distances = new Dictionary<City, float>();
        allCities.ForEach(c => distances[c] = Vector3.Distance(transform.position, c.transform.position));
        // sort
        citiesByNearest.Sort((c1, c2) => distances[c1].CompareTo(distances[c2]));
    }

    public string GenerateCityInfo()
    {
        
        return cityName + " (" + faction.name + ")"
        + "\r\nProduces: " + string.Join("/", productions.ConvertAll(p => p.produces.ToString()))
        + "\r\nCrystals: " + stock[Common.ResId(Common.Resource.CRYSTAL)]  + " (" + GetCost(Common.Resource.CRYSTAL) + " each)"
        + "\r\nWood: " + stock[Common.ResId(Common.Resource.WOOD)]  + " (" + GetCost(Common.Resource.WOOD) + " each)"
        + "\r\nFood: " + stock[Common.ResId(Common.Resource.FOOD)]  + " (" + GetCost(Common.Resource.FOOD) + " each)"
        + "\r\nOre: " + stock[Common.ResId(Common.Resource.ORE)] + " (" + GetCost(Common.Resource.ORE) + " each)"
        + "\r\nTools: " + stock[Common.ResId(Common.Resource.TOOLS)] + " (" + GetCost(Common.Resource.TOOLS) + " each)";
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastTick += Time.deltaTime;
        fleetProdtimeSinceLastTick += Time.deltaTime;

        // 10 SEC TICK
        if (timeSinceLastTick > tickTime) {
            timeSinceLastTick -= tickTime;
            // ??
        }

        // 1 MIN TICK
        if (fleetProdtimeSinceLastTick > tickFleetProd) {
            fleetProdtimeSinceLastTick -= tickFleetProd;
            AddWealth(shortages.Count); // remove wealth for each shortage
            CheckFleetProduction(); // produce fleet ?
            TickResources();
        }
    }
}
