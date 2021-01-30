using BomberMan.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BomberMan.Scripts.Player {
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;
        public float speed = 1f;
        [FormerlySerializedAs("Player Rigidbody")] public Rigidbody playerRB;
        public Renderer render;

        public int ExplosionLength
        {
            get => _explosionLength;
        }
        private float _horizontal, _vertical;

        private int _bombPlaceCount = 1;
        private int _maxBombCount = 1;
        private int _explosionLength = 1;
        public List<PowerUpAbilities> powerUpAbilities { get; } = new List<PowerUpAbilities>();

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Place Bomb Logic
                PlaceBomb();
            }
        }
        private void FixedUpdate()
        {
            playerRB.transform.position = new Vector3(playerRB.transform.position.x + speed * _horizontal * Time.deltaTime,
                playerRB.transform.position.y,
                playerRB.transform.position.z + speed * _vertical * Time.deltaTime);
        }

        private void PlaceBomb()
        {
            if (_bombPlaceCount <= 0) return;
            var x = Mathf.RoundToInt(transform.position.x);
            var y = Mathf.RoundToInt(transform.position.z);

            var pos = new Vector3(x, 0.4f, y);
            //Debug.Log("Bomb Position : " + pos);
            gameObject.layer = 12;
            Instantiate(GameManager.Instance.bombPrefab, pos, Quaternion.identity);
            _bombPlaceCount--;
        }
        public void ChangeLayerMask()
        {
            gameObject.layer = 10;
        }
        public void UpdateBombCount()
        {
            lock (this)
            {
                _bombPlaceCount += 1;
                _bombPlaceCount = _bombPlaceCount > _maxBombCount ? _maxBombCount : _bombPlaceCount;
            }
        }

        public void UpdateMaxBombCount(int count)
        {
            _maxBombCount += count;
        }

        public void StartDissolve()
        {
            StartCoroutine("Dissolve");
        }

        public void UpdatePowerUp(PowerUpAbilities abilityType)
        {
            switch(abilityType)
            {
                case PowerUpAbilities.IncreaseFlame:
                    _explosionLength++;
                    break;

            }
        }
        IEnumerator Dissolve()
        {
            var dis = 0f;
            while (dis < 1f)
            {
                dis += Time.deltaTime * 2f;
                render.material.SetFloat("_SliceAmount", dis);
                yield return 0;
            }
            //Destroy(gameObject);
        }
    }
}
