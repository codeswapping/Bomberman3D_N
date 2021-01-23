using System;
using System.Collections;
using System.Collections.Generic;
using BomberMan.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class BrickWallController : MonoBehaviour
{
    [FormerlySerializedAs("Renderer")]public Renderer render;
    public int pathIndex { get; set; }

    public void StartDissolve()
    {
        StartCoroutine("Dissolve");
        WalkablePathInfo info = GameManager.Instance.walkablePath[pathIndex];
        info.isBrickWall = false;
    }

    IEnumerator Dissolve()
    {
        var dis = 0f;
        while (dis < 1f)
        {
            dis += Time.deltaTime * 2f;
            render.material.SetFloat("_Level",dis);
            yield return 0;
        }
        Destroy(gameObject);
        if(GameManager.Instance.walkablePath[pathIndex].hasAbility)
        {
            string powerupname = GameManager.Instance.GetPowerupName(GameManager.Instance.walkablePath[pathIndex].abilityType);
            UnityEngine.Object powerup = Resources.Load("Powerups/" + powerupname);
            Instantiate(powerup, transform.position, Quaternion.identity);
        }
    }

}
