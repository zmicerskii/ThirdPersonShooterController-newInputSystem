using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text _idleHealth;
    [SerializeField] 
    private TMP_Text _health;
    [SerializeField] 
    private TMP_Text _allBullets;
    [SerializeField] 
    private TMP_Text _idleBulletsClip;
    [SerializeField] 
    private TMP_Text _bulletsCount;
    [SerializeField] 
    private TMP_Text _firstTeamScore;
    [SerializeField] 
    private TMP_Text _secondTeamScore;

    private const string _line = "/";

    private void Awake()
    {
        PersonHealthController.HealthStartInitialize += OnHealthInitialize;
        Firearms.BulletStartInitialize += OnBulletInitialize;
        Firearms.BulletsCountChange += OnBulletsCountChange;
        Firearms.ReloadingWeapon += OnReloadingWeapon;
        PersonHealthController.HealthChange += HealthChangeHandler;
    }

    private void OnHealthInitialize(int health)
    {
        _idleHealth.text = health + _line;
        _health.text = _idleHealth.text;
    }
    private void HealthChangeHandler(int health)
    {
        _health.text = health.ToString();
    }

    private void OnBulletInitialize(int allBullets, int bulletsCount, int maxBulletInClip)
    {
        _allBullets.text = allBullets.ToString();
        _idleBulletsClip.text = _line + maxBulletInClip;
        _bulletsCount.text = bulletsCount.ToString();
    }

    private void OnBulletsCountChange(int bulletsCount)
    {
        _bulletsCount.text = bulletsCount.ToString();
    }
    
    private void OnReloadingWeapon (int allBullets, int bulletsCount)
    {
        _allBullets.text = allBullets.ToString();
        _bulletsCount.text = bulletsCount.ToString ();
    }

    private void OnDestroy()
    {
        PersonHealthController.HealthStartInitialize -= OnHealthInitialize;
        Firearms.BulletStartInitialize -= OnBulletInitialize;
        Firearms.BulletsCountChange -= OnBulletsCountChange;
        Firearms.ReloadingWeapon -= OnReloadingWeapon;
    }
}
