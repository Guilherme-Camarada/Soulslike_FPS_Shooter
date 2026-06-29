using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

// This tells Unity to use this inspector for Upgrade.cs and any classes that inherit from it
[CustomEditor(typeof(Upgrade), true)]
public class UpgradeEditor : Editor
{
    private ReorderableList _list;
    private SerializedProperty _effectListProp;

    private void OnEnable()
    {
        // IMPORTANT: This string must exactly match the variable name in Upgrade.cs
        _effectListProp = serializedObject.FindProperty("_upgradeEffectList");

        if (_effectListProp != null)
        {
            // Initialize the ReorderableList
            _list = new ReorderableList(serializedObject, _effectListProp, true, true, true, true);

            // 1. Draw the Header
            _list.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Upgrade Effects");
            };

            // 2. Draw each element in the list
            _list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = _list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;

                // --- THE FIX IS HERE ---
                // Default to "Element X" just in case the reference is null
                string displayName = $"Element {index} (Empty)";

                // If the effect exists, grab the actual name of the class (e.g., "AmmoIncreaseEffect")
                if (element.managedReferenceValue != null)
                {
                    displayName = element.managedReferenceValue.GetType().Name;
                }

                // Pass our custom displayName into the GUI Content!
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)),
                    element,
                    new GUIContent(displayName),
                    true
                );
            };

            // 3. Dynamically adjust the height based on how many variables the specific effect has
            _list.elementHeightCallback = (int index) => {
                var element = _list.serializedProperty.GetArrayElementAtIndex(index);
                return EditorGUI.GetPropertyHeight(element) + 4;
            };

            // 4. THE MAGIC: Intercept the '+' button to show our polymorphic dropdown
            _list.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) => {
                var menu = new GenericMenu();

                // Find every class in the project that inherits from UpgradeEffect
                var types = TypeCache.GetTypesDerivedFrom<UpgradeEffect>()
                    .Where(t => !t.IsAbstract && !t.IsInterface);

                foreach (var type in types)
                {
                    // Add them to the dropdown menu
                    menu.AddItem(new GUIContent(type.Name), false, () => {
                        serializedObject.Update();

                        // Increase the list size
                        int index = l.serializedProperty.arraySize;
                        l.serializedProperty.arraySize++;
                        var newElement = l.serializedProperty.GetArrayElementAtIndex(index);

                        // Instantiate the specific pure C# class chosen from the dropdown
                        newElement.managedReferenceValue = Activator.CreateInstance(type);

                        serializedObject.ApplyModifiedProperties();
                    });
                }
                menu.ShowAsContext();
            };
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw all normal properties (like Name and Flavour) EXCEPT the list, to avoid drawing it twice
        DrawPropertiesExcluding(serializedObject, "m_Script", "_upgradeEffectList");

        if (_effectListProp != null)
        {
            EditorGUILayout.Space();
            // Draw our custom ReorderableList
            _list.DoLayoutList();
        }
        else
        {
            EditorGUILayout.HelpBox("Could not find the effect list property. Make sure the variable name in FindProperty matches the list in your Upgrade script.", MessageType.Warning);
        }

        serializedObject.ApplyModifiedProperties();
    }
}