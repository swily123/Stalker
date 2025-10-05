using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance;
    [SerializeField] private float _speed;
    [SerializeField] private float _gravityScaler;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 gravity = Physics.gravity * Time.fixedDeltaTime;
        
        if (IsEnoughToTarget() == false)
        {
            Vector3 velocity  = GetDistance().normalized;
            velocity.y = 0;
            velocity *= _speed * Time.fixedDeltaTime;
            velocity += gravity;
            _rigidbody.Move(transform.position + velocity, Quaternion.identity);
        }
        else
        {
            _rigidbody.Move(transform.position + gravity, Quaternion.identity);
        }
    }

    private bool IsEnoughToTarget()
    {
        float distance = GetDistance().sqrMagnitude;
        bool isTouch = distance <= _distance * _distance;
        return isTouch;
    }

    private Vector3 GetDistance()
    {
        Vector3 position = transform.position;
        Vector3 targetPosition = _target.position;
        Vector3 distance = targetPosition - position;
        return distance;
    }
}