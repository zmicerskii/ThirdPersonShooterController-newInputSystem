using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Pool))]
public class Firearms : Weapon
{
    public static event Action<int, int, int> BulletStartInitialize;
    public static event Action<int> BulletsCountChange;
    public static event Action<int, int> ReloadingWeapon;
    public static event Action<bool> CanReload;

    [Header("BulletCharacteristics")]
    [SerializeField, Min(1)] 
    private int _numberClips;
    [SerializeField, Min(1)] 
    private int _maxBulletsInClip;
    [SerializeField, Min(1)] 
    private int _bulletsCount;
    [SerializeField, Min(1)] 
    private float _bulletSpeed;
    [SerializeField] 
    private Transform _bulletSpawnPosition;
    
    [Space(5)]
    [Header("WeaponCharacteristics")]
    [SerializeField, Min(1)] 
    private float _weaponReloadTime;

    private int _allBullets;
    private Pool _pool;
    
    protected override void Awake()
    {
        base.Awake();
        PersonShooterController.WeaponTriggerPressed += DealingDamage;
        PersonShooterController.WeaponReloading += WeaponReload;

        _allBullets = _numberClips * _maxBulletsInClip;
        BulletStartInitialize?.Invoke(_allBullets, _bulletsCount, _maxBulletsInClip);

        _pool = GetComponent<Pool>();
    }

    public override void DealingDamage(Vector3 mouseWorldPosition)
    {
        if (_bulletsCount > 0)
        {
            var aimDirection = (mouseWorldPosition - _bulletSpawnPosition.position).normalized;
            var newBullet = _pool.GetFreeElement(_bulletSpawnPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));

            if (!newBullet.TryGetComponent<Bullet>(out var bulletBehaviour))
            {
                throw new MissingComponentException();
            }

            bulletBehaviour.Initialize(_bulletSpeed, _damage);
            _audioSource.PlayOneShot(MusicScriptableObject.GetAudioClipByType(AudioType.Shooting));

            BulletsCountChange?.Invoke(--_bulletsCount);
        }
        else
        {
            _audioSource.PlayOneShot(MusicScriptableObject.GetAudioClipByType(AudioType.NoBullets));
        }
    }

    private void WeaponReload()
    {
        if (_bulletsCount == _maxBulletsInClip)
        {
            return;
        }
        
        CanReload?.Invoke(false);
        
        StartCoroutine(ReloadRoutine());
    }
    private IEnumerator ReloadRoutine()
    {
        if (_allBullets > 0)
        {
            _audioSource.PlayOneShot(MusicScriptableObject.GetAudioClipByType(AudioType.Reloading));
            yield return new WaitForSeconds(_weaponReloadTime);

            _allBullets = _allBullets + _bulletsCount - _maxBulletsInClip;

            if (_allBullets < 0)
            {
                _bulletsCount = _maxBulletsInClip + _allBullets;
                _allBullets = 0;
            }
            else
            {
                _bulletsCount = _maxBulletsInClip;
            }

            ReloadingWeapon?.Invoke(_allBullets, _bulletsCount);
        }
        
        CanReload?.Invoke(true);
    }

    private void OnDestroy()
    {
        PersonShooterController.WeaponTriggerPressed -= DealingDamage;
        PersonShooterController.WeaponReloading -= WeaponReload;
    }
}
