using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_control : MonoBehaviour
{
    ParticleSystem skill_effect;
    public BallController ball;
    public string skill_key;
    // Start is called before the first frame update
    void Start()
    {
        skill_effect = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(skill_key)) 
        {
            skill_effect.Play();
            if (transform.parent.name == "Player1")
            {
                ball.ball_skill1();
            }
            else if (transform.parent.name == "Player2")
            {
                ball.ball_skill2();
            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "ball")
        {
            skill_effect.Stop();
        }
    }
}
