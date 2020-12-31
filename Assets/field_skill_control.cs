using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class field_skill_control : MonoBehaviour
{
    ParticleSystem field_skill_effect;
    float current_time = 0.0f;
    float start_time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        field_skill_effect = GetComponentInChildren<ParticleSystem>();
        field_skill_effect.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        current_time = Time.time;
        if (current_time - start_time >= 30.0f)
        {
            field_skill_effect.Clear();
            field_skill_effect.Stop();
        }
    }
    public void field_skill(string player_name)
    {
        start_time = Time.time;
        field_skill_effect.Play();
    }
}
