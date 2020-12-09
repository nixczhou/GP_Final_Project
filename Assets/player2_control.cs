using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player2_control : MonoBehaviour
{
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
}
