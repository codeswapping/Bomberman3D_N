using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BomberMan.Scripts;

namespace BomberMan.Scripts.Editor {
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var gm = (GameManager)target;
            var option = new GUILayoutOption[] { GUILayout.MaxHeight(20), GUILayout.MaxWidth(60) };
            int.TryParse( GUILayout.TextField("", option), out int num);
            if(GUILayout.Button("Get Pos at Path"))
            {
                Debug.Log("Position At Index : " + gm.walkablePath[num].position);
            }
        }
    }
}
