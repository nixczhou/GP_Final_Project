using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1_control : MonoBehaviour
{
    //Self Setup
    Rigidbody rb;
    public racket1_control racket;
    public float speed;
    public bool accumulative_state;

    //Serving
    public bool serving = true; //if it is your turn to serve.
    public float force = 10.0f;

    // Other Gameobjects
    public GameObject aim;
    public float aimSpeed;
    public GameObject ball;

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
                
            }
            else if(Input.GetKeyUp(KeyCode.E)){
                //Initialize Ball
                ball.transform.position = transform.position + new Vector3(0.0f, 2.0f, 0.0f);
                force = 15.0f;
                animator.SetBool("serve", true);
                animator.SetBool("servePrep", false);

                // Hitting direction
                Vector3 dir = aim.transform.position - gameObject.transform.position;
                ball.GetComponent<Rigidbody>().velocity = dir.normalized * force + new Vector3(0,10,0);
                force = 10.0f;
                serving = false;
            }
        }
        else{
            //Character Movement
            if(Input.GetKey(KeyCode.W)) gameObject.transform.Translate(Vector3.forward * Time.deltaTime *speed, Space.World);
            if(Input.GetKey(KeyCode.A)) gameObject.transform.Translate(Vector3.left * Time.deltaTime *speed, Space.World);
            if(Input.GetKey(KeyCode.S)) gameObject.transform.Translate(Vector3.forward * Time.deltaTime *speed *-1, Space.World);
            if(Input.GetKey(KeyCode.D)) gameObject.transform.Translate(Vector3.right * Time.deltaTime *speed, Space.World);
        }        
        GetComponent<BoxCollider>().enabled = true;
        // Aim cylinder Movement
        if(Input.GetKey(KeyCode.F)) aim.transform.Translate(Vector3.left * Time.deltaTime *aimSpeed, Space.World);
        if(Input.GetKey(KeyCode.G)) aim.transform.Translate(Vector3.right * Time.deltaTime *aimSpeed, Space.World);
        

        //Update Animation
        if(state.fullPathHash == forehandState) animator.SetBool("forehand", false);
        if(state.fullPathHash == backhandState) animator.SetBool("backhand", false);
        if(state.fullPathHash == servePrepState) animator.SetBool("servePrep", false);
        if(state.fullPathHash == serveState) animator.SetBool("serve", false);
    
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
            col.gameObject.GetComponent<Rigidbody>().velocity = dir.normalized * force + new Vector3(0,6,0);
            
        }
    }

    IEnumerator initalizeBall(){
        yield return new WaitForSeconds(10);
    }

}
