﻿using UnityEditor;

using RPG.CameraUI;

// TODO consider changing to a property drawer
[CustomEditor(typeof(CameraRaycaster))]
public class CameraRaycasterEditor : Editor
{
    bool isLayerPrioritiesUnfolded = true; // store the UI state

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Serialize cameraRaycaster instance

        isLayerPrioritiesUnfolded = EditorGUILayout.Foldout(isLayerPrioritiesUnfolded, "Layer Priorities");
        if (isLayerPrioritiesUnfolded)
        {
            EditorGUI.indentLevel++;
            {
                BindArraySize();
                BindArrayElements();
            }
            EditorGUI.indentLevel--;
        }
        //Note by having all these UI paints as methods, it is very easy to reorganize them
        //PrintString();

        serializedObject.ApplyModifiedProperties(); // De-serialize back to cameraRaycaster (and create undo point)
    }

    void BindArraySize()
    {
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
        int requiredArraySize = EditorGUILayout.IntField("Size", currentArraySize);
        if (requiredArraySize != currentArraySize)
        {
            serializedObject.FindProperty("layerPriorities.Array.size").intValue = requiredArraySize;
        }
    }

    void BindArrayElements()
    {
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
        for (int i = 0; i < currentArraySize; i++)
        {
            var prop = serializedObject.FindProperty(string.Format("layerPriorities.Array.data[{0}]", i));
            prop.intValue = EditorGUILayout.LayerField(string.Format("Layer {0}:", i), prop.intValue);
        }
    }

    private void PrintString() {
        //Current value is displayed in the textfield, has default value
        string currentText = serializedObject.FindProperty("stringToPrint").stringValue;
        string requiredText = EditorGUILayout.TextField("String to print", currentText);
        //Update the property in CameraRaycaster
        serializedObject.FindProperty("stringToPrint").stringValue = requiredText;
    }
}
