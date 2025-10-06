using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance = 0.5f;
    [SerializeField] private float _speed = 3f;

    [Header("Ground Detection")]
    [SerializeField] private float _groundCheckDistance = 1.1f;
    [SerializeField] private LayerMask _groundLayer = ~0;

    [Header("Step Detection")]
    [SerializeField] private float _maxStepHeight = 0.3f;
    [SerializeField] private float _stepCheckDistance = 0.5f;
    [SerializeField] private float _stepStrength = 3f;

    private Rigidbody _rigidbody;
    private Collider _collider;
    private bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void FixedUpdate()
    {
        if (_target == null) return;

        Vector3 toTarget = _target.position - transform.position;

        if (toTarget.sqrMagnitude <= _distance * _distance)
        {
            _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);
            return;
        }

        Vector3 dir = new Vector3(toTarget.x, 0f, toTarget.z);

        if (dir.sqrMagnitude > 0.0001f) 
            dir.Normalize();

        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayer, QueryTriggerInteraction.Ignore);

        Vector3 desiredHorizontal = dir * _speed;

        if (_isGrounded && StepAhead(dir))
        {
            float vy = Mathf.Max(_rigidbody.velocity.y, _stepStrength);
            _rigidbody.velocity = new Vector3(desiredHorizontal.x, vy, desiredHorizontal.z);
        }
        else
        {
            _rigidbody.velocity = new Vector3(desiredHorizontal.x, _rigidbody.velocity.y, desiredHorizontal.z);
        }
    }

    private bool StepAhead(Vector3 moveDir)
    {
        Bounds b = _collider.bounds;
        float bottomY = b.min.y;

        Vector3 originLow = new Vector3(b.center.x, bottomY + 0.05f, b.center.z);
        Vector3 originHigh = new Vector3(b.center.x, bottomY + _maxStepHeight, b.center.z);

        bool footBlocked = Physics.Raycast(originLow, moveDir, _stepCheckDistance, _groundLayer, QueryTriggerInteraction.Ignore);
        bool kneeClear = !Physics.Raycast(originHigh, moveDir, _stepCheckDistance, _groundLayer, QueryTriggerInteraction.Ignore);

        return footBlocked && kneeClear;
    }
}