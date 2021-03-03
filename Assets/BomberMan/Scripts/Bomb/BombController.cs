using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using static BomberMan.Scripts.Bomb.ExplosionController;

namespace BomberMan.Scripts.Bomb
{
    public class BombController : MonoBehaviour
    {
        [FormerlySerializedAs("Explosion Time")] public float autoExplodeTime = 3f;
        [FormerlySerializedAs("Explosion Layer Mask")] public LayerMask masks;
        private bool _isAutoExplode = true;
        private bool _isExploded = false;

        private void Start()
        {
            _isAutoExplode = !Player.PlayerController.Instance.powerUpAbilities.Contains(PowerUpAbilities.ExplosionAtWill);
        }

        private void Update()
        {
            if (_isAutoExplode)
            {
                autoExplodeTime -= Time.deltaTime;
                if (autoExplodeTime <= 0 && !_isExploded)
                {
                    Explode();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.B)
                    && Player.PlayerController.Instance.powerUpAbilities.Contains(PowerUpAbilities.ExplosionAtWill))
                {
                    Explode();
                }
            }
        }

        public void Explode()
        {
            Destroy(gameObject);
            _isExploded = true;
            var explosionLength = Player.PlayerController.Instance.ExplosionLength;
            Instantiate(GameManager.Instance.explosionPrefab).transform.position = transform.position;
            var leftpos = transform.position;
            leftpos.x -= 1;
            ExplosionController leftec = Instantiate(GameManager.Instance.explosionPrefab).GetComponent<ExplosionController>();
            leftec.transform.position = leftpos;
            leftec.StartExplosion(explosionLength, Direction.LEFT);
            
            var rightpos = transform.position;
            rightpos.x += 1;
            ExplosionController rightec = Instantiate(GameManager.Instance.explosionPrefab).GetComponent<ExplosionController>();
            rightec.transform.position = rightpos;
            rightec.StartExplosion(explosionLength, Direction.RIGHT);
            
            var toppos = transform.position;
            toppos.z += 1;
            ExplosionController topec = Instantiate(GameManager.Instance.explosionPrefab).GetComponent<ExplosionController>();
            topec.transform.position = toppos;
            topec.StartExplosion(explosionLength, Direction.TOP);
            
            var bottompos = transform.position;
            bottompos.z -= 1;
            ExplosionController bottomec = Instantiate(GameManager.Instance.explosionPrefab).GetComponent<ExplosionController>();
            bottomec.transform.position = bottompos;
            bottomec.StartExplosion(explosionLength, Direction.BOTTOM);

            Player.PlayerController.Instance.UpdateBombCount();
        }
    }
}
