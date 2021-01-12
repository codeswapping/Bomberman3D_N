using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace BomberMan.Scripts.Bomb
{
    public class BombController : MonoBehaviour
    {
        [FormerlySerializedAs("Explosion Time")]public float autoExplodeTime = 3f;
        private bool _isAutoExplode = true;
        private void Start()
        {
            _isAutoExplode = !PlayerController.Instance.powerUpAbilities.Contains(PowerUpAbilities.ExplosionAtWill);
        }

        private void Update()
        {
            if (_isAutoExplode)
            {
                autoExplodeTime -= Time.deltaTime;
                if (autoExplodeTime <= 0)
                {
                    StartExplosion();
                }
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.B))
                {
                    StartExplosion();
                }
            }
        }

        private void StartExplosion()
        {
            ExplosionController con =
                Instantiate(GameManager.Instance.explosionPrefab, transform.position, transform.rotation)
                    .GetComponent<ExplosionController>();
            con.Explode(PlayerController.Instance.explosionLength, Direction.ALL);
            PlayerController.Instance.UpdateBombCount();
            Destroy(gameObject);
        }
    }
}
