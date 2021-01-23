using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BomberMan.Scripts
{
    public class PowerupController : MonoBehaviour
    {
        public PowerUpAbilities AbilityType;
        public float TimeToCheckCollision = 1f;
        private float _timeFromSpawn = 0;

        private void Update()
        {
            _timeFromSpawn += Time.deltaTime;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (_timeFromSpawn >= TimeToCheckCollision)
            {
                //Debug.Log("Tag : " + other.tag);
                switch (other.tag)
                {
                    case "Player":
                        Player.PlayerController.Instance.UpdatePowerUp(AbilityType);
                        //Debug.Log("Player Abliity Updated : " + AbilityType);
                        break;
                    case "Explosion":
                        break;
                }
                Destroy(gameObject);
            }
        }
    }
}