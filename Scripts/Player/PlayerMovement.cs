using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public Vector2 PlayerVelocity;


    private CharacterController MoveController;
    void Start()
    {
        MoveController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveController.Move(new Vector3(PlayerVelocity.x, 0, PlayerVelocity.y) * Time.deltaTime);
        if (!MoveController.isGrounded)
        {
            //transform.position = 0.1f * Vector3.down + transform.position;
            MoveController.Move(3 * Vector3.down *Time.deltaTime);
        }
    }
}
