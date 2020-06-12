using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float speed;
    float smooth = 1.5f;
    int MaxDist = 15;
    int MinDist = 15;

    public Transform Player;
    private GameManager gameManager;
    public float angleBetween;
    public GameObject coin;






    // Start is called before the first frame update
    void Start()
    {

       
        gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
        


    }


    // Update is called once per frame
    void Update()
    {

        Player = GameObject.FindWithTag("Player").transform;

        if (Vector3.Distance(transform.position, Player.position) >= MaxDist && gameManager.gameOver == false)
        {
            transform.position += transform.forward * speed * Time.deltaTime;


        }
        else if (Vector3.Distance(transform.position, Player.position) <= MinDist && gameManager.gameOver == false)
        {
            float angleBetween = Vector3.SignedAngle(transform.position, Player.transform.position, Vector3.forward);
            float tiltAroundZ = Mathf.Clamp(angleBetween, -60, 60);
            float tiltAroundY = Mathf.Clamp(angleBetween, -50, 50);

            if (angleBetween <= 60)
            {
                Quaternion target = Quaternion.Euler(0, tiltAroundY, tiltAroundZ);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
            }
            if (transform.position.y != 1)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0.85f, 1.15f), transform.position.z);
            }
            transform.position += transform.forward * speed * Time.deltaTime;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            gameManager.health -= 1;
            gameManager.HealthTracker();

        }
        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);



        }
    }

}
