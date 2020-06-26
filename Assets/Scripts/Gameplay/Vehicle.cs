using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vehicle : MonoBehaviour
{
    public enum VehicleType
    {
        CarBlack,
        CarRed,
        CarBlue
    }

    private Rigidbody2D rb;
    private Collider2D vehicleCollider;
    private bool isMoving = false;

    public Vector2 Velocity { get; set; }
    public Vector2 Destination { get; set; }
    public Collider2D FinishLine { get; set; }

    /**
     * Events
     */
    public static event Action OnVehicleCollided;
    public static event Action OnVehicleLiberated;


    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        vehicleCollider = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving) return;
        rb.velocity = Velocity;
        if (Vector2.Distance(transform.position, Destination) < 0.25)
        {
            StopMovement();
        }

    }

    public void StartMovement()
    {
        gameObject.SetActive(true);
        isMoving = true;
        vehicleCollider.enabled = true;
    }

    private void StopMovement()
    {
        isMoving = false;
        rb.velocity = new Vector2(0.0f, 0.0f);
        gameObject.SetActive(false);
    }

    /**
    * COLLISION
    */
    void OnCollisionEnter2D(Collision2D collision)
    {
        isMoving = false;
        OnVehicleCollided?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != FinishLine) { return; }
        OnVehicleLiberated?.Invoke();
        vehicleCollider.enabled = false;
    }

    /**
    * EVENTS
    */
    void OnMouseDown()
    {
        ToggleMovement();
    }

    void ToggleMovement()
    {
        isMoving = !isMoving;
        if (!isMoving)
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
        }
    }
}
