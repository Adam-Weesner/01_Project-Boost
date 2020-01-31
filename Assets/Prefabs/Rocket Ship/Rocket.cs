using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float thrust = 30000.0f;
    public float turnSpeed = 80.0f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
        audioSource.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        
    }

    private void ProcessInput()
    {
        // Boosting
        if (Input.GetKeyDown(KeyMapping.Instance.boost))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrust * Time.deltaTime);
            audioSource.UnPause();
        } 
        else if (Input.GetKeyUp(KeyMapping.Instance.boost))
        {
            audioSource.Pause();
        }

        // Turning left/right
        if (Input.GetKey(KeyMapping.Instance.turnLeft))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * turnSpeed);
        } else if (Input.GetKey(KeyMapping.Instance.turnRight))
        {
            transform.Rotate(-Vector3.forward * Time.deltaTime * turnSpeed);
        }
    }
}
