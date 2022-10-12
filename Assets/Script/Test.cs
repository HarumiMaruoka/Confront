using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    [InputName, SerializeField]
    string a;
    [TagName, SerializeField]
    string t;

    void Start()
    {

    }
    void Update()
    {

    }

    private void DrawInputName()
    {
        var inputManager = UnityEditor.AssetDatabase.LoadAssetAtPath("ProjectSettings/InputManager.asset", typeof(UnityEngine.Object));
        var serializedObject = new SerializedObject(inputManager);
        var axesProperty = serializedObject.FindProperty("m_Axes");

        var names = new List<string>();

        for (int i = 0; i < axesProperty.arraySize; i++)
        {
            var axis = axesProperty.GetArrayElementAtIndex(i);
            var name = axis.FindPropertyRelative("m_Name").stringValue;

            names.Add(name);
            Debug.Log(name);
        }
    }
}
