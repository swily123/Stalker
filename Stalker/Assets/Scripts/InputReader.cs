using UnityEngine;

public class InputReader : MonoBehaviour
{
    public Vector3 PlayerInput => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
}
