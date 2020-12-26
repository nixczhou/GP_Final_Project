using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Rigidbody rb;

    //firstBounce
    public bool firstBounce = false;
    public bool player1LastHit = true;
    /*
        ballState (Same As GameState)
        0 = rightfirstServe
        1 = rightSecondServe
        2 = leftFirstServe 
        3 = leftSecondServe
        -1 = notServing
    */
    public int ballState;

    public ParticleSystem skill1;
    public ParticleSystem skill2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        skill1 = GameObject.Find("skill1").GetComponent<ParticleSystem>();
        skill2 = GameObject.Find("skill2").GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            skill1.Play();
        }
        if (Input.GetKey(KeyCode.Keypad7))
        {
            skill2.Play();
        }
    }
    void OnCollisionEnter (Collision other)
    {
        if(other.gameObject.name == "Player1"){
            player1LastHit = true;
            firstBounce = false;
        } 
        else if(other.gameObject.name == "Player2"){
            player1LastHit = false;
            firstBounce = false;
        }
        /*
        if(other.gameObject.name=="player1" || other.gameObject.name=="player2")
        {
            if (skill1.isPlaying)
            {
                skill1.Stop();
            }
            if (skill2.isPlaying)
            {
                skill2.Stop();
            }
        }
        */
    }

    public void ball_reinit_to_player1()
    {
        gameObject.transform.position = new Vector3(1.76f, 1.8f, -9.49f);
        rb.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
    }
    public void ball_reinit_to_player2()
    {
        gameObject.transform.position = new Vector3(-1.7f, 1.8f, 5.0f);
        rb.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
    }
    public void add_force_to_ball(float force, Vector3 direction)
    {
        rb.AddForce((direction*100.0f + new Vector3(0.0f, 10.0f, 0.0f))*force);
    }
}
