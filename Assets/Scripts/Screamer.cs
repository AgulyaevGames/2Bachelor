using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Screamer : MonoBehaviour
{
    public GameObject screamer;
    private Transform startPos;
    private Transform endPos;

    public float timeToReach = 1f;

    private bool triggered;
    private void Start()
    {
        startPos = transform.GetChild(0);
        endPos = transform.GetChild(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        triggered = true;
        
        if (other.CompareTag("Player"))
        {
            SpawnScreamer();
        }
    }

    private void SpawnScreamer()
    {
        var tempScreamer = Instantiate(screamer, startPos.position, startPos.rotation);
        
        tempScreamer.transform.DOMove(endPos.position, timeToReach).OnComplete(()=> Destroy(tempScreamer.gameObject));
    }
    
    
}
