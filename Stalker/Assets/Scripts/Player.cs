using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float _speed;
    
    private CharacterController _characterController;
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_characterController.isGrounded)
        {
            Vector3 playerSpeed = _inputReader.PlayerInput.normalized * (_speed * Time.deltaTime);
            _characterController.Move(playerSpeed + Vector3.down);
        }
        else
        {
            _characterController.Move(Physics.gravity * Time.deltaTime);
        }
    }
}
