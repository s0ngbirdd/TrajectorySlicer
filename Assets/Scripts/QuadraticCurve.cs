using Joystick;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class QuadraticCurve : MonoBehaviour
{
    [SerializeField] private Transform _a;
    [SerializeField] private Transform _b;
    [SerializeField] private Transform _control;
    
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

    public Vector3 Evaluate(float t)
    {
        Vector3 ac = Vector3.Lerp(_a.position, _control.position, t);
        Vector3 cb = Vector3.Lerp(_control.position, _b.position, t);
        return Vector3.Lerp(ac, cb, t);
    }

    private void Update()
    {
        if (_showLine)
        {
            _lineRenderer.positionCount = 20;

            for (int i = 0; i < 20; i++)
            {
                _lineRenderer.SetPosition(i, Evaluate(i / 20f));
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_a == null || _b == null || _control == null)
        {
            return;
        }

        for (int i = 0; i < 20; i++)
        {
            Gizmos.DrawWireSphere(Evaluate(i / 20f), 0.1f);
        }
    }

    private void HideLine()
    {
        _lineRenderer.positionCount = 0;
        _showLine = false;
    }
}
