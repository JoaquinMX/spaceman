using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Se llama cuando un collider entra en otro
    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            PlayerController controller = collision.GetComponent<PlayerController>();
            controller.Die();
        }
    }
    //Se llama mientras un objeto  esta dentro de otro como caminar por fuego.
    //OnTriggerStay2D
    //Cuando un personaje sale de un collider
    //OnTriggerExit2D
}
