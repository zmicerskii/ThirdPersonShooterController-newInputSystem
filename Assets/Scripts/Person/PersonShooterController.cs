using System;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;

public class PersonShooterController : MonoBehaviour
{
    public static event Action<Vector3> WeaponTriggerPressed;
    public static event Action WeaponReloading;

    [SerializeField] 
    private CinemachineVirtualCamera _personFollowComponent;

    [SerializeField] 
    private float _aimIdle = 25f;
    [SerializeField] 
    private float _aimApproximation = 15f;

    [SerializeField] 
    private float _idleSensitivity = 1f;
    [SerializeField] 
    private float _aimSensitivity = 0.5f;

    [SerializeField] 
    private LayerMask _aimColliderLayerMask;

    [SerializeField] 
    private Image _crosshair;

    [SerializeField] 
    private float _smoothnessTurningToCrosshair = 1f;

    private StarterAssetsInputs _input;
    private ThirdPersonController _thirdPersonController;
    private bool _isReload = true;

    private void Awake()
    {
        Firearms.CanReload += ReloadHandler;
        
        _input = GetComponent<StarterAssetsInputs>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void FixedUpdate()
    {
        if (!_isReload) return;
        AimShooting();
        ReloadingWeapon();
    }

    private void AimShooting()
    {
        if (_input.aim)
        {
            var mouseWorldPosition = Vector3.zero;
            var screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
            var ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 500f, _aimColliderLayerMask))
            {
                mouseWorldPosition = raycastHit.point;
            }

            AimState(_aimApproximation, _aimSensitivity, true, false);
            
            var worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            var aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * _smoothnessTurningToCrosshair);

            Shoot(mouseWorldPosition);
        }
        else
        {
            AimState(_aimIdle, _idleSensitivity, false, true);
        }
    }
    private void Shoot (Vector3 mouseWorldPosition)
    {
        if (!_input.shoot) return;
        WeaponTriggerPressed?.Invoke(mouseWorldPosition);
        _input.shoot = false;
    }

    private void ReloadingWeapon()
    {
        if (_input.weaponReloading)
        {
            WeaponReloading?.Invoke();
        }
    }

    private void ReloadHandler(bool flag) => _isReload = flag;

    private void AimState(float aimValue, float sensitivity, bool crosshairEnabled, bool rotateOnMove)
    {
        _personFollowComponent.m_Lens.FieldOfView = aimValue;
        _thirdPersonController.SetSensitivity(sensitivity);
        _thirdPersonController.SetRotateOnMove(rotateOnMove);
        _crosshair.enabled = crosshairEnabled;
    }
    private void OnDestroy()
    {
        Firearms.CanReload -= ReloadHandler;
    }
}
