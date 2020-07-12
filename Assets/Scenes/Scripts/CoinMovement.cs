using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    [Range(0f, 50f)]
    [Tooltip("Outer detection radius within a sphere")]
    public float outerDetectionRadius = 15f;
    [Range(0f, 50f)]
    [Tooltip("Inner detection radius")]
    public float detectionRadius = 5f;

    [Range(0f, 100f)]
    [Tooltip("Flight speed of the coin")]
    public float flySpeed = 10f;

    public GameManager gameManager;
    public GameObject player;

    private float timeSpeed;
    private Rigidbody body;



    // Start is called before the first frame update
    void Start()
    {
        // If something is not set, throw error
        if (gameManager == null)
        {
            throw new UnityException("Game manager is not assigned");
        }


        if (player == null)
        {
            throw new UnityException("Player is not assigned");
        }

        // No need to check for Rigidbody nullability, it will never be null as its specified in RequireComponent
        body = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        player = GameObject.Find("Player");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        var position = body.position;
        var playerPosition = player.transform.position;
        Vector3 difference = playerPosition - position;
        float distance = Vector3.Distance(position, playerPosition);

        if (distance < detectionRadius)
        {
            timeSpeed = flySpeed * 4 * Time.deltaTime;
            body.AddForce(difference * timeSpeed, ForceMode.Impulse);
            body.angularDrag = 4.0f;
        }
      else if (distance < outerDetectionRadius && distance > detectionRadius)
        {
            timeSpeed = flySpeed * 2 * Time.deltaTime;
            body.AddForce(difference * timeSpeed, ForceMode.Impulse);
            
        }
        else
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            gameManager.CoinPickup(1);
            Destroy(gameObject);

        }
    }
}

