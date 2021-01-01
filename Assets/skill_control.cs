using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_control : MonoBehaviour
{
    ParticleSystem skill_effect_on_player;
    ParticleSystem skill3_effect;
    public string skill_key;
    public BallController ball;
    public field_skill_control field;
    float skill3_current_time = 0.0f;
    float skill3_start_time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("ball").GetComponent<BallController>();
        field = GameObject.Find("Court").GetComponent<field_skill_control>();
        skill_effect_on_player = transform.Find("skill_effect").GetComponent<ParticleSystem>();
        skill_effect_on_player.Stop();
        skill3_effect = transform.Find("skill3").GetComponent<ParticleSystem>();
        skill3_effect.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        skill3_current_time = Time.time;
        if(Input.GetKey(skill_key)) 
        {
            if (transform.name == "Player_1")
            {
                skill_effect_on_player.Play();
                ball.ball_skill(transform.parent.name);
            }
            else if  (transform.name == "Player_2")
            {
                field.field_skill(transform.parent.name);
            }
            else if  (transform.name == "Player_3")
            {
                skill3_start_time = Time.time;
                skill3_effect.Play();
            }
        }
        if (skill3_current_time - skill3_start_time >= 30.0f)
        {
            skill3_effect.Clear();
            skill3_effect.Stop();
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "ball")
        {
            skill_effect_on_player.Stop();
        }
    }
}
