using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace shoot
{
    [System.Serializable]
    public class Boundary
    {
        public float xMin, xMax, zMin, zMax;

    }

    public class PlayerController : MonoBehaviour
    {
        public float speed;
        public float tiltAngle = 0.5f;
        public float rotationSpeed = 5.0f;

        public float fireRate = 0.5f;
        private float nextFire = 0.0f;

        public GameObject bullet;
        public Transform bulletSpawn;
        public Boundary boundary;
        public Rigidbody myRigidbody;
        private GameManager gameManager;
        public UnityEvent onShoot;


        /*public float smoothing = 5;
        private Vector3 smoothDirection; */


        void Start()
        {
            myRigidbody = GetComponent<Rigidbody>();
            gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
        }

        private void FixedUpdate()
        {

            //keyboard
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            //if movement key is pressed
            if (moveHorizontal != 0 || moveVertical != 0 && gameManager.gameOver == false)
            {
                myRigidbody.velocity = new Vector3(moveHorizontal, 0.0f, moveVertical) * speed;
            }
            else
            {
                myRigidbody.velocity = new Vector3(0f, 0f, 0f);
            }
            myRigidbody.position = new Vector3
               (
               Mathf.Clamp(myRigidbody.position.x, boundary.xMin, boundary.xMax),
               1.0f,
               Mathf.Clamp(myRigidbody.position.z, boundary.zMin, boundary.zMax)
               );


            EulerRotate();

            /* else MOUSE CONTROLS FOR LATER
             {
                 Vector3 pos = Input.mousePosition;
                 pos.z = Camera.main.transform.position.y + 1;
                 pos = Camera.main.ScreenToWorldPoint(pos);
                 Vector3 origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                 Vector2 currentPosition = new Vector3(pos.x, pos.z);
                 Vector3 directionRaw = pos - origin;
                 Debug.Log("directionRaw.magnitude=" + directionRaw.magnitude);

                 Vector3 direction = directionRaw.normalized;

                 smoothDirection = Vector3.MoveTowards(smoothDirection, direction, smoothing);

                 direction = smoothDirection;
                 Vector3 movement = new Vector3(direction.x, 0, direction.z);
                 myRigidbody.velocity = movement * speed;
             } */

            //movementborder

            // shoot controller


        }
        void EulerRotate()
        {
            float input = Input.GetAxis("Horizontal");
            Vector3 currAngles = transform.eulerAngles;
            myRigidbody.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(currAngles.x, currAngles.y, tiltAngle * input), Time.deltaTime * rotationSpeed);
        }
        void Update()
        {
            if ((Input.GetButton("Fire1") || Input.GetKeyDown(KeyCode.Space)) && Time.time > nextFire && gameManager.gameOver == false)
            {
                nextFire = Time.time + fireRate;
                Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);

                onShoot.Invoke();
            }
        }
    }
}
