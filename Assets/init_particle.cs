using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init_particle : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem particleObject;
    void Start()
    {
        particleObject = GetComponent<ParticleSystem>();
        particleObject.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
