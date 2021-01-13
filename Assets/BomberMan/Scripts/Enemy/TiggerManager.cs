using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BomberMan.Scripts.Enemy
{
    public class TiggerManager : MonoBehaviour
    {
        public delegate void OnTriggered(TriggerDirection direction);

        public event OnTriggered onTriggered;

        public TriggerDirection Direction;
        public bool IsTriggered;

        private void OnTriggerEnter(Collider other)
        {
            IsTriggered = true;
            onTriggered?.Invoke(Direction);
        }

        private void OnTriggerExit(Collider other)
        {
            IsTriggered = false;
        }

        public enum TriggerDirection
        {
            TOP,
            BOTTOM,
            RIGHT,
            LEFT
        }
    }
}
