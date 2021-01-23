using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BomberMan.Scripts.Player
{
    public class PlayerTrigger : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.layer == 13)
            {
                PlayerController.Instance.ChangeLayerMask();
            }
        }
    }
}
