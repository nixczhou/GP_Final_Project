using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //Self Setup
    Rigidbody rb;

    public float speed;
    public bool accumulative_state;
    private float holdDownStartTime;
    private float holdDownTime;

    //Player Identity
    public bool player1;
    
    private GameObject court;

    /*
        Game State
        -1 = freeState
        0 = rightfirstServe
        1 = rightSecondServe
        2 = leftFirstServe 
        3 = leftSecondServe
        4 = rightRecieving
        5 = leftRecieving
        
    */
    public int gameState = 0;
    public bool pointStart = true;
    public float force = 10.0f;

    //PowerBar
    public GameObject PowerBar;
    public float max_force = 18.0f;

    //HealthBar
    public GameObject HealthBar;

    //Charge
    private float charge = 0.0f;
    public float charge_amount = 0.2f;

    // Other Gameobjects
    public GameObject aim;
    public float aimSpeed;
    public GameObject ball;

    //Animation 
    Animator animator;
    protected int normalState;
    protected int forehandState;
    protected int backhandState;
    protected int servePrepState;
    protected int serveState;

    //Keys
    public string serveKey;
    public string forwardKey;
    public string backwardKey;
    public string rightKey;
    public string leftKey;
    public string powerKey;
    public string aimRightKey;
    public string aimLeftKey;
    public string[] specialAttackKeys;

    public int hitNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        court = GameObject.Find("Court");
        accumulative_state = false;
        animator = GetComponent<Animator>();

        //Initialize Animation States
        animationInitialization();

        //Initialize Charge Bar
        HealthBar.transform.GetChild(4).GetComponent<Image>().fillAmount = 0.0f;
    }

    void Update()
    {
        //re-position after a point finish
         if(pointStart){
            int playerDir = 1;
            if (!player1) {
                playerDir = -1;
            }
            float courtRight = court.GetComponent<GameScript>().courtRight;
            float courtLeft= court.GetComponent<GameScript>().courtLeft;
            float serveLine = court.GetComponent<GameScript>().serveLine;
            if(gameState == 0 || gameState == 1){
                aim.transform.position = new Vector3(-(courtRight - courtLeft)/4*playerDir, 0.1f, serveLine*playerDir);
                transform.position = new Vector3(0.5f * playerDir, -0.223f, court.GetComponent<GameScript>().baseline);
            }
            else if(gameState == 2 || gameState == 3){
                aim.transform.position = new Vector3((courtRight - courtLeft)/4*playerDir, 0.1f, serveLine*playerDir);
                transform.position = new Vector3(-0.5f * playerDir, -0.223f, court.GetComponent<GameScript>().baseline);
            }
            else if(gameState == 4){
                aim.transform.position = new Vector3(0.0f,0.1f,serveLine*playerDir);
                transform.position = new Vector3(5.58f * playerDir, -0.223f, -court.GetComponent<GameScript>().baseline * playerDir);
            }
            else if(gameState == 5){
                aim.transform.position = new Vector3(0.0f,0.1f,serveLine*playerDir);
                transform.position = new Vector3(-5.58f * playerDir, -0.223f, -court.GetComponent<GameScript>().baseline * playerDir);
            }
            pointStart = false;
        }

        //Animation State
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        
        //Serving 
        if(gameState >= 0 && gameState <= 3){
            serve();
        }
        characterMovement();      
        GetComponent<BoxCollider>().enabled = true;
        
        aimMovement();

        //Update Animation
        if(state.fullPathHash == forehandState) animator.SetBool("forehand", false);
        if(state.fullPathHash == backhandState) animator.SetBool("backhand", false);
        if(state.fullPathHash == servePrepState) animator.SetBool("servePrep", false);
        if(state.fullPathHash == serveState) animator.SetBool("serve", false);

        powerBar();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "ball")
        {
            if(hitNum != 0) GameObject.Find("ball").GetComponent<BallController>().ballState = -1;
            hitNum +=1;
            GameObject.Find("ball").GetComponent<BallController>().firstBounce = false;

            if(gameObject.name == "Player1") GameObject.Find("ball").GetComponent<BallController>().player1LastHit = true; 
            else GameObject.Find("ball").GetComponent<BallController>().player1LastHit = false;

            // Checking what type of Animation
            Vector3 ballDir = col.gameObject.transform.position - transform.position;
            if(ballDir.x >= 0){
                animator.SetBool("forehand", true);
            }
            else{
                animator.SetBool("backhand", true);
            }

            // Hitting direction
            Vector3 dir = aim.transform.position - gameObject.transform.position;
            col.gameObject.GetComponent<Rigidbody>().velocity = dir.normalized * force + new Vector3(0,6,0);

            //Reset the power
            force = 10.0f;
            PowerBar.transform.GetChild(2).GetComponent<Image>().fillAmount = 0.0f;

            //Gain charge
            charge = charge + charge_amount;
            HealthBar.transform.GetChild(4).GetComponent<Image>().fillAmount = Mathf.Min(1.0f,charge);
        }
    }

    protected float CalculateHoldDownForce(float holdTime){
        float maxForceHoldDownTime = 1.8f;
        float holdTimeNormalized = Mathf.Clamp01(holdTime/maxForceHoldDownTime);
        float power = holdTimeNormalized * max_force;
        return power;
    }

    protected void characterMovement(){
        int playerDir = 1;
        if (!player1) playerDir = -1;
        if(Input.GetKey(forwardKey)) gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speed * playerDir, Space.World);
        if(Input.GetKey(leftKey)) gameObject.transform.Translate(Vector3.left * Time.deltaTime * speed * playerDir, Space.World);
        if(Input.GetKey(backwardKey)) gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speed *-1 * playerDir, Space.World);
        if(Input.GetKey(rightKey)) gameObject.transform.Translate(Vector3.right * Time.deltaTime * speed * playerDir, Space.World);

        //Restrict from moving across the court
        if(player1 && transform.position.z >= -court.GetComponent<GameScript>().net){
            transform.position = new Vector3(transform.position.x, transform.position.y, -court.GetComponent<GameScript>().net);
        }
        else if(!player1 && transform.position.z <= court.GetComponent<GameScript>().net){
            transform.position = new Vector3(transform.position.x, transform.position.y, court.GetComponent<GameScript>().net);
        }

        //Serving Restriction
        if(gameState >= 0 && gameState <= 3){
            if(player1 && transform.position.z >= - court.GetComponent<GameScript>().baseline){
                transform.position = new Vector3(transform.position.x, transform.position.y, -court.GetComponent<GameScript>().baseline); 
            }
            else if(!player1 && transform.position.z <= court.GetComponent<GameScript>().baseline){
                transform.position = new Vector3(transform.position.x, transform.position.y, court.GetComponent<GameScript>().baseline); 
            }
            if(gameState == 0 || gameState == 1){
                if(player1 && transform.position.x <= 0.5f){
                    transform.position = new Vector3(0.5f, transform.position.y, transform.position.z);
                }
                else if(!player1 && transform.position.x >= -0.5f){
                    transform.position = new Vector3(-0.5f, transform.position.y, transform.position.z);
                }
            }
            else if(gameState == 2 || gameState == 3){
                if(player1 && transform.position.x >= -0.5f){
                    transform.position = new Vector3(-0.5f, transform.position.y, transform.position.z);
                }
                else if(!player1 && transform.position.x <= 0.5f){
                    transform.position = new Vector3(0.5f, transform.position.y, transform.position.z);
                }
            }
        }
    }

    protected void aimMovement(){
        // Aim cylinder Movement
        int playerDir = 1;
        if (!player1) playerDir = -1;
        if(Input.GetKey(aimLeftKey)) aim.transform.Translate(Vector3.left * Time.deltaTime * aimSpeed * playerDir, Space.World);
        if(Input.GetKey(aimRightKey)) aim.transform.Translate(Vector3.right * Time.deltaTime * aimSpeed * playerDir, Space.World);
    }

    protected void serve(){
        GetComponent<BoxCollider>().enabled = false;
        if(Input.GetKeyDown(serveKey)){
            animator.SetBool("servePrep", true);
            //start to record time
            holdDownStartTime = Time.time;
        }
        
        if(Input.GetKey(serveKey)){
            holdDownTime = Time.time - holdDownStartTime;
            PowerBar.transform.GetChild(2).GetComponent<Image>().fillAmount = CalculateHoldDownForce(holdDownTime)/max_force;
        }
        
        if(Input.GetKeyUp(serveKey)){
            //Initialize Ball
            holdDownTime = Time.time - holdDownStartTime;
            ball.transform.position = transform.position + new Vector3(0.0f, 2.0f, 0.0f);
            force = CalculateHoldDownForce(holdDownTime);
            animator.SetBool("serve", true);
            animator.SetBool("servePrep", false);

            // Hitting direction
            Vector3 dir = aim.transform.position - gameObject.transform.position;
            ball.GetComponent<Rigidbody>().velocity = dir.normalized * force + new Vector3(0,10,0);
            gameState = -1;
            holdDownTime = 0.0f;
        }
        
    }

    protected void powerBar(){
        if(Input.GetKey("space")){
            //update show force bar
            force += 0.2f;
            float bar_fill = force-10.0f;
            PowerBar = GameObject.FindWithTag("PowerBar1");
            PowerBar.transform.GetChild(2).GetComponent<Image>().fillAmount = bar_fill/max_force;
            if(bar_fill > max_force) force = 10.0f;
        }


        if(Input.GetKey("p")){
            //update show force bar
            force += 0.2f;
            float bar_fill = force-10.0f;
            PowerBar = GameObject.FindWithTag("PowerBar2");
            PowerBar.transform.GetChild(2).GetComponent<Image>().fillAmount = bar_fill/max_force;
            if(bar_fill > max_force) force = 10.0f;
        }
    }

    void animationInitialization(){

    }

}
