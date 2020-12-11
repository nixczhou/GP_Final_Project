using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1_control : MonoBehaviour
{
    Rigidbody rb;
    public racket1_control racket;
    public float speed;
    public float angle;
    public bool accumulative_state;
    public bool serving = true;
    
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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if(Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime *speed, Space.World);
            
            //rb.velocity = gameObject.transform.forward * speed;
        }
        if(Input.GetKey(KeyCode.A))
        {
             gameObject.transform.Translate(Vector3.left * Time.deltaTime *speed, Space.World);
        }
        if(Input.GetKey(KeyCode.S))
        {
             gameObject.transform.Translate(Vector3.forward * Time.deltaTime *speed *-1, Space.World);
        }
        if(Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(Vector3.right * Time.deltaTime *speed, Space.World);
        }
        if(Input.GetKey(KeyCode.G))
        {
            accumulative_state = true;
            if (angle <= 140.0f)
            {
                gameObject.transform.Rotate(0.0f, 10.0f, 0.0f, Space.Self);
                angle +=10.0f;
            }
        }
        else
        {
            accumulative_state = false;
            
            if (angle > 30.0f)
            {
                StartCoroutine(ForehandDelay());
            }
            
        }
        if(Input.GetKey(KeyCode.F))
        {
            accumulative_state = true;
            if (angle >= -140.0f)
            {
                gameObject.transform.Rotate(0.0f, -10.0f, 0.0f, Space.Self);
                angle -=10.0f;
            }
        }
        else
        {
            accumulative_state = false;
            
            if (angle < -30.0f)
            {
                StartCoroutine(BackhandDelay());
            }
            
        }
        gameObject.transform.rotation = Quaternion.Euler(0.0f, gameObject.transform.rotation.eulerAngles.y, 0.0f); 
        rb.angularVelocity = Vector3.zero;
    }

    IEnumerator ForehandDelay()
    {
        while (angle > 0.0f)
        {
            gameObject.transform.Rotate(0.0f, -10.0f, 0.0f, Space.Self);
            angle -= 10.0f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator BackhandDelay()
    {
        while (angle < 0.0f)
        {
            gameObject.transform.Rotate(0.0f, 10.0f, 0.0f, Space.Self);
            angle += 10.0f;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
