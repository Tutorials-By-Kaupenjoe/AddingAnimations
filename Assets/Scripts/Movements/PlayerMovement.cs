using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum MovementType
    {
        RigidbodyVelocity,
        RigidbodyAddForce,
        VectorMoveTowards,
        TransformTranslate,
        DirectPositionChange
    }

    // The Speed variable can be used in each of the 5 Movement Variants
    [SerializeField] private float speed = 3f;

    // The rigidbody is only used in two of the five Variants
    // If the rigidbody is used, the Physics System is also being used! 
    private Rigidbody2D body;

    // The 2D Vector saves your Movement in both X and Y direction is an "easier" way
    // than checking for each of the buttons pressed. This should also work with gamepads without changes! 
    private Vector2 axisMovement;

    // We can change this variable to change the Movement Type we wish to use
    // This should be changed in the inspector
    [SerializeField] private MovementType movementType = MovementType.RigidbodyVelocity;

    private PlayerAnimations playerAnimations;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerAnimations = GetComponent<PlayerAnimations>();
    }

    void Update()
    {
        axisMovement.x = Input.GetAxisRaw("Horizontal");
        axisMovement.y = Input.GetAxisRaw("Vertical");

        if (movementType == MovementType.VectorMoveTowards)
        {
            MoveTowards();
        }

        if (movementType == MovementType.TransformTranslate)
        {
            Translate();
        }

        if (movementType == MovementType.DirectPositionChange)
        {
            PositionChange();
        }
    }

    public bool IsMoving()
    {
        return axisMovement.x != 0 || axisMovement.y != 0;
    }

    // The Rigidbody Movement Methods are inside the FixedUpdate
    // because they are Physics based which ought to go into this method
    private void FixedUpdate()
    {
        if (movementType == MovementType.RigidbodyVelocity)
        {
            RigidbodyVelocity();
        }

        if (movementType == MovementType.RigidbodyAddForce)
        {
            RigidbodyAddForce();
        }
    }


    #region Movement 1: RigidbodyVelocity

    private void RigidbodyVelocity()
    {
        // Here we don't need to check for any inputs as the inputs
        // are dealt with in-line with the GetAxisRaw call
        body.velocity = axisMovement.normalized * speed;

        playerAnimations.SetMovement(axisMovement);
    }

    #endregion


    #region Movement 2: Rigidbody Add Force

    private void RigidbodyAddForce()
    {
        body.AddForce(axisMovement * speed, ForceMode2D.Impulse);
    }

    #endregion


    #region Movement 3: Vector Move Towards

    private void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            transform.position + (Vector3)axisMovement, speed * Time.deltaTime);
    }

    #endregion


    #region Movement 4: Transform Translate

    private void Translate()
    {
        transform.Translate(axisMovement * speed * Time.deltaTime);
    }

    #endregion


    #region Movement 5: Direct Position Change

    private void PositionChange()
    {
        transform.position += (Vector3)axisMovement * Time.deltaTime * speed;
    }

    #endregion

}
