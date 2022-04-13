using System;
using UnityEngine;

public class InputDamage : MonoBehaviour
{
    public static event Action<int> TakeDamage;

    [SerializeField, Range (1,100), Tooltip("1~100%")] 
    private float _percentBlockDamage;

    private const float convertPercents = 100f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet")) return;
        var bulletBehaviour = other.gameObject.GetComponent<Bullet>();
        Damage(bulletBehaviour.Damage);
    }

    private void Damage(float bulletDamage)
    {
        var damage = (int)Math.Round(bulletDamage * _percentBlockDamage / convertPercents);
        TakeDamage?.Invoke(damage);
    }
}
