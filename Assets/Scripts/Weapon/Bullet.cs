using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObjectPool))]
public class Bullet : MonoBehaviour
{
    [SerializeField] 
    private float _bulletLifetimeAfterCollision = 1f;

    public float Damage { get; private set; } = 1;

    private Rigidbody _bulletRigidbody;
    private Rigidbody bulletRigidbody => _bulletRigidbody ?? (_bulletRigidbody = GetComponent<Rigidbody>());

    private ObjectPool _objectPool;

    private void Awake()
    {
        _objectPool = GetComponent<ObjectPool>();
    }
    
    public void Initialize(float speed, int damage)
    {
        Damage = damage;
        bulletRigidbody.velocity = transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter()
    {
        StartCoroutine(BulletTimeDestroy());
    }

    private IEnumerator BulletTimeDestroy()
    {
        yield return new WaitForSeconds(_bulletLifetimeAfterCollision);
        _objectPool.ReturnToPool();
    }
}
