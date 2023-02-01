using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController sharedInstance;
    //Variables del Movimiento del Personaje
    public float jumpForce = 6f;
    public float runningSpeed = 5f;
    //Nos permite guardar en una variable la caracteristica de RigidBody de nuestro personaje
    private Rigidbody2D playerRigidBody;
    //Esto nos permite cambiar como se ve el personaje en el juego, que sprite va a usar
    SpriteRenderer spriteRenderer;
    Animator animator;

    Vector3 startPosition;

    const string STATE_ALIVE = "isAlive";
    const string STATE_ON_THE_GROUND = "isOnTheGround";
    private const string VERTICAL_FORCE = "verticalForce";
    [SerializeField]
    private int healthPoints, manaPoints;

    public const int INITIAL_HEALTH = 100,
        INITIAL_MANA = 15,
        MAX_HEALTH = 200,
        MAX_MANA = 30,
        MIN_HEALTH = 10,
        MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;

    public float jumpRaycastDistance = 1.5f;

    static Vector2 gravity;

    public LayerMask groundMask;
    //Awake preconfigura todo antes de que empiece, seria como el cuadro 0.
    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(sharedInstance == null) {
            sharedInstance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
    }
    public void StartGame() {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, false);
        animator.SetFloat(VERTICAL_FORCE, 0f);
        Invoke("RestartPosition", 0.2f);

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

    }


    void RestartPosition() {
        this.transform.position = startPosition;
        //Reinicia la velocidad del player ya que al morir tiene mucha velocidad en vertical
        this.playerRigidBody.velocity = Vector2.zero;
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }
    // Update is called once per frame
    void Update()
    {
        Jump();

        animator.SetBool(STATE_ON_THE_GROUND, isTouchingTheGround());
        animator.SetFloat(VERTICAL_FORCE, playerRigidBody.velocity.y);

        //Permite hacer debug de la distancia que se utiliza para que no se repita el salto
        Debug.DrawRay(this.transform.position, Vector2.down*jumpRaycastDistance, Color.red);
    }
    void FixedUpdate()
    {
        if(GameManager.sharedInstance.currentGameState == GameState.inGame){
            characterDirection();
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                
                if(playerRigidBody.velocity.x < runningSpeed) {
                    playerRigidBody.velocity = new Vector2(runningSpeed, playerRigidBody.velocity.y);
                }
            }
            else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
                if(playerRigidBody.velocity.x < runningSpeed) {
                    playerRigidBody.velocity = new Vector2(runningSpeed*-1, playerRigidBody.velocity.y);
                }
            }
            else {
                playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
            }
        }
        else {//Si no estamos dentro de la partida no hay movimiento
            playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
        }
    }

    void Jump()
    {
        float jumpForceFactor = jumpForce;
        //Permite realizar el salto
        //en ForceMode2D. se pueden poner 2 valores, Impulse para que sea toda la fuerza de golpe o Force para que sea constante
        if(GameManager.sharedInstance.currentGameState == GameState.inGame){
            if (Input.GetButtonDown("Jump")) {
                if(isTouchingTheGround())
                {
                    GetComponent<AudioSource>().Play();
                    playerRigidBody.AddForce(Vector2.up*jumpForceFactor, ForceMode2D.Impulse);
                }
            }
            if (Input.GetButtonDown("SuperJump"))
            {
                jumpForceFactor *= SUPERJUMP_FORCE;
                    if (isTouchingTheGround())
                    {
                        if (manaPoints >= SUPERJUMP_COST)
                        {
                            manaPoints -= SUPERJUMP_COST;
                            GetComponent<AudioSource>().Play();
                            playerRigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
                            jumpForceFactor = 6;
                        }
                    }


            }
            
            
        }
            
        
    }
    //Metodo que nos indica si el personaje esta o no tocando el suelo
    bool isTouchingTheGround() { 
        //this.transform.position nos da la posicion de nuestro personaje
        //Vector2 nos da el eje Y y down nos dice que queremos el rayo hacia abajo, .2f es la cantidad de unidades que va a medir el rayo y groundMask cual es la capa que analiza.
        if(Physics2D.Raycast(this.transform.position, Vector2.down, jumpRaycastDistance, groundMask)) {
            // animator.enabled = true;
            return true;
            
        }
        else {
            // animator.enabled = false;
            return false;
        }
    }

    void characterDirection()
    {
        if(playerRigidBody.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void Die()
    {
        float travelledDistance = GetTravelDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore", 0f);
        if (travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }
        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if (this.healthPoints >= MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }

        if (this.healthPoints <= 0)
        {
            Die();
        }
    }

    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if (this.healthPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public int GetMana()
    {
        return manaPoints;
    }

    public float GetTravelDistance()
    {
        return this.transform.position.x - startPosition.x;
    }
}