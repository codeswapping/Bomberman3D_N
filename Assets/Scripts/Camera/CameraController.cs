using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minX = 0f;
    public float maxX = 0f;
    public float minY = 0f;
    public float maxY = 0f;

    private Transform playerTrans;
    // Start is called before the first frame update

    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SetMax(float maxx, float maxy)
    {
        maxX = maxx;
        maxY = maxy;
        Debug.Log("Max X : " + maxx + ", Max Y : " + maxy);
    }

    private void LateUpdate()
    {
        float x = Mathf.Clamp(playerTrans.position.x, minX, maxX);
        float z = Mathf.Clamp(playerTrans.position.z, minY, maxY);
        transform.position = new Vector3(x,transform.position.y,z);
    }
}
