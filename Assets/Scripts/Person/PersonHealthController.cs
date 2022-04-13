using System;
using UnityEngine;


public class PersonHealthController : MonoBehaviour
{
    public static event Action<int> HealthStartInitialize;
    public static event Action<int> HealthChange;
    public static event Action Died;

    [SerializeField, Min(1)] 
    private int _health;

    private void Awake()
    {
        HealthStartInitialize?.Invoke(_health);

        InputDamage.TakeDamage += TakeDamageHandler;
    }

    private void TakeDamageHandler(int damage)
    {
        if (damage >= _health)
        {
            HealthChange?.Invoke(0);
            Died?.Invoke();
        }
        else
        {
            _health -= damage;
            HealthChange?.Invoke(_health);
        }
    }

    private void OnDestroy()
    {
        InputDamage.TakeDamage -= TakeDamageHandler;
    }
}
