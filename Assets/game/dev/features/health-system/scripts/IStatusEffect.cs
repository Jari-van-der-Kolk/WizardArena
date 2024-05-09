public interface IStatusEffect
{
    void OnApply(HealthComponent health, int tickAmount);
    bool OnUpdate(HealthComponent health);
    void OnRemove(HealthComponent health);
    void OnReset();

}




