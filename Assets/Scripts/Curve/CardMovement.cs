using System;
using DG.Tweening;
using Joystick;
using UnityEngine;

namespace Curve
{
    public class CardMovement : MonoBehaviour
    {
        public static event Action OnEndMove; 

        [SerializeField] private QuadraticCurve _quadraticCurve;
        [SerializeField] private float _movementSpeed = 1;
        [SerializeField] private Transform _bodyTransform;

        private float _sampleTime;
        private bool _isMoving;
        private Tween _tween;

        private void OnEnable()
        {
            PlayerTouchMovement.OnFingerUp += StartMoving;
        }

        private void OnDisable()
        {
            PlayerTouchMovement.OnFingerUp -= StartMoving;
        }

        private void Start()
        {
            _sampleTime = 0f;
        }

        private void Update()
        {
            if (_isMoving)
            {
                _sampleTime += Time.deltaTime * _movementSpeed;
                transform.position = _quadraticCurve.EvaluatePointPosition(_sampleTime);
                transform.forward = _quadraticCurve.EvaluatePointPosition(_sampleTime + 0.001f) - transform.position;

                if (_sampleTime >= 1f)
                {
                    if (_tween != null)
                    {
                        _tween.Kill();
                        _isMoving = false;
                        OnEndMove?.Invoke();
                    }
                }
            }
        }

        private void StartMoving()
        {
            _isMoving = true;
            _tween = _bodyTransform.DORotate(new Vector3(0, 360, 0), 1, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetRelative().SetEase(Ease.Linear);
        }
    }
}
