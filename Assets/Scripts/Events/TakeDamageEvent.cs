using SimpleEventBus.Events;

public class TakeDamageEvent : EventBase
{
    public int Damage { get; }

    public TakeDamageEvent(int damage)
    {
        Damage = damage;
    }
}