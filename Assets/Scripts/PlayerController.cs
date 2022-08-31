using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    private Vector2 pInput;
    public float MouseAngle { private set; get; }
    public float PSpeedRatio { get; private set; }
    private Camera camera;
    [SerializeField] private float speedlimit = 10f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Rigidbody2D rbod;
    
    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        MovePlayer();
        Aim();
        
        PSpeedRatio = (rbod.velocity.magnitude) / (speedlimit * speedlimit);
        PSpeedRatio = Mathf.Clamp01(PSpeedRatio);
        
        if (Input.GetMouseButtonDown(0))
        {
            pInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 relativeMousePos = mousePos - transform.position;
            ApplyKnockback(relativeMousePos.normalized * 10f);
        }
    }

    private void Aim()
    {
        Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 relativeMousePos = mousePos - transform.position;
        MouseAngle = Mathf.Atan2(relativeMousePos.y, relativeMousePos.x) * Mathf.Rad2Deg;
    }

    private void MovePlayer()
    {
        pInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rbod.AddForce(pInput * (moveSpeed * Time.deltaTime), ForceMode2D.Impulse);
        
        //Limit player speed
        float velocity = rbod.velocity.magnitude;
        float diff = velocity - speedlimit;
        if (velocity >= speedlimit)
        {
            rbod.AddForce(-pInput * (diff * Time.deltaTime), ForceMode2D.Impulse);
        }
        else if (-velocity <= -speedlimit)
        {
            rbod.AddForce(pInput * (diff * Time.deltaTime), ForceMode2D.Impulse);
        }
    }

    public void ApplyKnockback(Vector2 dir)
    {
        rbod.AddForce(dir, ForceMode2D.Impulse);
    }
}