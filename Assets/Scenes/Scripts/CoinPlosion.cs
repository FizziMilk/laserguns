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
}
