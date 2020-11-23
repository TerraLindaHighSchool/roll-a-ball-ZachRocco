using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player.GetComponent<Rigidbody>().velocity == new Vector3(0.0f, 0.0f, 0.0f))
        {
            float angle = 0.0f;
            if (Keyboard.current.qKey.isPressed)
            {
                angle--;
            }
            if (Keyboard.current.eKey.isPressed)
            {
                angle++;
            }
            transform.RotateAround(player.transform.position, Vector3.up, angle);
            offset = transform.position - player.transform.position;
        }
        else
        {
            transform.position = player.transform.position + offset;
        }
    }
}
