using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnHit : MonoBehaviour
{

    //[HideInInspector] public Health health;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Transform t;

    private void OnEnable()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        //health = GetComponent<Health>();
        t = GetComponent<Transform>();
    }

    public abstract void Hit(float dot);
}


