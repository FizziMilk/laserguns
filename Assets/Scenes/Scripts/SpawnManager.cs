using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float spawnRangeX = 12;
    private float spawnPosZ = 16;
    private float startDelay = 2;
    private float spawnInterval = 1.5f;

    public GameObject[] Enemies;
    public GameManager gameManager;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void SpawnEnemy()
    {
        if (gameManager.gameOver == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 1, spawnPosZ);
            var enemy = Instantiate(Enemies[0], spawnPos, Enemies[0].transform.rotation);
            var ai = enemy.GetComponent<AIController>();

            ai.gameManager = gameManager;
            ai.player = player;
        }
    }
}
