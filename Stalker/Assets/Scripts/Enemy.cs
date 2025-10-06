using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    [Header("Movement")]  
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance;
    [SerializeField] private float _speed;
    
    [Header("Ground Detection")]  
    [SerializeField] private float _groundCheckDistance = 1.1f;
    [SerializeField] private LayerMask _groundLayer;
    
    [Header("Step Detection")]  
    [SerializeField] private float _maxStepHeight = 0.3f;
    [SerializeField] private float _stepCheckDistance = 0.5f;
    [SerializeField] private float _stepStrength = 1;

    private Rigidbody _rigidbody;
    private Collider _collider;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (IsEnoughToTarget() == false)
        {
            Vector3 gravity = Physics.gravity * Time.fixedDeltaTime;
            bool isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayer);

            if (isGrounded)
            {
                Vector3 direction = GetDirection().normalized;
                Vector3 velocityY = Vector3.zero;
                
                direction.y = 0;
                gravity = Vector3.down * Time.fixedDeltaTime;
                
                if (IsStepAhead(direction, out Vector3 stepSize))
                {
                    _rigidbody.position = _rigidbody.position + Vector3.up * (stepSize.y * _stepStrength);
                }
                
                Vector3 velocity = direction * (_speed * Time.fixedDeltaTime);
                _rigidbody.Move(transform.position + velocity + velocityY + gravity, Quaternion.identity);
            }
            else
            {
                _rigidbody.Move(transform.position + gravity, Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * _groundCheckDistance);
    }
    
    private bool IsEnoughToTarget()
    {
        float distance = GetDirection().sqrMagnitude;
        bool isTouch = distance <= _distance * _distance;
        return isTouch;
    }

    private Vector3 GetDirection()
    {
        return  _target.position - transform.position;
    }

    private bool IsStepAhead(Vector3 moveDirection, out Vector3 stepSize)
    {
        Vector3 checkDirection = moveDirection.normalized;
        stepSize = Vector3.zero;
            
        float bottomY = transform.position.y - (_collider.bounds.size.y / 2);

        Vector3 footRayOrigin = new Vector3(
            transform.position.x,
            bottomY + 0.05f,
            transform.position.z
        );

        Vector3 kneeRayOrigin = new Vector3(
            transform.position.x,
            bottomY + _maxStepHeight,
            transform.position.z
        );

        bool footBlocked = Physics.Raycast(footRayOrigin, checkDirection, out RaycastHit hit, _stepCheckDistance);
        bool kneeClear = Physics.Raycast(kneeRayOrigin, checkDirection, _stepCheckDistance) == false;

        if (footBlocked)
        {
            stepSize = hit.collider.bounds.size;
        }

        return footBlocked && kneeClear;
    }
}