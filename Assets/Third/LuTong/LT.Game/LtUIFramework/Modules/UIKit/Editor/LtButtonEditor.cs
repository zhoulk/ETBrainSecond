/***
 * 
 *    项目: "LtUIFrame" UI框架
 *
 *    描述: 
 *           功能： 
 *                  
 *    时间: 2017.7
 *
 *    版本: 0.1版本
 *
 *    修改记录: 无
 *
 *    开发人: 邓平
 *     
 */

using LtFramework;
using LtFramework.UI;
using UnityEditor;  
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(LtButton), true)]
[CanEditMultipleObjects]
public class LtButtonEditor : ButtonEditor
{
    private SerializedProperty _CtrlType;
    private SerializedProperty _SelecteLayer;
    private SerializedProperty _CanBothSelecte;
    private SerializedProperty _SelectedShow1P;
    private SerializedProperty _SelectedShow2P;

    protected override void OnEnable()
    {
        base.OnEnable();
        _CtrlType = serializedObject.FindProperty("CtrlType");
        _SelecteLayer = serializedObject.FindProperty("SelecteLayer");
        _CanBothSelecte = serializedObject.FindProperty("CanBothSelecte");
        _SelectedShow1P = serializedObject.FindProperty("SelectedShow1P");
        _SelectedShow2P = serializedObject.FindProperty("SelectedShow2P");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();

        EditorGUILayout.PropertyField(_SelecteLayer);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_CtrlType);
        switch (_CtrlType.enumValueIndex)
        {
            case (int) (LtButton.SelecteType.All):
                EditorGUILayout.PropertyField(_CanBothSelecte);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_SelectedShow1P);
                EditorGUILayout.PropertyField(_SelectedShow2P);
                break;
            case (int) (LtButton.SelecteType.Ctrl1P):
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_SelectedShow1P);
                break;
            case (int) (LtButton.SelecteType.Ctrl2P):
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_SelectedShow2P);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
