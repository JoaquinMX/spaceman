using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    healPotion,
    manaPotion,
    money
}
public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.money;

    private SpriteRenderer sprite;

    private CircleCollider2D itemCollider;

    private bool hasBeenCollected = false;

    public int value = 1;

    private GameObject player;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Show()
    {
        sprite.enabled = true;
        itemCollider.enabled = true;
        hasBeenCollected = false;
    }

    void Hide()
    {
        sprite.enabled = false;
        itemCollider.enabled = false;
    }

    void Collect()
    {
        Hide();
        hasBeenCollected = true;

        switch (this.type)
        {
            case CollectableType.money:
                GameManager.sharedInstance.CollectObject(this);
                GetComponent<AudioSource>().Play();
                break;
            case CollectableType.healPotion:
                player.GetComponent<PlayerController>().CollectHealth(this.value);
                break;
            case CollectableType.manaPotion:
                player.GetComponent<PlayerController>().CollectMana(this.value);
                break;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Collect();
        }
    }
}
