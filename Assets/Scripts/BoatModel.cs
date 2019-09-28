
public class BoatModel {
    public readonly int maxStorage;
    public readonly float speed;
    public readonly float accel;
    public readonly float turningSpeed;

    public BoatModel(int st, float s, float a, float t) {
        maxStorage = st;
        speed = s;
        accel = a;
        turningSpeed = t;
    }

    public static BoatModel Default() {
        return new BoatModel(300, 30, 15, 60);
    }
}