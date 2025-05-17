using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform _playerTransform;
    public Weapon weapon;
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float maxSpeed = 10f;
    private Rigidbody2D _rb;

    Vector2 _mousePosition;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the player object.");
        }

        _playerTransform = transform;
    }

    private void Update()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        XYMovement();
        SmothRotatePlayer();

        if (Input.GetMouseButtonDown(0))
        {
            weapon.Fire();
        }
    }

    private void XYMovement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 moveVelocity = moveInput.normalized * 1f;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVelocity *= maxSpeed;
        }
        else
        {
            moveVelocity *= moveSpeed;
        }

        _rb.velocity = new Vector2(moveVelocity.x, moveVelocity.y);
    }

    private void SmothRotatePlayer()
    {
        Vector2 direction = _mousePosition - _rb.position;

        // Calculate the angle between the player and the mouse position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Create a rotation based on the angle
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        // Smoothly rotate the player towards the target rotation
        _playerTransform.rotation = Quaternion.RotateTowards(_playerTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);



        // if (moveInput == Vector2.zero)
        // {
        //     return;
        // }

        // float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
        // Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);

    }

}
