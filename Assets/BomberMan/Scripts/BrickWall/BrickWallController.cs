using System;
using System.Collections;
using System.Collections.Generic;
using BomberMan.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class BrickWallController : MonoBehaviour
{
    [FormerlySerializedAs("Renderer")]public Renderer renderer;

    public int pathIndex { get; set; }

    public void StartDissolve()
    {
        StartCoroutine("Dissolve");
    }

    IEnumerator Dissolve()
    {
        var dis = 0f;
        while (dis < 1f)
        {
            dis += Time.deltaTime * 2f;
            renderer.material.SetFloat("_Level",dis);
            yield return 0;
        }
        Destroy(gameObject);
    }

}
