using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace Joystick
{
    public class PlayerTouchMovement : MonoBehaviour
    {
        public static event Action OnFingerUp;
        
        [Header("Joystick")]
        [SerializeField] private Vector2 _joystickSize = new Vector2(200, 200);
        [SerializeField] private FloatingJoystick _joystick;
        
        [Header("Player Movement")]
        [SerializeField] private float _moveSpeed = 10;
        [SerializeField] private float _borderMaxXPosition = 1.95f;
        [SerializeField] private float _borderMaxZPosition = 1.95f;

        private Finger _movementFinger;
        private Vector2 _movementAmount;

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += HandleFingerDown;
            ETouch.Touch.onFingerUp += HandleFingerUp;
            ETouch.Touch.onFingerMove += HandleFingerMove;
        }

        private void OnDisable()
        {
            ETouch.Touch.onFingerDown -= HandleFingerDown;
            ETouch.Touch.onFingerUp -= HandleFingerUp;
            ETouch.Touch.onFingerMove -= HandleFingerMove;
            EnhancedTouchSupport.Disable();
        }

        private void Update()
        {
            Vector3 verticalMove = Vector3.zero;
            Vector3 horizontalMove = Vector3.zero;
        
            if (transform.position.z * Mathf.Sign(_movementAmount.y) < _borderMaxZPosition)
            {
                verticalMove = transform.forward * (_movementAmount.y * _moveSpeed * Time.deltaTime);
            }
            
            if (transform.position.x * Mathf.Sign(_movementAmount.x) < _borderMaxXPosition)
            {
                horizontalMove = transform.right * (_movementAmount.x * _moveSpeed * Time.deltaTime);
            }
            
            transform.Translate(verticalMove + horizontalMove);
        }

        private void HandleFingerDown(Finger touchedFinger)
        {
            if (_movementFinger == null)
            {
                _movementFinger = touchedFinger;
                _movementAmount = Vector2.zero;
                _joystick.gameObject.SetActive(true);
                _joystick.rectTransform.sizeDelta = _joystickSize;
                _joystick.rectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
            }
        }
    
        private void HandleFingerUp(Finger lostFinger)
        {
            if (lostFinger == _movementFinger)
            {
                _movementFinger = null;
                _joystick.knob.anchoredPosition = Vector2.zero;
                _joystick.gameObject.SetActive(false);
                _movementAmount = Vector2.zero;
                
                OnFingerUp?.Invoke();
                Destroy(this);
            }
        }
    
        private void HandleFingerMove(Finger movedFinger)
        {
            if (movedFinger == _movementFinger)
            {
                Vector2 knobPosition;
                float maxMovement = _joystickSize.x / 2f;
                ETouch.Touch currentTouch = movedFinger.currentTouch;

                if (Vector2.Distance(currentTouch.screenPosition, _joystick.rectTransform.anchoredPosition) > maxMovement)
                {
                    knobPosition = (currentTouch.screenPosition - _joystick.rectTransform.anchoredPosition).normalized * maxMovement;
                }
                else
                {
                    knobPosition = currentTouch.screenPosition - _joystick.rectTransform.anchoredPosition;
                }

                _joystick.knob.anchoredPosition = knobPosition;
                _movementAmount = knobPosition / maxMovement;
            }
        }

        private Vector2 ClampStartPosition(Vector2 startPosition)
        {
            if (startPosition.x < _joystickSize.x / 2)
            {
                startPosition.x = _joystickSize.x / 2;
            }
            else if (startPosition.x > Screen.width - _joystickSize.x / 2)
            {
                startPosition.x = Screen.width - _joystickSize.x / 2;
            }

            if (startPosition.y < _joystickSize.y / 2)
            {
                startPosition.y = _joystickSize.y / 2;
            }
            else if (startPosition.y > Screen.height - _joystickSize.y / 2)
            {
                startPosition.y = Screen.height - _joystickSize.y / 2;
            }

            return startPosition;
        }
    }
}
