using System;
using UnityEngine;

public class InputDamage : MonoBehaviour
{
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
        EventStreams.Game.Publish(new TakeDamageEvent(damage));
    }
}
