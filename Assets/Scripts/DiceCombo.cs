using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;

[System.Serializable]
public class DiceCombo
{
    public int baseValue;

    [Space]

    public bool add;

    [Space]

    public int diceCount;
    public int diceValue;

    public DiceCombo(int baseValue, bool add, int diceCount, int diceValue)
    {
        this.baseValue = baseValue;
        this.add = add;
        this.diceCount = diceCount;
        this.diceValue = diceValue;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DiceCombo))]
public class DiceComboDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {


        var win = rect.width;
        var h = rect.y + rect.height * 1.5f;

        int windows = 6;
        var w1 = win * 1/windows; 
        var w2 = win * 1 / windows;
        var w3 = win * 1 / windows;
        var w4 = win * 1 / windows;
        var w5 = win * 1 / windows;
        var w6 = win * 1 / windows;
        var space = win * 0.025f;

        var rect1 = new Rect(rect.x, h, w1, rect.height);

        var rect2 = new Rect(rect.x + w1 + space, h, w2 - space, rect.height);
        var rect3 = new Rect(rect.x + w1 + w2 + space, h, w3 - space, rect.height);
        var rect4 = new Rect(rect.x + w1 + w2 + w3 + space, h, w4 - space, rect.height);
        var rect5 = new Rect(rect.x + w1 + w2 + w3 + w4 + space, h, w5 - space, rect.height);
        var rect6 = new Rect(rect.x + w1 + w2 + w3 + w4 + w5 + space, h, w6 - space, rect.height);
        EditorGUI.LabelField(rect, label.text);

        EditorGUI.BeginProperty(rect, label, property);

        SerializedProperty baseValue = property.FindPropertyRelative("baseValue");
        SerializedProperty diceCount = property.FindPropertyRelative("diceCount");
        SerializedProperty diceValue = property.FindPropertyRelative("diceValue");

        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        
        EditorGUI.LabelField(rect1, "Roll: ");
        var baseField = EditorGUI.IntField(rect2, baseValue.intValue);
        EditorGUI.LabelField(rect3, " + ");
        var countField = EditorGUI.IntField(rect4, diceCount.intValue);
        EditorGUI.LabelField(rect5, " d ");
        var valueField = EditorGUI.IntField(rect6, diceValue.intValue);
        
        if (EditorGUI.EndChangeCheck())
        {
            baseValue.intValue = baseField;
            diceCount.intValue = countField;
            diceValue.intValue = valueField;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUI.EndProperty();

        GUILayout.Space(35);

        if (GUILayout.Button("Roll Dice"))
        {
            int value = baseField;

            for (int i = 0; i < countField; i++)
            {
                value += Random.Range(1, valueField);
            }
            Debug.Log("Rolled: " + value);
        }
    }
}
#endif