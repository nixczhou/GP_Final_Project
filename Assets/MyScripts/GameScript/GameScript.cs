using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    public int p1MatchWon = 0;
    public int p2MatchWon = 0;
    /*
        0 = 0
        1 = 15
        2 = 30
        3 = 40
        4 = Adv
        5 = won
    */
    public int p1gamepoint = 0;
    public int p2gamepoint = 0;
    public float courtRight;
    public float courtLeft;
    public float baseline;
    public float net;
    public float serveLine;
    public bool player1LastHit = false;
    
    //Serving
    public bool ballOut = false;

    //Reinitialize
    public bool pointReset = false;
    public int serveNum = 0;

    public static GameObject player1;
    public static GameObject player2;
    public GameObject ball;

    public Text outText;
    public Text gameText;
    public Text p1MatchWonText;
    public Text p2MatchWonText;

    private float HealthBar1;
    private float HealthBar2;

    // Start is called before the first frame update
    void Start()
    {
        player1.GetComponent<P1Controller>().gameState = 0;
        player2.GetComponent<P2Controller>().gameState = 4;
        ball.GetComponent<BallController>().ballState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(pointReset){
            bool player1LastHit = ball.GetComponent<BallController>().player1LastHit;
            //Double Fault
            if(serveNum == 2){
                if(player1LastHit) p2gamepoint += 1;
                else p1gamepoint += 1;
                updateGameText();
                ballReset();
                outText.text = "Double Fault!";
                StartCoroutine(clearOutText(2.0f));
            }

            player1.GetComponent<P1Controller>().hitNum = 0;
            player2.GetComponent<P2Controller>().hitNum = 0;

            ball.GetComponent<BallController>().firstBounce = false;
            int ballState = ball.GetComponent<BallController>().ballState;
            
            pointReset = false;

            //Match Calculation
            if(p1gamepoint == 5 || p2gamepoint == 5){
                if(p1gamepoint == 5) p1MatchWon += 1;
                else p2MatchWon += 1;

                p1MatchWonText.text = "Match Won: " + p1MatchWon.ToString();
                p2MatchWonText.text = "Match Won: " + p2MatchWon.ToString();
                p1gamepoint = 0;
                p2gamepoint = 0;
                
                if((p1MatchWon + p2MatchWon) % 2 == 0){
                    player1.GetComponent<P1Controller>().gameState = 0;
                    player2.GetComponent<P2Controller>().gameState = 4;
                    ball.GetComponent<BallController>().ballState = 0;
                    //player1.GetComponent<P1Controller>().serve = true;
                    //player2.GetComponent<P2Controller>().serve = false;
                }
                else{
                    player1.GetComponent<P1Controller>().gameState = 4;
                    player2.GetComponent<P2Controller>().gameState = 0;
                    ball.GetComponent<BallController>().ballState = 0;
                    //player1.GetComponent<P1Controller>().serve = false;
                    //player2.GetComponent<P2Controller>().serve = true;
                }
                player1.GetComponent<P1Controller>().pointStart = true;
                player2.GetComponent<P2Controller>().pointStart = true;
                updateGameText();
                return;
            }

            //Game state change
            int p1GameState = player1.GetComponent<P1Controller>().gameState;
            int p2GameState = player2.GetComponent<P2Controller>().gameState;

            //Player 1 Serving
            if((p1MatchWon + p2MatchWon) % 2 == 0){
                if((p1gamepoint + p2gamepoint) % 2 == 0){
                    player1.GetComponent<P1Controller>().gameState = 0;
                    player2.GetComponent<P2Controller>().gameState = 4;
                    ball.GetComponent<BallController>().ballState = 0;
                }
                else{
                    player1.GetComponent<P1Controller>().gameState = 2;
                    player2.GetComponent<P2Controller>().gameState = 5;
                    ball.GetComponent<BallController>().ballState = 2;
                }
                player1.GetComponent<P1Controller>().pointStart = true;
                player2.GetComponent<P2Controller>().pointStart = true;
            }
            //Player 2 Serving
            else{
                if((p1gamepoint + p2gamepoint) % 2 == 0){
                    player2.GetComponent<P2Controller>().gameState = 0;
                    player1.GetComponent<P1Controller>().gameState = 4;
                    ball.GetComponent<BallController>().ballState = 0;
                }
                else{
                    player2.GetComponent<P2Controller>().gameState = 2;
                    player1.GetComponent<P1Controller>().gameState = 5;
                    ball.GetComponent<BallController>().ballState = 2;
                }
                player1.GetComponent<P1Controller>().pointStart = true;
                player2.GetComponent<P2Controller>().pointStart = true;
            }
        }

        HealthBar1 = GameObject.FindWithTag("healthbar1").GetComponent<Image>().fillAmount;
        HealthBar2 = GameObject.FindWithTag("healthbar2").GetComponent<Image>().fillAmount;

        if(HealthBar1 == 0.0f){
            outText.text = "Player 2 Win!!! \n Press ESC to exit";  
        }
        else if(HealthBar2 == 0.0f){
            outText.text = "Player 1 Win!!! \n Press ESC to exit";
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

    }

    void FixedUpdate(){
        if(ball.transform.position.y < -1){
            ballReset();
            if(ball.GetComponent<BallController>().firstBounce){
                gamePointCalculation();
                pointReset = true;
                outText.text = "Great Shot!";
                StartCoroutine(clearOutText(2.0f));
            }
            else if(player1.GetComponent<P1Controller>().hitNum != 0 && player2.GetComponent<P2Controller>().hitNum != 0){
                outText.text = "OUT!";
                StartCoroutine(clearOutText(2.0f));
            }
            ball.GetComponent<BallController>().firstBounce = false;
        }
    }

    void OnCollisionEnter (Collision other)
    {
        
        if(other.gameObject.name =="ball")
        {
            int ballState = other.gameObject.GetComponent<BallController>().ballState;
            bool player1LastHit = other.gameObject.GetComponent<BallController>().player1LastHit;
            float positionX = other.gameObject.transform.position.x ;
            float positionZ = other.gameObject.transform.position.z;
            bool firstBounce = other.gameObject.GetComponent<BallController>().firstBounce;
            
            //First Bounce Occuring
            if(!firstBounce){

                // ball Out on the side lines & baselines
                if(positionX < courtLeft || positionX  > courtRight || positionZ > baseline || positionZ < -baseline){
                    ballOut = true;
                    pointReset = true;
                    if((player1LastHit && positionZ > serveLine) || !player1LastHit && positionZ < -serveLine){
                        if(ballState >= 0 && ballState <= 3){
                            serveNum += 1;
                            ballOut = true;
                            pointReset = true;
                            ballReset();
                            if(serveNum == 1){
                                outText.text = "Fault! Second Serve";
                                StartCoroutine(clearOutText(2.0f));
                            }
                            print(serveNum);
                            return;
                        }
                    }
                    if(ballState >= 0 && ballState <= 3){
                        serveNum += 1;
                        if(serveNum == 1){
                            outText.text = "Fault! Second Serve";
                            StartCoroutine(clearOutText(2.0f));
                            return;
                        }
                    }
                    else{
                        ballOut = true;
                        pointReset = true;
                        ballReset();
                        gamePointCalculation();
                        outText.text = "OUT!";
                        StartCoroutine(clearOutText(2.0f));
                    }
                    return;
                    
                }
                //didn't go over the net
                if(player1LastHit && positionZ <= 0){
                    print("ballOut!");
                    ballOut = true;
                    pointReset = true;
                    if(ballState  >= 0 && ballState <= 3){
                        serveNum += 1;
                        ballOut = true;
                        pointReset = true;
                        ballReset();
                        if(serveNum == 1){
                            outText.text = "Fault! Second Serve";
                            StartCoroutine(clearOutText(2.0f));
                        }
                        return;
                    }
                    else{
                        ballOut = true;
                        pointReset = true;
                        ballReset();
                        gamePointCalculation();
                        outText.text = "Bad Luck!";
                        StartCoroutine(clearOutText(2.0f));
                    }
                    ballReset();
                    return;
                }
                else if(!player1LastHit && positionZ >= 0.0f){
                    ballOut = true;
                    pointReset = true;
                    if(ballState  >= 0 && ballState <= 3){
                        serveNum += 1;
                        ballOut = true;
                        pointReset = true;
                        ballReset();
                        if(serveNum == 1){
                            outText.text = "Fault! Second Serve";
                            StartCoroutine(clearOutText(2.0f));
                        }
                        return;
                    }
                    else{
                        ballOut = true;
                        pointReset = true;
                        ballReset();
                        gamePointCalculation();
                        outText.text = "OUT!";
                        StartCoroutine(clearOutText(2.0f));
                    }
                    return;
                }

                // Player 1 Serving Check
                if(player1LastHit ){
                    if( ballState == 0 || ballState == 1){
                        if(positionX > 0.0f || positionZ > serveLine){
                            serveNum += 1;
                            ballOut = true;
                            pointReset = true;
                            ballReset();
                            if(serveNum == 1){
                                outText.text = "Fault! Second Serve";
                                StartCoroutine(clearOutText(2.0f));
                            }
                            return;
                        }
                    } 
                    else if(ballState == 2 || ballState == 3){
                        if(positionX < 0.0f || positionZ > serveLine){
                            print("ballOut!");
                            serveNum += 1;
                            ballOut = true;
                            pointReset = true;
                            ballReset();
                            if(serveNum == 1){
                                outText.text = "Fault! Second Serve";
                                StartCoroutine(clearOutText(2.0f));
                            }
                            return;
                        }
                    }
                }
                //Player 2 Serving Check
                else{
                    if(ballState == 0 || ballState == 1){
                        if(positionX < 0.0f || positionZ > serveLine){
                            serveNum += 1;
                            ballOut = true;
                            pointReset = true;
                            ballReset();
                            if(serveNum == 1){
                                outText.text = "Fault! Second Serve";
                                StartCoroutine(clearOutText(2.0f));
                            }
                            return;
                        }
                    }
                    else if(ballState == 2 || ballState == 3){
                        if(positionX > 0.0f || positionZ > serveLine){
                            serveNum += 1;
                            ballOut = true;
                            pointReset = true;
                            ballReset();
                            if(serveNum == 1){
                                outText.text = "Fault! Second Serve";
                                StartCoroutine(clearOutText(2.0f));
                            }
                            return;
                        }
                    }
                }
                
                other.gameObject.GetComponent<BallController>().ballState = -1;
                other.gameObject.GetComponent<BallController>().firstBounce = true;
                
            }
            //Second Bounce
            else{
                outText.text = "Double Bounce!";
                StartCoroutine(clearOutText(2.0f));
                other.gameObject.GetComponent<BallController>().firstBounce = false;
                gamePointCalculation(); 
                pointReset = true;
                ball.GetComponent<BallController>().firstBounce = false;
                ballReset();
            }
        }
    }

    private void gamePointCalculation(){

        if(ball.GetComponent<BallController>().player1LastHit){
            if(p1gamepoint >= 0 && p1gamepoint <= 2){
                p1gamepoint += 1;
            }
            else{
                if(p1gamepoint == 3 && p2gamepoint == 3 || p1gamepoint == 4 && p2gamepoint == 3){
                    p1gamepoint += 1;
                }
                else{
                    p1gamepoint = 5;
                }
            }
        }
        else{
            if(p2gamepoint >= 0 && p2gamepoint <= 2){
                p2gamepoint += 1;
            }
            else{
                if(p2gamepoint == 3 && p1gamepoint == 3 || p2gamepoint == 4 && p1gamepoint == 3){
                    p2gamepoint += 1;
                }
                else{
                    p2gamepoint = 5;
                }
            }
        }
        serveNum = 0; 
        updateGameText();
    }

    void ballReset(){
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.transform.position = new Vector3(0.04f, -20.0f, -22.87f);
    } 
    void updateGameText(){
        string p1Text = "00";
        string p2Text = "00";

        if(p1gamepoint == 1) p1Text = "15";
        else if(p1gamepoint == 2) p1Text = "30";
        else if(p1gamepoint == 3) p1Text = "40";
        else if(p1gamepoint == 4) p1Text = "Adv";

        if(p2gamepoint == 1) p2Text = "15";
        else if(p2gamepoint == 2) p2Text = "30";
        else if(p2gamepoint == 3) p2Text = "40";
        else if(p2gamepoint == 4) p2Text = "Adv";

        gameText.text = p1Text + " - " + p2Text;
    }

    private IEnumerator clearOutText(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            outText.text = "";
        }
    }
}
