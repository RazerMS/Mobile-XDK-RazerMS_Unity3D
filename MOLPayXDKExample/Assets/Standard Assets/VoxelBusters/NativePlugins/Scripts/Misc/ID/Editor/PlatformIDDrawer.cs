using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

namespace VoxelBusters.NativePlugins
{
	[CustomPropertyDrawer(typeof(PlatformID))]
	public class PlatformIDDrawer : PropertyDrawer 
	{
		#region Drawer Methods
		
		public override float GetPropertyHeight (SerializedProperty _property, GUIContent _label) 
		{
			return EditorGUIUtility.singleLineHeight;
		}
		
		public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label) 
		{
			EditorGUI.BeginProperty (_position, _label, _property);

			// Draw label
			_position				= EditorGUI.PrefixLabel (_position, GUIUtility.GetControlID (FocusType.Passive), _label);

			// Don't make child fields be indented
			int  indent 			= EditorGUI.indentLevel;
			EditorGUI.indentLevel 	= 0;

			// Calculate
			Rect	_platformRect	= new Rect (_position.x, _position.y, 60, _position.height);
			Rect	_IDRect			= new Rect (_position.x + 65, _position.y, _position.width - 65, _position.height);

			// Draw fields
			EditorGUI.PropertyField (_platformRect, _property.FindPropertyRelative ("m_platform"), GUIContent.none);
			EditorGUI.PropertyField (_IDRect, _property.FindPropertyRelative ("m_value"), GUIContent.none);

			// Reverting indent back to original
			EditorGUI.indentLevel = indent;
			
			EditorGUI.EndProperty ();
		}
		
		#endregion
	}
}
#endif