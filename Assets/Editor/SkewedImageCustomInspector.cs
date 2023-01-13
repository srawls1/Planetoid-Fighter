using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(SkewedImage))]
public class SkewedImageCustomInspector : UnityEditor.UI.ImageEditor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		SerializedProperty skewXProperty = serializedObject.FindProperty("skewX");
		EditorGUILayout.PropertyField(skewXProperty);
		serializedObject.ApplyModifiedProperties();
	}
}
