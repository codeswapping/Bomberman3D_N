using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    [FormerlySerializedAs("Player Rigidbody")]public Rigidbody playerRB;

    private float _horizontal, _vertical;
    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        playerRB.transform.position = new Vector3(playerRB.transform.position.x + speed * _horizontal * Time.deltaTime,
            playerRB.transform.position.y,
            playerRB.transform.position.z + speed * _vertical * Time.deltaTime);
    }
}
