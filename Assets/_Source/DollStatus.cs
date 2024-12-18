using R3;

public class DollStatus
{
    public readonly ClampedReactiveProperty<float> Satiety = new(1, 0, 1);
    public readonly ClampedReactiveProperty<float> Energy = new(1, 0, 1);
}