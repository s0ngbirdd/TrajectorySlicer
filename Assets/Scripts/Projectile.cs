using DG.Tweening;
using Joystick;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private QuadraticCurve _curve;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _child;

    private float _sampleTime;
    private bool _startedMoving;
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
        if (_startedMoving)
        {
            _sampleTime += Time.deltaTime * _speed;
            transform.position = _curve.Evaluate(_sampleTime);
            transform.forward = _curve.Evaluate(_sampleTime + 0.001f) - transform.position;

            if (_sampleTime >= 1f)
            {
                Debug.Log("FINISH!!!");
                _tween.Kill();
                Destroy(gameObject);
            }
        }
    }

    private void StartMoving()
    {
        _startedMoving = true;
        _tween = _child.DORotate(new Vector3(0, 360, 0), 1, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetRelative().SetEase(Ease.Linear);
    }
}
