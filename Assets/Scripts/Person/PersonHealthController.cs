using System;
using SimpleEventBus.Disposables;
using UnityEngine;


public class PersonHealthController : MonoBehaviour
{
    public static event Action<int> HealthStartInitialize;
    public static event Action<int> HealthChange;
    public static event Action Died;

    [SerializeField, Min(1)] 
    private int _health;

    private CompositeDisposable _subscriptions;
    
    private void Awake()
    {
        HealthStartInitialize?.Invoke(_health);

        _subscriptions = new CompositeDisposable
        {
            EventStreams.Game.Subscribe<TakeDamageEvent>(TakeDamageHandler)
        };
    }

    private void TakeDamageHandler(TakeDamageEvent eventData)
    {
        if (eventData.Damage >= _health)
        {
            HealthChange?.Invoke(0);
            Died?.Invoke();
        }
        else
        {
            _health -= eventData.Damage;
            HealthChange?.Invoke(_health);
        }
    }

    private void OnDestroy()
    {
        _subscriptions?.Dispose();
    }
}
