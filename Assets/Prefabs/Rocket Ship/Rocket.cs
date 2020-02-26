using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Range(0.0f, 100000.0f)] [SerializeField] private float thrust = 30000.0f;
    [Range(0.0f, 400.0f)] [SerializeField] private float turnSpeed = 80.0f;

    private Rigidbody rigidBody;

    [SerializeField] private GameObject mesh;

    private AudioSource audioSource;
    [SerializeField] private AudioClip SFX_engine;
    [SerializeField] private AudioClip SFX_explosion;
    [SerializeField] private AudioClip SFX_goal;

    private enum State { Alive, Dying, Transcending};
    private State state;

    [SerializeField] private ParticleSystem particles_engine;
    [SerializeField] private ParticleSystem particles_explosion;
    [SerializeField] private ParticleSystem particles_success;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = SFX_engine;
        audioSource.Play();
        audioSource.Pause();

        state = State.Alive;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            ProcessInput();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyMapping.Instance.d_nextLevel))
        {
            print("DEBUG - Next map");
            SceneManagement.Instance.NextScene();
        } 
        else if (Input.GetKey(KeyMapping.Instance.d_toggleCollision))
        {
            print("DEBUG - Disabled collision");
            GetComponent<Rigidbody>().detectCollisions = !GetComponent<Rigidbody>().detectCollisions;
        }
    }

    private void ProcessInput()
    {
        Boost();
        Rotate();
    }

    private void Boost()
    {
        // Boosting
        if (Input.GetKey(KeyMapping.Instance.boost))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrust * Time.deltaTime);
            particles_engine.Play();
            audioSource.UnPause();
        }
        else if (Input.GetKeyUp(KeyMapping.Instance.boost))
        {
            particles_engine.Stop();
            audioSource.Pause();
        }
    }

    private void Rotate()
    {
        // Remove rotation due to physics
        rigidBody.angularVelocity = Vector3.zero;

        // Turning left/right
        if (Input.GetKey(KeyMapping.Instance.turnLeft))
        {
            transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyMapping.Instance.turnRight))
        {
            transform.Rotate(-Vector3.forward * turnSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }

        switch(collision.collider.tag)
        {
            // Do nothing
            case "Friendly":
                break;

            // End stage
            case "Finish":
                if (state == State.Alive)
                {
                    StartCoroutine(FinishStage());
                }
                break;

            // Damage player
            default:
                if (state != State.Transcending)
                {
                    StartCoroutine(Death());
                }
                break;
        }
    }

    private IEnumerator FinishStage()
    {
        print("Goal!");
        state = State.Transcending;

        particles_success.Play();

        audioSource.Stop();
        audioSource.PlayOneShot(SFX_goal);

        yield return new WaitForSeconds(1.0f);

        SceneManagement.Instance.NextScene();
    }

    private IEnumerator Death()
    {
        print("Dead");
        state = State.Dying;

        particles_engine.Stop();

        audioSource.Stop();

        yield return new WaitForSeconds(0.5f);

        audioSource.PlayOneShot(SFX_explosion);
        particles_explosion.Play();
        Destroy(mesh);

        yield return new WaitForSeconds(1.0f);

        SceneManagement.Instance.LoadFirstScene();
    }
}
