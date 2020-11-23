using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI winTextObject;
    public Arrow arrow;
    public GameObject colorCircle;

    private float angle;
    private float angle_rad;
    private float angle_increase;
    private float acceleration;
    private float power;
    private bool keyShoot;

    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private int count;
    private float movementX;
    private float movementY;
    private int powerup;
    private bool powerup_used;
    private int shots;

    private enum BallColor
    {
        CYAN, RED, PURPLE, GREEN
    };
    private BallColor color;

    private enum State
     {
        AIMING, SHOOTING, MOVING
     };
    private State curState;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        count = 0;
        curState = State.AIMING;
        SetCountText();
        winTextObject.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }

    private void Update()
    {
        switch_color();
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            keyShoot = true;
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
    }

    void SetCountText()
    {
        if (shots.ToString().Length == 1)
        {
            countText.text = "SHOT \n00" + shots.ToString();
        }
        else countText.text = "SHOT \n0" + shots.ToString();
    }

    void FixedUpdate()
    {
        switch(curState)
        {
            case State.AIMING:
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
                    curState = State.SHOOTING;
                    keyShoot = false;
                    power = 0;
                }

                powerup_used = false;
                break;
            case State.SHOOTING:
                Vector3 movement = new Vector3(Mathf.Sin(angle_rad), 0.0f, Mathf.Cos(angle_rad));
                power++;
                if(power > 100)
                {
                    power = 0;
                }
                powerText.text = "POW:" + power.ToString();
                if (keyShoot)
                {
                    rb.AddForce(movement * (power * speed));
                    curState = State.MOVING;
                    keyShoot = false;
                    shots++;
                }
                break;
            case State.MOVING:
                arrow.set_render(false);
                check_powerups();
                if(rb.velocity == new Vector3(0.0f, 0.0f, 0.0f))
                {
                    curState = State.AIMING;
                    keyShoot = false;
                }
                break;
        }

        keyShoot = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            winTextObject.text = "YOU FINISHED IN " + shots.ToString() + " SHOTS!";
            winTextObject.gameObject.SetActive(true);
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            Powerup powerupObject = other.gameObject.GetComponent<Powerup>();
            if (powerupObject != null)
            {
                powerup = powerupObject.powerup;
            }
            powerup_used = false;
        }
    }

    private void check_powerups()
    {
        switch(powerup)
        {
            case 1:
                if (keyShoot && rb.velocity.y == 0.0f && !powerup_used)
                {
                    rb.AddForce(new Vector3(0.0f, 600.0f, 0.0f));
                    keyShoot = false;
                    powerup_used = true;
                }
                color = BallColor.RED;
            break;

            case 2:
                if (keyShoot && !powerup_used)
                {
                    rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
                    rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
                    keyShoot = false;
                    powerup_used = true;
                }
                color = BallColor.PURPLE;
                break;

            case 3:
                if (keyShoot && !powerup_used && rb.velocity.y != 0.0f)
                {
                    rb.velocity = new Vector3(rb.velocity.x, -20.0f, rb.velocity.z);
                    keyShoot = false;
                    powerup_used = true;
                }
                color = BallColor.GREEN;
            break;

            default:
                color = BallColor.CYAN;
                break;
        }
    }

    private void switch_color()
    {
        switch(color)
        {
            case BallColor.RED:
                meshRenderer.material.color = Color.red;
                colorCircle.GetComponent<Image>().color = new Color32(200, 30, 30, 255);
                break;
            case BallColor.PURPLE:
                meshRenderer.material.color = Color.magenta;
                colorCircle.GetComponent<Image>().color = new Color32(200, 30, 200, 255);
                break;
            case BallColor.GREEN:
                meshRenderer.material.color = Color.green;
                colorCircle.GetComponent<Image>().color = new Color32(30, 200, 30, 255);
                break;
            default:
                meshRenderer.material.color = new Color32(0, 220, 255, 255);
                colorCircle.GetComponent<Image>().color = new Color32(0, 170, 200, 255);
                break;
        }
    }
}
