﻿using System;
using System.Collections;
using System.Collections.Generic;
using BomberMan.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public float speed = 1f;
    [FormerlySerializedAs("Player Rigidbody")]public Rigidbody playerRB;

    public int explosionLength
    {
        get => _explosionLength;
    }
    private float _horizontal, _vertical;
    
    private int _bombPlaceCount = 1;
    private int _maxBombCount = 1;
    private int _explosionLength = 1;
    public PowerUpAbilities[] powerUpAbilities { get; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }
    private void FixedUpdate()
    {
        playerRB.transform.position = new Vector3(playerRB.transform.position.x + speed * _horizontal * Time.deltaTime,
            playerRB.transform.position.y,
            playerRB.transform.position.z + speed * _vertical * Time.deltaTime);
    }
    public void UpdateBombCount()
    {
        lock (this)
        {
            _bombPlaceCount += 1;
            _bombPlaceCount = _bombPlaceCount > _maxBombCount ? _maxBombCount : _bombPlaceCount;
        }
    }

    public void UpdateMaxBombCount(int count)
    {
        _maxBombCount += count;
    }
}