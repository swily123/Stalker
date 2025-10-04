using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance;
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (IsEnoughToTarget() == false)
        {
            Vector3 velocity = GetDistance().normalized;
            velocity *= _speed;
            
            _rigidbody.velocity = velocity;
        }
        
        _rigidbody.velocity += Physics.gravity;
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