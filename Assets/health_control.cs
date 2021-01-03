using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health_control : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject HealthBar;
    public BallController ball;
    bool init_state = true;
    public Text game_score_text;
    string temp = "";
    void Start()
    {
        ball = GameObject.Find("ball").GetComponent<BallController>();
        game_score_text = GameObject.Find("CurGameText").GetComponent<Text>();
        temp = game_score_text.text;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (temp != game_score_text.text)
        {
            temp = game_score_text.text;
            init_state = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.transform.root.name);
        if (other.gameObject.transform.root.name == "Player1")
        {
            if (init_state)
            {
                init_state = false;
            }
            else
            {
                HealthBar = GameObject.Find("Player1-HealthBar");
                HealthBar.transform.GetChild(2).GetComponent<Image>().fillAmount -= 0.05f;
                if (ball.skill_mode)
                {
                    HealthBar.transform.GetChild(2).GetComponent<Image>().fillAmount -= 0.2f;
                    ball.skill_mode = false;
                }
            }
        }
        if (other.gameObject.transform.root.name == "Player2")
        {
            if (init_state)
            {
                init_state = false;
            }
            else
            {
                HealthBar = GameObject.Find("Player2-HealthBar");
                HealthBar.transform.GetChild(2).GetComponent<Image>().fillAmount -= 0.05f;
                if (ball.skill_mode)
                {
                    HealthBar.transform.GetChild(2).GetComponent<Image>().fillAmount -= 0.2f;
                    ball.skill_mode = false;
                }
            }
        }
    }
}
