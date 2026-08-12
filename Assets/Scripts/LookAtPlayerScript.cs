using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class LookAtPlayerScript : MonoBehaviour
{
    [SerializeField] public Transform player;
    void Start()
    {
        if (!player) player = GameObject.Find("player parent").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
    }
}
