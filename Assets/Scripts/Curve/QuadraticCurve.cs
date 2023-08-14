using Joystick;
using UnityEngine;

namespace Curve
{
    [RequireComponent(typeof(LineRenderer))]
    public class QuadraticCurve : MonoBehaviour
    {
        [SerializeField] private Transform _pointA;
        [SerializeField] private Transform _pointB;
        [SerializeField] private Transform _middlePoint;
        [SerializeField] private int _curvePointNumber = 20;
    
        private LineRenderer _lineRenderer;
        private bool _showLine = true;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }
    
        private void OnEnable()
        {
            PlayerTouchMovement.OnFingerUp += HideLine;
        }

        private void OnDisable()
        {
            PlayerTouchMovement.OnFingerUp -= HideLine;
        }

        public Vector3 EvaluatePointPosition(float t)
        {
            Vector3 aToMiddle = Vector3.Lerp(_pointA.position, _middlePoint.position, t);
            Vector3 middleToB = Vector3.Lerp(_middlePoint.position, _pointB.position, t);
            return Vector3.Lerp(aToMiddle, middleToB, t);
        }

        private void Update()
        {
            if (_showLine)
            {
                _lineRenderer.positionCount = _curvePointNumber;

                for (int i = 0; i < _curvePointNumber; i++)
                {
                    _lineRenderer.SetPosition(i, EvaluatePointPosition(i / (float)_curvePointNumber));
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_pointA == null || _pointB == null || _middlePoint == null)
            {
                return;
            }

            for (int i = 0; i < _curvePointNumber; i++)
            {
                Gizmos.DrawWireSphere(EvaluatePointPosition(i / (float)_curvePointNumber), 0.1f);
            }
        }

        private void HideLine()
        {
            _lineRenderer.positionCount = 0;
            _showLine = false;
        }
    }
}
