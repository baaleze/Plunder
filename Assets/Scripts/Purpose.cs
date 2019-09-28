
public class Purpose {
    public readonly Common.Purpose purpose;
    public readonly City owner;
    public readonly City cityTarget;
    public readonly Common.Resource resourceTarget;
    public bool done = false;
    public bool waitingForAnother = false;

    public Purpose(Common.Purpose p, City o, City ct, Common.Resource rt) {
        purpose = p;
        owner = o;
        cityTarget = ct;
        resourceTarget = rt;
    }
}