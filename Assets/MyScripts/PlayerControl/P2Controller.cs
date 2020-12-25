using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Controller : PlayerController
{
    void animationInitialization(){
        normalState = Animator.StringToHash("Base Layer.NormalStatus");
        forehandState = Animator.StringToHash("Base Layer.Forehand");
        backhandState = Animator.StringToHash("Base Layer.Backhand");
        serveState = Animator.StringToHash("Base Layer.Serve");
    }
}
