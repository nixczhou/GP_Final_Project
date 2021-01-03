using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_skill_control : MonoBehaviour
{
    ParticleSystem skill_effect_on_player;
    ParticleSystem skill3_effect;
    public GameObject energy_bar;
    public string skill_key;
    public BallController ball;
    public field_skill_control field;
    public PlayerController player;
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
        player = transform.gameObject.GetComponent<PlayerController>();
        if (transform.parent.name == "Player1")
        {
            energy_bar =  GameObject.Find("Player1-HealthBar");
        }
        else
        {
            energy_bar =  GameObject.Find("Player2-HealthBar");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        skill3_current_time = Time.time;
        if(Input.GetKey(skill_key) && energy_bar.transform.GetChild(4).GetComponent<Image>().fillAmount == 1.0f) 
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
                player.speed = 7.5f;
            }
            energy_bar.transform.GetChild(4).GetComponent<Image>().fillAmount = 0.0f;
            player.charge = 0.0f;
        }
        if (skill3_current_time - skill3_start_time >= 10.0f)
        {
            skill3_effect.Clear();
            skill3_effect.Stop();
            player.speed = 5.0f;
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
