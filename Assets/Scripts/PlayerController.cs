using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI powerText;
    public GameObject winTextObject;
    public Arrow arrow;

    private float angle;
    private float angle_rad;
    private float angle_increase;
    private float acceleration;
    private float power;
    private bool keyShoot;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    private enum state
     {
        aiming, shooting, moving
     };
    private state curState;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        curState = state.aiming;
        SetCountText();
        winTextObject.SetActive(false);
    }

    private void Update()
    {
        if(rb.velocity == new Vector3(0.0f, 0.0f, 0.0f))
        {
            
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        angle_increase = movementVector.x;
        acceleration = 1.0f;

        movementX = movementVector.x;
        movementY = movementVector.y;
        SetCountText();
        if (movementY > 0)
        {
            keyShoot = true;
        }
    }

    void SetCountText()
    {
        countText.text = "Angle:" + angle.ToString();
        if(count >= 8)
        {
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        switch(curState)
        {
            case state.aiming:
                //pretty self explanitory, angle is increased and then converted to radians
                angle += angle_increase * acceleration;
                angle_rad = angle * Mathf.PI / 180;

                //accelerates the turning speed
                if (acceleration < 5.0f)
                {
                    acceleration += 0.1f;
                }

                //turns the ball and cursor
                arrow.transform.rotation = Quaternion.Euler(0, angle, 0);
                arrow.transform.rotation = Quaternion.Euler(0, angle, 0);
                arrow.set_render(true);
                if (keyShoot)
                {
                    curState = state.shooting;
                    keyShoot = false;
                }
                break;
            case state.shooting:
                Vector3 movement = new Vector3(Mathf.Sin(angle_rad), 0.0f, Mathf.Cos(angle_rad));
                power++;
                if(power > 100)
                {
                    power = 0;
                }
                powerText.text = "POW:" + power.ToString();
                if (keyShoot)
                {
                    rb.AddForce(movement * (power * 15));
                    curState = state.moving;
                    keyShoot = false;
                }
                break;
            case state.moving:
                arrow.set_render(false);
                if(rb.velocity.y == 0.0f && keyShoot)
                {
                    rb.AddForce(new Vector3(0.0f, 200.0f, 0.0f));
                    keyShoot = false;
                }
                if(rb.velocity == new Vector3(0.0f, 0.0f, 0.0f))
                {
                    curState = state.aiming;
                    keyShoot = false;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }
    }

    private void hit_ball()
    {
        
    }
}
