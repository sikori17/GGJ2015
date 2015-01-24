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
	
	void Init(){

		initialized = true;

		AudioClassEnums = Enum.GetNames(typeof(AudioClass));
		AudioEnums = Enum.GetNames(typeof(Audio));

		audioHandler = (AudioHandler) target;
		audioHandler.EditorInit();
	}
	
	void OnEnable(){
		if(!initialized || AudioHandler.Instance == null){Init();}
		//ReloadAudioClips();
		SyncWithEnum();
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
		if(audioHandler.GetAudioObjects().Length != (int) Audio.Length) SyncWithEnum();
		
		GUILayout.BeginVertical();

		GUILayout.Space(10);

		GUILayout.Box("Audio Classes", EditorStyles.whiteLargeLabel);
		GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
		
		for(int i = 0; i < AudioClassEnums.Length - 1; i++){

			GUILayout.BeginHorizontal();
			GUILayout.Label(AudioClassEnums[i], EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			bool mute = audioHandler.GetAudioClassMute((AudioClass) i);
			bool state = GUILayout.Toggle(mute , "Mute", GUILayout.MinWidth(30));
			if(state != mute) audioHandler.SetAudioClassMute((AudioClass) i, state);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			float volume = audioHandler.GetAudioClassVolume((AudioClass) i);

			string curVol = "" + volume;
			GUILayout.Label(curVol, GUILayout.MaxWidth(35));

			float newVolume = GUILayout.HorizontalSlider(volume, 0, 1.0f);
			if(volume != newVolume) audioHandler.SetAudioClassVolume((AudioClass) i, newVolume); 

			GUILayout.EndHorizontal();
		}

		GUILayout.Space(20);

		GUILayout.Box("Audio Objects", EditorStyles.whiteLargeLabel);

		for(int i = 0; i < (int) Audio.Length; i++){

			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

			GUILayout.BeginHorizontal();
			GUILayout.Label(((Audio) i).ToString(), EditorStyles.boldLabel);
			bool mute = audioHandler.GetAudioObject((Audio) i).mute;
			GUILayout.FlexibleSpace();

			bool state = GUILayout.Toggle(audioHandler.GetAudioObject((Audio) i).mute, "Mute", GUILayout.MinWidth(60), GUILayout.MinHeight(20));
			if(mute != state) audioHandler.GetAudioObject((Audio) i).SetDefaultMute(state);

			bool preload = GUILayout.Toggle(audioHandler.GetAudioObjectPreload((Audio) i), "Preload", GUILayout.MinWidth(60));
			audioHandler.SetAudioObjectPreload((Audio) i, preload);

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			AudioClass audioClass = (AudioClass) EditorGUILayout.EnumPopup(audioHandler.GetAudioObjectClass((Audio) i));
			audioHandler.SetAudioObjectClass((Audio) i, audioClass);

			/*
			GUILayout.FlexibleSpace();
			bool preload = GUILayout.Toggle(audioHandler.GetAudioObjectPreload((Audio) i), "Preload", GUILayout.MinWidth(70));
			audioHandler.SetAudioObjectPreload((Audio) i, preload);
			*/
			GUILayout.EndHorizontal();

			AudioClip clip = null;

			// if clip is already set
			if(audioClips[i] != null){
				GUILayout.BeginHorizontal();
				clip = (AudioClip) EditorGUILayout.ObjectField(audioClips[i], typeof(AudioClip), false);
				//bool remove = GUILayout.Button("Remove");
				//if(remove){
				//	audioClips[i] = null;
				//	audioHandler.ResetAudioObject((Audio) i);
				//}
				if(clip == null){
					audioClips[i] = null;
					audioHandler.ResetAudioObject((Audio) i);
				}
				else if(clip != audioClips[i]){
					AttemptClipAdd((Audio) i, clip);
				}

				GUILayout.EndHorizontal();
			}
			else{
				clip = (AudioClip) EditorGUILayout.ObjectField(null, typeof(AudioClip), false);
				if(clip != null){
					AttemptClipAdd((Audio) i, clip);
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

	private void AttemptClipAdd(Audio audio, AudioClip clip){

		string path = AssetDatabase.GetAssetPath(clip);
		string end = "Resources/" + clip.name;

		if(path.EndsWith(end + ".wav") || path.EndsWith(end + ".mp3") || path.EndsWith(end + ".ogg")){
			audioClips[(int) audio] = clip;
			audioHandler.SetAudioObjectName(audio, clip.name);
		}
		else{
			Debug.Log("<color=red> ERROR: AudioClip with name '" + clip.name + "' is not in Resources.</color>");
		}
	}

	private void SyncWithEnum(){

		AudioObject[] audioObjects = audioHandler.GetAudioObjects();
		AudioObject[] newAudioObjectsArray = new AudioObject[(int) Audio.Length];

		// Names of current Audio enums
		List<string> enumNames = new List<string>(Enum.GetNames(typeof(Audio)));

		// Transfer existing clips - Handles reordering of enums
		for(int i = 0; i < audioObjects.Length; i++){
			// If this audio object still exists in enum list
			if(enumNames.Contains(audioObjects[i].audioEnumName)){
				// Get correct Audio enum by parsing stored string enum name
				Audio audioEnum = (Audio) Enum.Parse(typeof(Audio), audioObjects[i].audioEnumName);
				// Transfer audio object
				newAudioObjectsArray[(int) audioEnum] =  audioObjects[i];
			}
		}

		// Fill in missing clips with new empty AudioObjects
		for(int i = 0; i < newAudioObjectsArray.Length; i++){
			if(newAudioObjectsArray[i] == null){
				newAudioObjectsArray[i] = new AudioObject((Audio) i);
			}
		}

		// Resetting local audioClips array used for editor display
		audioClips = new AudioClip[(int) Audio.Length];
		string audioName = "";
		for(int i = 0; i < audioClips.Length; i++){
			audioName = audioHandler.GetAudioObjectName((Audio) i);
			if(audioName != "") audioClips[i] = (AudioClip) Resources.Load(audioName);
		}

		audioHandler.SetAudioObjects(newAudioObjectsArray);
	}
}
