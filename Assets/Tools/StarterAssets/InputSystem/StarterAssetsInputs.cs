using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		
		public bool jump;
		public bool sprint;
		public bool crouch;
		public bool aim;
		public bool shoot;
		public bool weaponReloading;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value) => move = value.Get<Vector2>();
		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				look = value.Get<Vector2>();
			}
		}
		public void OnJump(InputValue value) => jump = value.isPressed;
		public void OnSprint(InputValue value) => sprint = value.isPressed;
		public void OnCrouch (InputValue value) => crouch = value.isPressed;
		public void OnAim (InputValue value) => aim = value.isPressed;
        public void OnShoot(InputValue value) => shoot = value.isPressed;
        public void OnWeaponReload(InputValue value) => weaponReloading = value.isPressed;

#endif

#if !UNITY_IOS || !UNITY_ANDROID
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}
		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
#endif
	}	
}