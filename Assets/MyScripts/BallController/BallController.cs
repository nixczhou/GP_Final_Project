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

    public GameObject blackhole_1;
    public GameObject blackhole_2;

    public ParticleSystem skill1;
    public ParticleSystem skill2;

    float skill2_start_time = 0.0f;
    float skill2_current_time = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        blackhole_1 = GameObject.Find("blackhole_1");
        blackhole_2 = GameObject.Find("blackhole_2");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player1")
        {
            player1LastHit = true;
            firstBounce = false;
        }
        else if (other.gameObject.name == "Player2")
        {
            player1LastHit = false;
            firstBounce = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "blackhole")
        {
            if (other.gameObject.name == "blackhole_1" && player1LastHit == true)
            {
                gameObject.transform.position = blackhole_2.transform.position;
            }
            else if (other.gameObject.name == "blackhole_2" && player1LastHit == false)
            {
                gameObject.transform.position = blackhole_1.transform.position;
            }
        }
        skill2_current_time = Time.time;
        if (other.name == "Player2" && player1LastHit == false)
        {
            skill1.Clear();
            skill1.Stop();
        }
        else if (other.name == "Player1" && player1LastHit == true)
        {
            if (skill2_current_time - skill2_start_time >= 30.0f)
            {
                skill2.Clear();
                skill2.Stop();
            }
        }
    }

    public void ball_reinit_to_player1()
    {
        gameObject.transform.position = new Vector3(1.76f, 1.8f, -9.49f);
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
    public void ball_reinit_to_player2()
    {
        gameObject.transform.position = new Vector3(-1.7f, 1.8f, 5.0f);
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
    public void add_force_to_ball(float force, Vector3 direction)
    {
        rb.AddForce((direction * 100.0f + new Vector3(0.0f, 10.0f, 0.0f)) * force);
    }
    public void ball_skill1()
    {
        skill1.Play();
    }
    public void ball_skill2()
    {
        skill2.Play();
        skill2_start_time = Time.time;
    }

}
