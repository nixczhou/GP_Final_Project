using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class player2_control : MonoBehaviour
{
    //Self Setup
    Rigidbody rb;
    public racket1_control racket;
    public float speed;
    public bool accumulative_state;
    private float holdDownStartTime;

    //Serving
    public bool serving = false; //if it is your turn to serve.
    public float force = 10.0f;

    // Other Gameobjects
    public GameObject aim;
    public float aimSpeed;
    public GameObject ball;

    //PowerBar
    public GameObject PowerBar;
    public float max_force = 18.0f;

    //HealthBar
    public GameObject HealthBar;

    //Charge
    private float charge = 0.0f;
    public float charge_amount = 0.2f;

    //Animation 
    Animator animator;
    private int normalState;
    private int forehandState;
    private int backhandState;
    private int servePrepState;
    private int serveState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        accumulative_state = false;
        animator = GetComponent<Animator>();

        //Initialize Animation States
        normalState = Animator.StringToHash("Base Layer.NormalStatus");
        forehandState = Animator.StringToHash("Base Layer.Forehand");
        backhandState = Animator.StringToHash("Base Layer.Backhand");
        serveState = Animator.StringToHash("Base Layer.Serve");

        //Initialize Charge Bar
        HealthBar.transform.GetChild(4).GetComponent<Image>().fillAmount = 0.0f;

    }

    private float CalculateHoldDownForce(float holdTime){
        float maxForceHoldDownTime = 1.8f;
        float holdTimeNormalized = Mathf.Clamp01(holdTime/maxForceHoldDownTime);
        float power = holdTimeNormalized * max_force;
        return power;
    }
    
    void FixedUpdate()
    {
        //Animation State
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        //Serving 
        if(serving){
            GetComponent<BoxCollider>().enabled = false;
            if(Input.GetKeyDown(KeyCode.E)){
                animator.SetBool("servePrep", true);
                //start to record time
                holdDownStartTime = Time.time;
                
            }

            if(Input.GetKey(KeyCode.E)){
                float holdDownTime = Time.time - holdDownStartTime;
                PowerBar.transform.GetChild(2).GetComponent<Image>().fillAmount = CalculateHoldDownForce(holdDownTime)/max_force;
            }
            
            if(Input.GetKeyUp(KeyCode.E)){
                //Initialize Ball
                float holdDownTime = Time.time - holdDownStartTime;
                ball.transform.position = transform.position + new Vector3(0.0f, 2.0f, 0.0f);
                force = CalculateHoldDownForce(holdDownTime);
                animator.SetBool("serve", true);
                animator.SetBool("servePrep", false);

                // Hitting direction
                Vector3 dir = aim.transform.position - gameObject.transform.position;
                ball.GetComponent<Rigidbody>().velocity = dir.normalized * force + new Vector3(0,10,0);
                serving = false;
            }
        }
        else{
            //Character Movement
            if(Input.GetKey(KeyCode.UpArrow)) gameObject.transform.Translate(Vector3.forward * Time.deltaTime * -1 * speed, Space.World);
            if(Input.GetKey(KeyCode.LeftArrow)) gameObject.transform.Translate(Vector3.left * Time.deltaTime* -1  *speed, Space.World);
            if(Input.GetKey(KeyCode.DownArrow)) gameObject.transform.Translate(Vector3.forward * Time.deltaTime *speed, Space.World);
            if(Input.GetKey(KeyCode.RightArrow)) gameObject.transform.Translate(Vector3.right * Time.deltaTime* -1  *speed, Space.World);
        }        
        GetComponent<BoxCollider>().enabled = true;
        // Aim cylinder Movement
        if(Input.GetKey(KeyCode.L)) aim.transform.Translate(Vector3.left * Time.deltaTime *aimSpeed, Space.World);
        if(Input.GetKey(KeyCode.K)) aim.transform.Translate(Vector3.right * Time.deltaTime *aimSpeed, Space.World);
        

        //Update Animation
        if(state.fullPathHash == forehandState) animator.SetBool("forehand", false);
        if(state.fullPathHash == backhandState) animator.SetBool("backhand", false);
        if(state.fullPathHash == servePrepState) animator.SetBool("servePrep", false);
        if(state.fullPathHash == serveState) animator.SetBool("serve", false);


        if(Input.GetKey(KeyCode.P)){
            //update show force bar
            force += 0.2f;
            float bar_fill = force-10.0f;
            PowerBar.transform.GetChild(2).GetComponent<Image>().fillAmount = bar_fill/max_force;
            if(bar_fill > max_force) bar_fill = 0.0f;
        }
    
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "ball")
        {
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
            col.gameObject.GetComponent<Rigidbody>().velocity = (dir.normalized * force + new Vector3(0,6,0));

            //Reset the power
            force = 10.0f;
            PowerBar.transform.GetChild(2).GetComponent<Image>().fillAmount = 0.0f;

            //Gain charge
            charge = charge + charge_amount;
            HealthBar.transform.GetChild(4).GetComponent<Image>().fillAmount = Mathf.Min(1.0f, charge);

        }
    }

    /*
    Rigidbody rb;
    public racket2_control racket;
    public float speed;
    public float angle;
    public bool accumulative_state;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        angle = 0.0f;
        accumulative_state = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = gameObject.transform.forward * speed;
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = -1*gameObject.transform.right * speed;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = -1*gameObject.transform.forward * speed;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = gameObject.transform.right * speed;
        }
        if(Input.GetKey(KeyCode.Keypad4))
        {
            accumulative_state = true;
            if (angle <= 50.0f)
            {
                gameObject.transform.Rotate(0.0f, 5.0f, 0.0f, Space.Self);
                angle +=5.0f;
            }
        }
        else
        {
            accumulative_state = false;
            if (angle > 0.0f)
            {
                StartCoroutine(Delay());
            }
        }
        gameObject.transform.rotation = Quaternion.Euler(0.0f, gameObject.transform.rotation.eulerAngles.y, 0.0f); 
        rb.angularVelocity = Vector3.zero;
    }
    IEnumerator Delay()
    {
        while (angle > 0.0f)
        {
            gameObject.transform.Rotate(0.0f, -5.0f, 0.0f, Space.Self);
            angle -= 5.0f;
            yield return new WaitForSeconds(0.5f);
        }
    }
    */
}
