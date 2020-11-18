using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public MeshRenderer mesh;
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 location = new Vector3(player.transform.position.x + Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180), player.transform.position.y, player.transform.position.z + Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180));
        //Vector3 location = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        transform.position = location;
    }

    public void set_render(bool render)
    {
        if(render)
        {
            mesh.enabled = true;
        }
        else
        {
            mesh.enabled = false;
        }
    }
}
