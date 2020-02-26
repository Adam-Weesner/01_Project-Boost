using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movement = new Vector3(10.0f, 10.0f, 10.0f);

    // 0 for not moved, 1 for fully moved
    private float movementFactor;

    // Time it takes to complete 1 cycle
    [Range(0.01f, 100.0f)] [SerializeField] private float period = 2.0f;

    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSin = Mathf.Sin(cycles * tau);

        movementFactor = (rawSin / 2.0f) + 0.5f;

        Vector3 offset = movement * movementFactor;
        transform.position = startingPos + offset;
    }
}
