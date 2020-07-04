using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent allows you to specify what the script needs to function. In this case, the script won't work if the
// Spaceship doesn't have a Rigidbody and a Collider of some kind with triggers.

[RequireComponent(typeof(Rigidbody), typeof(Collider))]

public class AIController : MonoBehaviour
{
    [Range(0f, 100f)]
    [Tooltip("Angular turn speed of the spaceship")]
    public float turnSpeed = 10f;

    [Range(0f, 100f)]
    [Tooltip("Linear flight speed of the spaceship")]
    public float flySpeed = 10f;

    [Range(0f, 50f)]
    [Tooltip("Player detection radius within a sphere")]
    public float detectionRadius = 15f;

    [Range(0f, 90f)]
    [Tooltip("Max tilting angle when turning towards the player")]
    public float maxTiltAngle = 30f;

    [Range(0f, 90f)]
    [Tooltip("Max turning angle towards the player")]
    public float maxTurnAngle;

    public GameManager gameManager;
    public GameObject player;

    private Rigidbody body;





    // Start is called before the first frame update
    private void Start()
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
    }

    private void OnDrawGizmos()
    //detection radius debug
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // <returns> angle between given positions.</returns>
    private static float AngleBetween(Vector3 from, Vector3 to)
    {
        return Vector3.Angle(Vector3.forward, from - to);
    }

    // <returns> True if the target is visible from given position. </returns>
    private bool IsInSight(Vector3 from, Vector3 target)
    {
        // Is the target outside of detection range?
        if (Vector3.Distance(from, target) > detectionRadius)
        {
            return false;
        }

        // Is the target outside of the turn angle?
        if (AngleBetween(from, target) > maxTurnAngle)
        {
            return false;
        }

        return true;
    }
    /// <param name="from">Position to rotate from.</param>
    /// <param name="to">Position to rotate to.</param>
    /// <returns>Rotation aimed towards target position.</returns>
    private Quaternion GetTargetedRotation(Vector3 from, Vector3 to)
    {
        //Find the direction vector towards the target.
        var playerDirection = (from - to).normalized;
        //Find the rotation towards the target.
        var lookRotation = Quaternion.LookRotation(playerDirection);

        //Add tilting.
        var eulerRotation = lookRotation.eulerAngles;

        // Use angle between the player and the enemy for tilting. Also make sure it doesn't go above our set limit.
        var angleBetween = Mathf.Min(AngleBetween(from, to), maxTiltAngle);

        // Angle between player and enemy is always positive, the direction can be set by using quaternion angle sign.
        var zRotation = Mathf.Sign(lookRotation.y) * angleBetween;

        eulerRotation.z = zRotation;
        eulerRotation.x = 0;

        return Quaternion.Euler(eulerRotation);
    }



    /// <param name="rotation"> Rotation to relax.</param>
    /// <returns>Relaxed rotation.</returns>
    private Quaternion GetRelaxedRotation(Quaternion rotation)
    {
        var eulerRotation = rotation.eulerAngles;

        // Relaxed flight rotation is current rotation, except with nullified z axis rotation.
        eulerRotation.z = 0;
        return Quaternion.Euler(eulerRotation);

    }
    private Quaternion GetFlightRotation()
    {
        // Don't perform body.position, transform.position all the time - it is costly. Save them to variables and re-use them instead.

        var position = body.position;
        var rotation = body.rotation;

        var playerPosition = player.transform.position;
        var newRotation = IsInSight(position, playerPosition)

            // If player is in sight, rotate towards him.
            ? GetTargetedRotation(position, playerPosition)

            // Otherwise, stay on current course, but remove tilting.
            : GetRelaxedRotation(rotation);

        // Smoothly transition to new rotation.
        return Quaternion.Slerp(rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    /// <returns>
    /// New flight position.
    /// </returns>

    private Vector3 GetFlightPosition()
    {
        return body.position - body.transform.forward * (flySpeed * Time.deltaTime);
    }
    //If you're doing physics calculations ( in this case moving spaceships). Do it in FixedUpdate. Update runs as fast
    // as your PC can chug out frames, FixedUpdate runs only once per physics tick.
    private void FixedUpdate()
    {
        // This script cannot function (fully) if one of those variables is not set.
        if (gameManager == null || player == null)
        {
            return;
        }

        //Don't move Transform directily, move physics body (Rigidbody) instead. This way physics wil keep up to date
        // with your transformations and will be more accurate.
        body.MoveRotation(GetFlightRotation());
        body.MovePosition(GetFlightPosition());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            gameManager.health -= 1;
            gameManager.HealthTracker();

            Destroy(gameObject);

        }
        if (other.GetComponent<BulletController>() != null)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);



        }
    }

}
