using UnityEngine;
using UnityEditor;

[System.Serializable]
public class DiceCombo
{
    public int diceCount;
    public int diceSides;
    public int modifier;
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DiceCombo))]
public class DiceComboDrawer : PropertyDrawer
{
    bool showPosition = true;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        showPosition = EditorGUILayout.Foldout(showPosition, label, true);
        if (showPosition)
        {
            DisplayProperty(rect, property, label);
        }
    }

    private void DisplayProperty(Rect rect, SerializedProperty property, GUIContent label)
    {
        var w1 = rect.width * 0.15f;
        var w2 = rect.width * 0.15f;
        var w3 = rect.width * 0.05f;
        var w4 = rect.width * 0.15f;
        var w5 = rect.width * 0.05f;
        var w6 = rect.width * 0.15f;
        var w7 = rect.width * 0.15f;
        var h = rect.height * 2.5f;// rect.y + rect.height * 2.5f;

        var rect1 = new Rect(rect.x, h, w1, rect.height);
        var rect2 = new Rect(rect1.x + w1, h, w2, rect.height);
        var rect3 = new Rect(rect2.x + w2, h, w3, rect.height);
        var rect4 = new Rect(rect3.x + w3, h, w4, rect.height);
        var rect5 = new Rect(rect4.x + w4, h, w5, rect.height);
        var rect6 = new Rect(rect5.x + w5, h, w6, rect.height);
        var rect7 = new Rect(rect6.x + w6, h, w7, rect.height);

        GUIStyle centered = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter
        };
        GUIStyle righAlign = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleRight
        };

        EditorGUI.BeginProperty(rect, label, property);
        SerializedProperty count = property.FindPropertyRelative("diceCount");
        SerializedProperty sides = property.FindPropertyRelative("diceSides");
        SerializedProperty mod = property.FindPropertyRelative("modifier");

        EditorGUILayout.BeginHorizontal();
        EditorGUI.LabelField(rect1, "Roll: ");
        
        EditorGUI.BeginChangeCheck();
        var countField = EditorGUI.IntField(rect2, count.intValue);
        EditorGUI.LabelField(rect3, " d ", righAlign);
        var valueField = EditorGUI.IntField(rect4, sides.intValue);
        EditorGUI.LabelField(rect5, " + ", centered);
        var modField = EditorGUI.IntField(rect6, mod.intValue);

        if (EditorGUI.EndChangeCheck())
        {
            mod.intValue = modField;
            count.intValue = countField;
            sides.intValue = valueField;
        }

        var min = count.intValue + mod.intValue;
        var max = count.intValue * sides.intValue + mod.intValue;
        EditorGUI.LabelField(rect7, min + "/" + max, centered);

        EditorGUILayout.EndHorizontal();
        EditorGUI.EndProperty();

        GUILayout.Space(30);

        if (GUILayout.Button("Roll Dice"))
        {
            Debug.Log("Rolled: " + Dice.Roll(count.intValue, sides.intValue, mod.intValue));
        }
    }
}
#endif