public interface IStatusEffect
{
    string Name { get; }
    void OnApply(HealthComponent health, int tickAmount, bool putOnHold = false);
    bool OnUpdate(HealthComponent health);
    void OnRemove(HealthComponent health);
    void ReleaseHold();

}




