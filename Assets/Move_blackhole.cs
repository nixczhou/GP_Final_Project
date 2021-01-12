using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_blackhole : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject blackhole;
    public float move_distance;
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = blackhole.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        blackhole.transform.position = _startPosition + new Vector3(Mathf.Sin(Time.time)* move_distance, 0.0f, 0.0f);
    }
}
