using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] 
    protected int _damage;
    [SerializeField] 
    protected MusicScriptableObject MusicScriptableObject;
    
    protected AudioSource _audioSource;
    public abstract void DealingDamage(Vector3 mouseWorldPosition);

    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
