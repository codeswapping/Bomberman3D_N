using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BomberMan.Scripts.Bomb
{
    public class ExplosionController : MonoBehaviour
    {
        public void Explode(int explosionLength, Direction d)
        {
            if (d == Direction.ALL)
            {
                int leftcount, rightcount, topcount, bottomcount;
                float distance;
                if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit))
                {
                    if (hit.collider == null) return;
                    distance = Vector3.Distance(transform.position, hit.point);
                    leftcount = Mathf.FloorToInt(distance);
                    if (leftcount > explosionLength)
                        leftcount = explosionLength;
                    if (leftcount > 0)
                    {
                        var explo = new Explosion {explosionDirection = Direction.LEFT, explosionLength = leftcount};
                        StartCoroutine(nameof(WaitExplosion), explo);
                    }
                }
                if (Physics.Raycast(transform.position, Vector3.right, out hit))
                {
                    if(hit.collider == null) return;
                    distance = Vector3.Distance(transform.position, hit.point);
                    rightcount = Mathf.FloorToInt(distance);
                    if (rightcount > 0)
                    {
                        if (rightcount > explosionLength) rightcount = explosionLength;
                        var explo = new Explosion {explosionDirection = Direction.RIGHT, explosionLength = rightcount};
                        StartCoroutine(nameof(WaitExplosion), explo);
                    }
                }
                if (Physics.Raycast(transform.position, Vector3.forward, out hit))
                {
                    if(hit.collider == null) return;
                    distance = Vector3.Distance(transform.position, hit.point);
                    topcount = Mathf.FloorToInt(distance);
                    if (topcount > explosionLength) topcount = explosionLength;
                    if(topcount == 0) return;
                    if (topcount > 0)
                    {
                        var explo = new Explosion {explosionDirection = Direction.TOP, explosionLength = topcount};
                        StartCoroutine(nameof(WaitExplosion), explo);
                    }
                }
                if (!Physics.Raycast(transform.position, Vector3.back, out hit)) return;
                
                if(hit.collider == null) return;
                distance = Vector3.Distance(transform.position, hit.point);
                bottomcount = Mathf.FloorToInt(distance);
                if (bottomcount > explosionLength) bottomcount = explosionLength;
                if (bottomcount <= 0) return;
                {
                    var explo = new Explosion {explosionDirection = Direction.BOTTOM, explosionLength = bottomcount};
                    StartCoroutine(nameof(WaitExplosion), explo);
                }
            }
            else
            {
                if (explosionLength <= 0) return;
                var explo = new Explosion {explosionDirection = d, explosionLength = explosionLength};
                StartCoroutine(nameof(WaitExplosion), explo);
            }
        }

        private IEnumerator WaitExplosion(Explosion explosion)
        {
            yield return new WaitForSeconds(0.2f);
            int count = explosion.explosionLength - 1;
            Vector3 pos = Vector3.zero;
            switch (explosion.explosionDirection)
            {
                case Direction.TOP:
                    pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                    break;
                case Direction.BOTTOM:
                    pos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                    break;
                case Direction.LEFT:
                    pos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                    break;
                case Direction.RIGHT:
                    pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                    break;
            }
            var exp = Instantiate(GameManager.Instance.explosionPrefab,pos,transform.rotation).GetComponent<ExplosionController>();
            exp.Explode(count,explosion.explosionDirection);
        }

        private struct Explosion
        {
            public Direction explosionDirection;
            public int explosionLength;
        }
    }
}
