using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelGenerator
{
    public class LevelGenerator : MonoBehaviour
    {
        public static LevelGenerator Instance;
        public Material baseMaterial;
        public Material solidBlockMat;
        public GameObject EnemyAIPrefab;

        [Header("Enemy Setting")] [FormerlySerializedAs("Number of Enemies")]
        public int enemyNumber = 1;
        [Space(20)]
        
        [FormerlySerializedAs("Row Count")]public int rowCount = 10;
        [FormerlySerializedAs("Column Count")]public int columnCount = 10;
        public CameraController cameraController;

        [FormerlySerializedAs("_walkablePath")] public List<Vector3> walkablePath = new List<Vector3>();
        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine("GenerateLevel");
            }
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
            ground.transform.position = new Vector3(rowCount,-0.5f,columnCount);
            ground.transform.localScale = new Vector3(rowCount*2 + 1,1,columnCount*2 + 1);
            ground.GetComponent<MeshRenderer>().material = baseMaterial;
            
            var topWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            topWall.name = "topWall";
            topWall.transform.position = new Vector3(rowCount, 0,columnCount*2+1);
            topWall.transform.localScale = new Vector3(rowCount*2+1,1,1);
            topWall.GetComponent<MeshRenderer>().material = solidBlockMat;
            /*var topWallCol = topWall.AddComponent<BoxCollider>();
            topWallCol.size = topWall.transform.localScale;*/
            
            var bottomWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bottomWall.name = "bottomWall";
            bottomWall.transform.position = new Vector3(rowCount, 0,-1);
            bottomWall.transform.localScale = new Vector3(rowCount*2+1,1,1);
            bottomWall.GetComponent<MeshRenderer>().material = solidBlockMat;
            /*var bottomWallCol = topWall.AddComponent<BoxCollider>();
            bottomWallCol.size = bottomWall.transform.localScale;*/
            
            var leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftWall.name = "leftWall";
            leftWall.transform.position = new Vector3(-1, 0,columnCount);
            leftWall.transform.localScale = new Vector3(1,1,columnCount*2 + 3);
            leftWall.GetComponent<MeshRenderer>().material = solidBlockMat;
            /*var leftWallCol = topWall.AddComponent<BoxCollider>();
            leftWallCol.size = leftWall.transform.localScale;*/
            
            var rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightWall.name = "rightWall";
            rightWall.transform.position = new Vector3(rowCount* 2 +1, 0,columnCount);
            rightWall.transform.localScale = new Vector3(1,1,columnCount*2 + 3);
            rightWall.GetComponent<MeshRenderer>().material = solidBlockMat;
            /*var rightWallCol = topWall.AddComponent<BoxCollider>();
            rightWallCol.size = rightWall.transform.localScale;*/

            //var pos = mainCamera.position;
            //mainCamera.position = new Vector3(rowCount,15,columnCount);
            
            cameraController.SetMax(rowCount*2 - cameraController.minX, columnCount*2 - cameraController.minY );
            
            for (var x = 1; x < rowCount*2; x+=2)
            {
                for (var z = 1; z < columnCount*2; z+=2)
                {
                    //Debug.Log("Generating : row - " + x + ", Column - " +z);
                    var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    g.transform.rotation = Quaternion.identity;
                    g.transform.position = new Vector3(x, 0.5f, z);
                    g.transform.SetParent(transform);
                    g.GetComponent<MeshRenderer>().material = solidBlockMat;
                    //g.AddComponent<BoxCollider>();
                    
                    walkablePath.Add(new Vector3(x,0.4f,z+1));
                    walkablePath.Add(new Vector3(x+1,0.4f,z+1));
                    walkablePath.Add(new Vector3(x+1,0.4f,z));
                    
                    if (x == 1)
                    {
                        if (z == 1)
                        {
                            walkablePath.Add(new Vector3(x - 1, 0.4f, z));
                            walkablePath.Add(new Vector3(x - 1, 0.4f, z + 1));
                            walkablePath.Add(new Vector3(x - 1, 0.4f, z - 1));
                            walkablePath.Add(new Vector3(x, 0.4f, z - 1));
                            walkablePath.Add(new Vector3(x + 1, 0.4f, z - 1));
                        }
                        else
                        {
                            walkablePath.Add(new Vector3(x - 1, 0.4f, z));
                            walkablePath.Add(new Vector3(x - 1, 0.4f, z + 1));
                        }
                    }
                    else if (z == 1)
                    {
                        walkablePath.Add(new Vector3(x + 1, 0.4f, z - 1));
                        walkablePath.Add( new Vector3(x, 0.4f, z - 1));
                    }
                    //var s3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                }
            }
            
            AddEnemy();
        }

        private void AddEnemy()
        {
            for (var i = 0; i < enemyNumber; i++)
            {
                Instantiate(EnemyAIPrefab);
            }
        }
    }
}
