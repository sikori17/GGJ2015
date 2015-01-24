using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(AudioHandler))]
public class AudioHandlerEditor : Editor {

	AudioHandler audioHandler;
	[SerializeField] 
	private bool initialized;
	[SerializeField] [HideInInspector]
	private string[] AudioClassEnums;
	[SerializeField] [HideInInspector]
	private string[] AudioEnums;
	[SerializeField] [HideInInspector]
	private AudioClip[] audioClips;
	
	//flags for alert messages
	private bool addError;
	private float timeStamp;
	private float timer;
	private string errorMessage;
	
	void Init(){

		initialized = true;

		AudioClassEnums = Enum.GetNames(typeof(AudioClass));
		AudioEnums = Enum.GetNames(typeof(Audio));

		audioHandler = (AudioHandler) target;
		audioHandler.EditorInit();
	}
	
	void OnEnable(){
		if(!initialized || AudioHandler.Instance == null){Init();}
		ReloadAudioClips();
		audioHandler.EditorRefresh();
	}

	void ReloadAudioClips(){

		audioClips = new AudioClip[(int) Audio.Length];

		for(int i = 0; i < (int) Audio.Length; i++){

			string name = audioHandler.GetAudioObjectName((Audio) i);

			// A clip is set
			if(name != ""){
				AudioClip clip = Resources.Load(name) as AudioClip;
				// if clip exists
				if(clip != null){
					audioClips[i] = clip;
				}
				// clip is not in resources, reset
				else{
					audioHandler.SetAudioObjectName((Audio) i, "");
					audioHandler.SetAudioObjectClip((Audio) i, null);
					audioClips[i] = null;
				}
			}
		}
	}
	
	public override void OnInspectorGUI(){
		
		base.OnInspectorGUI();

		if(AudioHandler.Instance == null) OnEnable();
		
		GUILayout.BeginVertical();
		GUILayout.Box("Audio Handler Editor");

		GUILayout.Space(10);

		GUILayout.Box("Audio Classes");
		GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
		
		for(int i = 0; i < AudioClassEnums.Length - 1; i++){

			GUILayout.BeginHorizontal();
			GUILayout.Label(AudioClassEnums[i]);
			bool mute = audioHandler.GetAudioClassMute((AudioClass) i);
			bool state = GUILayout.Toggle(mute , "Mute");
			if(state != mute) audioHandler.SetAudioClassMute((AudioClass) i, state);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			string curVol = "" + audioHandler.GetAudioClassVolume((AudioClass) i);
			GUILayout.Label(curVol, GUILayout.MaxWidth(35));

			float volume = GUILayout.HorizontalSlider(audioHandler.GetAudioClassVolume((AudioClass) i), 0, 1.0f);
			audioHandler.SetAudioClassVolume((AudioClass) i, volume); 

			GUILayout.EndHorizontal();
		}

		GUILayout.Space(20);

		GUILayout.Box("Audio Objects");

		for(int i = 0; i < (int) Audio.Length; i++){

			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

			GUILayout.BeginHorizontal();
			GUILayout.Label(((Audio) i).ToString());
			bool mute = audioHandler.GetAudioObject((Audio) i).mute;
			bool state = GUILayout.Toggle(audioHandler.GetAudioObject((Audio) i).mute, "Mute");
			if(mute != state) audioHandler.GetAudioObject((Audio) i).SetDefaultMute(state);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			AudioClass audioClass = (AudioClass) EditorGUILayout.EnumPopup(audioHandler.GetAudioObjectClass((Audio) i));
			audioHandler.SetAudioObjectClass((Audio) i, audioClass);

			bool preload = GUILayout.Toggle(audioHandler.GetAudioObjectPreload((Audio) i), "Preload");
			audioHandler.SetAudioObjectPreload((Audio) i, preload);
			GUILayout.EndHorizontal();

			// if clip is already set
			if(audioClips[i] != null){
				GUILayout.BeginHorizontal();
				EditorGUILayout.ObjectField(audioClips[i], typeof(AudioClip), false);
				bool remove = GUILayout.Button("Remove");
				if(remove){
					audioClips[i] = null;
					audioHandler.ResetAudioObject((Audio) i);
				}
				GUILayout.EndHorizontal();
			}
			else{
				AudioClip clip = (AudioClip) EditorGUILayout.ObjectField(null, typeof(AudioClip), false);
				if(clip != null){
					string path = AssetDatabase.GetAssetPath(clip);
					string end = "Resources/" + clip.name;
					if(path.EndsWith(end + ".wav") || path.EndsWith(end + ".mp3") || path.EndsWith(end + ".ogg")){
						audioClips[i] = clip;
						audioHandler.SetAudioObjectName((Audio) i, clip.name);
					}
				}
			}

			GUILayout.BeginHorizontal();
			GUILayout.Label(audioHandler.GetAudioObjectDefaultVolume((Audio) i).ToString(), GUILayout.MaxWidth(35));
			float defaultVolume = GUILayout.HorizontalSlider(audioHandler.GetAudioObjectDefaultVolume((Audio) i), 0, 1.0f);
			audioHandler.SetAudioObjectDefaultVolume((Audio) i, defaultVolume);
			GUILayout.EndHorizontal();
		}
		
		GUILayout.EndVertical();

		EditorUtility.SetDirty(audioHandler);
	}
	
	private bool AttemptAdd(BaseBehaviour prefab, int index){
		
		bool result = false;
		string path = AssetDatabase.GetAssetPath(prefab);
		
		if(path.EndsWith("Resources/" + prefab.name + ".prefab")){
			
			string[] names = Enum.GetNames(typeof(Prefab));
			if(names[index] == prefab.name){
				//prefabHandler.AddPrefab(index);
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
//					if(!prefabHandler.Contains((Prefab) i)){
//						prefabHandler.AddPrefab(i);
//					}
//					else{
//						ErrorMessage(5, "Prefab already exists in collection");
//					}
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
