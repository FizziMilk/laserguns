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
        public UnityEvent coinPickup;

        [Range(0f, 50f)]
        [Tooltip("Coin pickup radius")]
        public float coinRadius = 15f;


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
        private void OnTriggerEnter(Collider other)
        {
            if (CompareTag("GoldCoin"))
            {
                coinPickup.Invoke();
                Destroy(other.gameObject);
            }
        }
    }
}
