using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameOver = false;
    public int health = 3;
    public GameObject gameOverText;
    public GameObject fullHeart1;
    public GameObject fullHeart2;
    public GameObject fullHeart3;
    public GameObject restartButton;
    public GameObject player;
    
    

   
    // Start is called before the first frame update
    void Start()
    {
    
     
    }

    // Update is called once per frame
    void Update()
    {
        HealthTracker();
    }

   public void HealthTracker()
    {
        if (health > 3)
        {
            health = 3;
        }
        switch (health)
        {
            case 3:
                fullHeart1.gameObject.SetActive(true);
                fullHeart2.gameObject.SetActive(true);
                fullHeart3.gameObject.SetActive(true);
                break;
            case 2:
                fullHeart1.gameObject.SetActive(false);
                fullHeart2.gameObject.SetActive(true);
                fullHeart3.gameObject.SetActive(true);
                break;
            case 1:
                fullHeart1.gameObject.SetActive(false);
                fullHeart2.gameObject.SetActive(false);
                fullHeart3.gameObject.SetActive(true);
                break;
            case 0:
                fullHeart1.gameObject.SetActive(false);
                fullHeart2.gameObject.SetActive(false);
                fullHeart3.gameObject.SetActive(false);
                GameOver();
                break;
        }
    }
    void GameOver()
    {
      
            gameOver = true;
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
        
    }
    void SpawnGoldCoins()
    {

    }
    public void Restart()
    {
        SceneManager.LoadScene("shoot");
    }
}

