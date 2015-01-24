using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(PrefabHandler))]
public class PrefabHandlerEditor : Editor {

	PrefabHandler prefabHandler;
	private bool initialized;
	private BaseBehaviour newPrefab;

	public string[] PrefabEnums;
	public BaseBehaviour[] prefabs;

	public List<Type> types;

	//flags for alert messages
	private bool addError;
	private float timeStamp;
	private float timer;
	private string errorMessage;

	void Init(){
		initialized = true;
		prefabs = new BaseBehaviour[(int) Prefab.Length];
	}

	void OnEnable(){
		if(!initialized){Init();}
		prefabHandler = (PrefabHandler) target;
		prefabHandler.Init();
		PrefabEnums = Enum.GetNames(typeof(Prefab));
		prefabHandler.Refresh();
	}

	public override void OnInspectorGUI(){

		base.OnInspectorGUI();

		GUILayout.BeginVertical();
		GUILayout.Box("Prefab Handler Editor");

		for(int i = 0; i < (int) Prefab.Length; i++){
			GUILayout.BeginHorizontal();
			GUILayout.Label(PrefabEnums[i]);

			if(prefabHandler.Contains((Prefab) i)){
				prefabHandler.SetPreload(i, GUILayout.Toggle(prefabHandler.GetPreload(i), "Preload"));
				bool remove = GUILayout.Button("Remove");
				if(remove){
					prefabHandler.RemovePrefab(i);
				}
			}
			else{
				BaseBehaviour field = (BaseBehaviour) EditorGUILayout.ObjectField(prefabs[i], typeof(BaseBehaviour), false);
				if(field != null && field != prefabs[i]){
					if(!AttemptAdd(field, i)){
						prefabs[i] = null;
						prefabHandler.RemovePrefab(i);
					}
				}
			}

			GUILayout.EndHorizontal();
		}

		if(addError){
			EditorGUILayout.HelpBox(errorMessage, MessageType.Error, true);
			timer -= Time.deltaTime;
			if(EditorApplication.timeSinceStartup - timeStamp > timer){
				addError = false;
			}
		}

		GUILayout.EndVertical();
	}

	private bool AttemptAdd(BaseBehaviour prefab, int index){

		bool result = false;
		string path = AssetDatabase.GetAssetPath(prefab);

		if(path.EndsWith("Resources/" + prefab.name + ".prefab")){
			
			string[] names = Enum.GetNames(typeof(Prefab));
			if(names[index] == prefab.name){
				prefabHandler.AddPrefab(index);
				result = true;
			}
			else{
				ErrorMessage(5, "Incorrect prefab for this Prefab Enum");
			}
		}
		else{
			ErrorMessage(5, "Object is not in a Resources folder");
		}

		return result;
	}

	private void AttemptAdd(BaseBehaviour prefab){

		string prefabName = prefab.name;
		string path = AssetDatabase.GetAssetPath(prefab);

		if(path.EndsWith("Resources/" + prefab.name + ".prefab")){

			string[] names = Enum.GetNames(typeof(Prefab));

			for(int i = 0; i < names.Length; i++){
				// if valid enum exists
				if(prefabName == names[i]){
					// if not already in array
					if(!prefabHandler.Contains((Prefab) i)){
						prefabHandler.AddPrefab(i);
					}
					else{
						ErrorMessage(5, "Prefab already exists in collection");
					}
					break;
				}

				ErrorMessage(5, "Need a matching Prefab enum");
			}
		}
		else{
			ErrorMessage(5, "Object is not in a Resources folder");
		}
	}

	protected void ErrorMessage(float seconds, string message){
		timeStamp = (float) EditorApplication.timeSinceStartup;
		errorMessage = message;
		addError = true;
		timer = seconds;
	}
}
