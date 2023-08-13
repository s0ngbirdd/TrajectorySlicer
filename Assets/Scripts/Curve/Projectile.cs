using System;
using DG.Tweening;
using Joystick;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static event Action OnEndMove; 

    [SerializeField] private QuadraticCurve _curve;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _child;
    //[SerializeField] private GameObject _poofParticle;

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
            _sampleTime += Time.deltaTime * _speed;
            transform.position = _curve.Evaluate(_sampleTime);
            transform.forward = _curve.Evaluate(_sampleTime + 0.001f) - transform.position;

            if (_sampleTime >= 1f)
            {
                //Debug.Log("FINISH!!!");
                if (_tween != null)
                {
                    _tween.Kill();
                    _isMoving = false;
                    OnEndMove?.Invoke();
                }
                //Instantiate(_poofParticle, transform.position, Quaternion.identity);
                //Destroy(gameObject);
            }
        }
    }

    private void StartMoving()
    {
        _isMoving = true;
        _tween = _child.DORotate(new Vector3(0, 360, 0), 1, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetRelative().SetEase(Ease.Linear);
    }
}
