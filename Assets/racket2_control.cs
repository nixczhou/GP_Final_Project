using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class racket2_control : MonoBehaviour
{
    public Rigidbody rb;
    public player2_control player2;
    public Vector3 temp_position;
    public Vector3 direction;
    public float force;
    public bool release_state = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        temp_position = new Vector3 (0.0f, 0.0f, 0.0f);
        direction = new Vector3 (0.0f, 0.0f, 0.0f);
        force = 0.0f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Keypad4))
        {
            force += 5.0f;
        }
        else
        {
            StartCoroutine(Delay());
        }
        if(Input.GetKeyUp(KeyCode.Keypad4))
        {
            release_state = true;
        }
        direction = gameObject.transform.position - temp_position;
        temp_position = gameObject.transform.position;
    }
    void OnCollisionEnter (Collision collision)
    {
        if(collision.gameObject.name=="ball" && release_state == true)
        {
            ball_controller ball = collision.gameObject.GetComponent<ball_controller>();
            ball.add_force_to_ball(force, direction);
            ball.fly_state = true;
        }
    }
    IEnumerator Delay()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.5f);
        force = 0.0f;
    }
}
