using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;

/*[CustomEditor(typeof(userPrefs))]
public class userPrefsEditor : Editor {
	Hashtable test = new Hashtable();

	public override void OnInspectorGUI() {
		//userPrefs myTarget = (userPrefs)target;
		userPrefs instance = (userPrefs)target;

		foreach (FieldInfo f in instance.GetType ().GetFields()) {
			if (f.GetValue (instance) == null)
				test[f.Name] = EditorGUILayout.ObjectField ("Players", test[f.Name] as player, f.FieldType, true);
		}

		//test["testing"] = EditorGUILayout.ObjectField ("Players", test["testing"], typeof(GameObject), true) as GameObject;
		//test["testing"] = EditorGUILayout.ObjectField ("Players", (GameObject)test["testing"], typeof(GameObject), true) as GameObject;
	}
}*/

/*[CustomEditor(typeof(userPrefs))]
public class userPrefsEditor : Editor {
	public override void OnInspectorGUI() {
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		serializedObject.Update ();
		userPrefs mytarget = (userPrefs)target;

		userPrefs.player1 = new player ();
		userPrefs.player1.prefab = EditorGUILayout.ObjectField ("Player 1", mytarget.player1.prefab, typeof(GameObject), true) as GameObject;
		userPrefs.player1.color = EditorGUILayout.ColorField ("Color", userPrefs.player1.color);
		
		EditorGUILayout.Separator ();

		userPrefs.player2 = new player ();
		userPrefs.player2.prefab = EditorGUILayout.ObjectField ("Player 2", userPrefs.player2.prefab, typeof(GameObject), true) as GameObject;
		userPrefs.player2.color = EditorGUILayout.ColorField ("Color", userPrefs.player2.color);

		mytarget.a = EditorGUILayout.ObjectField ("Player 1", mytarget.a, typeof(GameObject), true) as GameObject;

		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		serializedObject.ApplyModifiedProperties ();
	}

	/*public void OnEnable()
	{
		m_object = new SerializedObject((userPrefs)target);
	}*/

	/*public override void OnInspectorGUI()
	{
		SerializedObject inspect = new SerializedObject ((userPrefs)target);

		//inspect.Update ();
		//m_object.Update();

		//GUILayout.Label("Some label", EditorStyles.boldLabel);
		SerializedProperty a = userPrefs.player1;

		//Editor otherEditor = Editor.CreateEditor((Object)userPrefs.player1);
		//otherEditor.DrawDefaultInspector();

		//SerializedProperty prop = m_object.FindProperty("player1");
		/*userPrefs a = (userPrefs)target;
		SerializedObject b = new SerializedObject (target);
		EditorGUILayout.PropertyField(inspect.FindProperty ("player1"), true);
		
		//inspect.ApplyModifiedProperties();
	}*/
	
	/*public override void OnInspectorGUI() {
		/*Debug.Log ("start");
		foreach (FieldInfo f in target.GetType ().GetFields ()) {
			Debug.Log (f.FieldType);
		}

		//userPrefs.player2 = EditorGUILayout.ObjectField ("Player2", userPrefs.player2, typeof(player), true) as player;

		//test myTarget = (test)target;
		//Debug.Log (target);

		//test.get = EditorGUILayout.ObjectField ("Normal Camera", test.get, typeof(GameObject), true) as GameObject;

		//myTarget.hi = EditorGUILayout.IntField ("Enter: ", myTarget.hi);

		//mode.normalCamera = EditorGUILayout.ObjectField ("Normal Camera", mode.normalCamera, typeof(GameObject)) as GameObject;

		//normalCam = EditorGUILayout.ObjectField ("Normal Camera", normalCam, typeof(Camera), true) as GameObject;
		//myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
		//EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
	}
}*/