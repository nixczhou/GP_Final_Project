using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class field_skill_control : MonoBehaviour
{
    ParticleSystem field_skill_effect;
    public BallController ball;
    float current_time = 0.0f;
    float start_time = 0.0f;
    bool skill_mode = false;

    // Start is called before the first frame update
    void Start()
    {
        field_skill_effect = GetComponentInChildren<ParticleSystem>();
        field_skill_effect.Stop();
        ball = GameObject.Find("ball").GetComponent<BallController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        current_time = Time.time;
        if (current_time - start_time >= 10.0f)
        {
            field_skill_effect.Clear();
            field_skill_effect.Stop();
            skill_mode = false;
        }
        if (skill_mode)
        {
            if (ball.transform.position.x < 5.0f && ball.transform.position.x > -5.0f)
            {
                if (ball.transform.position.z < 10.0f && ball.transform.position.z > -10.0f)
                {
                    ball.add_force_to_ball(0.05f, new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
                }
            }
        }
    }
    public void field_skill(string player_name)
    {
        start_time = Time.time;
        field_skill_effect.Play();
        skill_mode = true;
    }
}
