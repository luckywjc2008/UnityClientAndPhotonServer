using UnityEditor;
using UnityEngine;

namespace StarForce.Editor
{
    [CustomEditor(typeof(DeviceModelConfigCustom))]
    public class DeviceModelConfigInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Device Model ConfigCustom Editor"))
            {
                DeviceModelConfigEditorWindow.OpenWindow((DeviceModelConfigCustom)target);
            }
        }
    }
}
