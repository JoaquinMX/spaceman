using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int enemyDamage = 10;
    
    public float runningSpeed = 4f;

    private Rigidbody2D rigidBody;

    public bool facingRight = false;

    private Vector3 startPosition;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        //this.transform.position = startPosition;
        
    }

    private void FixedUpdate()
    {
        GetComponent<AudioSource>().Play();
        float currentRunningSpeed = runningSpeed;

        if (facingRight)
        {
            //Mirando hacia la derecha
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            //Mirando hacia la izquierda
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = Vector3.zero;
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            rigidBody.velocity = new Vector2(currentRunningSpeed, rigidBody.velocity.y);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Collectable")
        {
            return;
        }

        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().CollectHealth(-enemyDamage);
            return;
        }
        //Si llegamos aqui, no hemos chocado ni con monedas ni con players, lo mas normal es que aqui haya otro enemigo o bien escenario
        //Vamos a hacer que el enemigo rote
        facingRight = !facingRight;
    }
}
