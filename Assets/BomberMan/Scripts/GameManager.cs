using System;
using System.Collections;
using System.Collections.Generic;
using BomberMan.Scripts.Camera;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace BomberMan.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Prefab References")]
        [FormerlySerializedAs("Explosion Effect Prefab")] public GameObject explosionPrefab;
        [FormerlySerializedAs("Enemy Prefab")]public GameObject EnemyAIPrefab;
        [FormerlySerializedAs("Bomb Prefab")] public GameObject bombPrefab;

        [FormerlySerializedAs("Brick Wall Prefab")]
        public GameObject brickWallPrefab;

        [Header("Material Preferences")][Space(20)]
        [FormerlySerializedAs("Base Material")]public Material baseMaterial;
        [FormerlySerializedAs("Solid Bar Material")]public Material solidBlockMat;
        
        [Header("Enemy Setting")][Space(20)] [FormerlySerializedAs("Number of Enemies")]
        public int enemyNumber = 1;

        [Header("Level Generator")]
        [Space(20)] [FormerlySerializedAs("Row Count")]
        public int rowCount = 10;
        [FormerlySerializedAs("Column Count")] public int columnCount = 10;
        public CameraController cameraController;
        [FormerlySerializedAs("Minimum Brick count")]
        public int minBrickCount;
        [FormerlySerializedAs("Maximum Brick Count")]
        public int maxBrickCount;

        [FormerlySerializedAs("_walkablePath")]
        public List<WalkablePathInfo> walkablePath = new List<WalkablePathInfo>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GenerateNextLevel();
        }

        private void GenerateNextLevel()
        {
            StartCoroutine("GenerateLevel");
        }

        private IEnumerator GenerateLevel()
        {
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }

            yield return 0;

            var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "Base";
            ground.transform.position = new Vector3(rowCount, -0.5f, columnCount);
            ground.transform.localScale = new Vector3(rowCount * 2 + 1, 1, columnCount * 2 + 1);
            ground.GetComponent<MeshRenderer>().material = baseMaterial;

            var topWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            topWall.name = "topWall";
            topWall.transform.position = new Vector3(rowCount, 0, columnCount * 2 + 1);
            topWall.transform.localScale = new Vector3(rowCount * 2 + 1, 1, 1);
            topWall.GetComponent<MeshRenderer>().material = solidBlockMat;

            var bottomWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bottomWall.name = "bottomWall";
            bottomWall.transform.position = new Vector3(rowCount, 0, -1);
            bottomWall.transform.localScale = new Vector3(rowCount * 2 + 1, 1, 1);
            bottomWall.GetComponent<MeshRenderer>().material = solidBlockMat;

            var leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftWall.name = "leftWall";
            leftWall.transform.position = new Vector3(-1, 0, columnCount);
            leftWall.transform.localScale = new Vector3(1, 1, columnCount * 2 + 3);
            leftWall.GetComponent<MeshRenderer>().material = solidBlockMat;

            var rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightWall.name = "rightWall";
            rightWall.transform.position = new Vector3(rowCount * 2 + 1, 0, columnCount);
            rightWall.transform.localScale = new Vector3(1, 1, columnCount * 2 + 3);
            rightWall.GetComponent<MeshRenderer>().material = solidBlockMat;

            cameraController.SetMax(rowCount * 2 - cameraController.minX, columnCount * 2 - cameraController.minY);

            for (var x = 1; x < rowCount * 2; x += 2)
            {
                for (var z = 1; z < columnCount * 2; z += 2)
                {
                    //Debug.Log("Generating : row - " + x + ", Column - " +z);
                    var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    g.transform.rotation = Quaternion.identity;
                    g.transform.position = new Vector3(x, 0.5f, z);
                    g.transform.SetParent(transform);
                    g.GetComponent<MeshRenderer>().material = solidBlockMat;
                    //g.AddComponent<BoxCollider>();

                    walkablePath.Add(new WalkablePathInfo{ isBrickWall = false, position = new Vector3(x, 0.4f, z + 1)});
                    walkablePath.Add(new WalkablePathInfo{ isBrickWall = false, position = new Vector3(x + 1, 0.4f, z + 1)});
                    walkablePath.Add(new WalkablePathInfo{ isBrickWall = false, position = new Vector3(x + 1, 0.4f, z)});

                    if (x == 1)
                    {
                        if (z == 1)
                        {
                            walkablePath.Add(new WalkablePathInfo{ isBrickWall = false, position = new Vector3(x - 1, 0.4f, z)});
                            walkablePath.Add(new WalkablePathInfo{ isBrickWall = false, position = new Vector3(x - 1, 0.4f, z + 1)});
                            walkablePath.Add(new WalkablePathInfo{ isBrickWall = false, position = new Vector3(x - 1, 0.4f, z - 1)});
                            walkablePath.Add(new WalkablePathInfo{ isBrickWall = false, position = new Vector3(x, 0.4f, z - 1)});
                            walkablePath.Add(new WalkablePathInfo{ isBrickWall = false, position = new Vector3(x + 1, 0.4f, z - 1)});
                        }
                        else
                        {
                            walkablePath.Add(new WalkablePathInfo{isBrickWall = false, 
                                position = new Vector3(x - 1, 0.4f, z)});
                            walkablePath.Add(new WalkablePathInfo{ isBrickWall = false, position = new Vector3(x - 1, 0.4f, z + 1)});
                        }
                    }
                    else if (z == 1)
                    {
                        walkablePath.Add(new WalkablePathInfo{ isBrickWall = false, position = new Vector3(x + 1, 0.4f, z - 1)});
                        walkablePath.Add(new WalkablePathInfo{ isBrickWall = false, position = new Vector3(x, 0.4f, z - 1)});
                    }

                    //var s3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                }
            }

            AddBrickWalls();
            AddEnemy();
        }

        private void AddBrickWalls()
        {
            var brickcount = Random.Range(minBrickCount, maxBrickCount);
            if (brickcount > walkablePath.Count - 10)
                brickcount = walkablePath.Count - Random.Range(10, 20);

            for (int brick = 0; brick < brickcount; brick++)
            {
                var pos = Random.Range(10, walkablePath.Count - 1);
                var walkablePathInfo = walkablePath[pos];
                while (walkablePathInfo.isBrickWall)
                {
                    pos = Random.Range(10, walkablePath.Count - 1);
                }

                walkablePathInfo.isBrickWall = true;
                var b = Instantiate(brickWallPrefab, walkablePathInfo.position, Quaternion.identity)
                    .GetComponent<BrickWallController>();
                b.pathIndex = pos;
            }
        }

        private void AddEnemy()
        {
            for (var i = 0; i < enemyNumber; i++)
            {
                Instantiate(EnemyAIPrefab);
            }
        }
    }

    public enum PowerUpAbilities
    {
        ExplosionAtWill,
        CanPassThroughWalls,
        CanPassThroughBomb,
        IsSpeedUp
    }
    
    public enum Direction
    {
        UNSPECIFIED = -1,
        TOP = 0,
        BOTTOM = 1,
        LEFT = 2,
        RIGHT = 3,
        ALL = 4
    }

    public struct WalkablePathInfo
    {
        public bool isBrickWall;
        public Vector3 position;
    }
}