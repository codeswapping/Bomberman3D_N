using System.Collections;
using UnityEngine;

namespace BomberMan.Scripts.Bomb
{
    public class ExplosionController : MonoBehaviour
    {
        bool _isTriggered = false;
        private void Start()
        {
            Invoke("DestroyNow", 0.5f);
        }
        public void StartExplosion(int explosionLength, Direction d)
        {
            var explo = new Explosion { explosionDirection = d, explosionLength = explosionLength };
            StartCoroutine(nameof(WaitExplosion), explo);
        }

        private void DestroyNow()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider collision)
        {
            _isTriggered = true;
            //Debug.Log("Collision TAG : " + collision.tag);
            switch(collision.tag)
            {
                case "BrickWall":
                    BrickWallController c = collision.GetComponent<BrickWallController>();
                    c.StartDissolve();
                    break;
                case "Enemy":
                    Enemy.EnemyController e = collision.GetComponent<Enemy.EnemyController>();
                    e.StartDissolve();
                    break;
                case "Bomb":
                    BombController ec = collision.GetComponent<BombController>();
                    ec.Explode();
                    break;
            }
            StopAllCoroutines();
        }

        private IEnumerator WaitExplosion(Explosion explosion)
        {
            yield return 0;
            if (_isTriggered) yield break;
            Vector3 pos = transform.position;
            switch (explosion.explosionDirection)
            {
                case Direction.TOP:
                    pos.z += 1;
                    break;
                case Direction.BOTTOM:
                    pos.z -= 1;
                    break;
                case Direction.LEFT:
                    pos.x -= 1;
                    break;
                case Direction.RIGHT:
                    pos.x += 1;
                    break;
            }
            int count = explosion.explosionLength - 1;
            if (count <= 0) yield break;
            var exp = Instantiate(GameManager.Instance.explosionPrefab,pos,transform.rotation).GetComponent<ExplosionController>();
            exp.StartExplosion(count,explosion.explosionDirection);
        }

        public struct Explosion
        {
            public Direction explosionDirection;
            public int explosionLength;
        }
    }
}
