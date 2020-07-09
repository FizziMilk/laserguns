using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinPlosion : MonoBehaviour
{

    public GameObject goldCoin;
    public int count = 3;

    public float explosionForce = 3f;
    public float explosionRadius = 3f;

    public float offset = 0.5f;
    public UnityEvent onExplode;
    public float detectionRadius;
    public GameObject player;
    
    private Rigidbody rb;
    private Rigidbody playerRb;

    [Range(0f, 50f)]
    public float speed; 





    public void Start()
    {
       
        
    }

    public void Explode(Vector3 at)
    {
        onExplode.Invoke();

        for (int i = 0; i < count; i++)
        {
            var coin = Instantiate(goldCoin, at, goldCoin.transform.rotation);
            var rb = coin.GetComponent<Rigidbody>();

            var pos = new Vector3(
                at.x + Random.Range(-offset, offset),
                at.y,
                at.z + Random.Range(-offset, offset)
            );

            rb.AddExplosionForce(explosionForce, pos, explosionRadius);
        }


    }
    private bool IsInSight(Vector3 from, Vector3 target)
    {
        if(Vector3.Distance(from, target) > detectionRadius)
        {
            return false;
        }
        return true;
    }

    private Vector3 LineToPlayer(Vector3 from, Vector3 to)
    {
        var playerDirection = (from - to).normalized;
        return playerDirection;
    }

  

    private void FixedUpdate()
    {
        rb = goldCoin.GetComponent<Rigidbody>();
        var position = rb.position;
        playerRb = player.GetComponent<Rigidbody>();
        var playerPosition = playerRb.position;

           rb.position = Vector3.MoveTowards(position, playerPosition, speed * Time.deltaTime);
        




    }

    
}
