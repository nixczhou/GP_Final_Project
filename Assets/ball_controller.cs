using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_controller : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    public Vector3 temp_position;
    public Vector3 direction;
    public ParticleSystem skill1;
    public ParticleSystem skill2;
    public bool fly_state;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        temp_position = new Vector3 (0.0f, 0.0f, 0.0f);
        direction = new Vector3 (0.0f, 0.0f, 0.0f);
        fly_state = false;
        skill1 = GameObject.Find("skill1").GetComponent<ParticleSystem>();
        skill2 = GameObject.Find("skill2").GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // if (Input.GetKey(KeyCode.K))
        // {
        //     ball_reinit_to_player1();
        // }
        // if (Input.GetKey(KeyCode.L))
        // {
        //     ball_reinit_to_player2();
        // }
        if (Input.GetKey(KeyCode.Q))
        {
            skill1.Play();
        }
        if (Input.GetKey(KeyCode.Keypad7))
        {
            skill2.Play();
        }
        direction = gameObject.transform.position - temp_position;
        temp_position = gameObject.transform.position;
    }
    void OnCollisionEnter (Collision other)
    {
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
