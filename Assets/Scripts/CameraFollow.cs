using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject follow;

    private void Start()
    {
        follow = FindObjectOfType<Rocket>().gameObject;
    }

    private void Update()
    {
        if (follow)
        {
            transform.position = new Vector3(
                follow.transform.position.x, 
                follow.transform.position.y, 
                follow.transform.position.z);
        }
    }
}
