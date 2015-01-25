using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioObject{
	
	public Audio name;
	public string audioEnumName; // For syncing with AudioHandlerEditor
	public AudioClass audioClass;
	public AudioClip clip;
	public string nameInResources;
	public bool preload;
	public float defaultVolume = 1.0f;
	public float currentVolume = 1.0f;
	public bool mute;
	
	public FloatParamDelegate UpdateVolume;
	public BoolParamDelegate UpdateMute;

	public AudioObject(Audio audio){
		name = audio;
		audioEnumName = audio.ToString();
	}
	
	public void Init(){
		AudioHandler.RegisterAudioObject(this);
	}
	
	public void LoadClip(){
		clip = (AudioClip) Resources.Load(nameInResources);
	}

	// Updates all audio objects to match volume
	public void SetVolume(float percentOfDefaultVolume){
		currentVolume = defaultVolume * percentOfDefaultVolume;
		if(UpdateVolume != null){ UpdateVolume(currentVolume);}
	}

	// Sets this audio object's personal volume.
	// Multiplicative effect with AudioClass volume - ie (0.5 AudioObject volume + 0.5 AudioClass volume) = 0.25 
	public void SetDefaultVolume(float volume){
		defaultVolume = Mathf.Clamp01(volume);
		SetVolume(AudioHandler.Instance.GetAudioClassVolume(audioClass));
	}

	// Updates all AudioObjects to match mute state
	public void Mute(bool state){
		if(UpdateMute != null){ UpdateMute(state);}
	}
	
	// Sets this audio object's personal mute.
	// Can be overridden by AudioClass' mute.
	public void SetDefaultMute(bool state){
		mute = state;
		Mute(mute);
	}

	// Registers a speaker's volume and mute update delegates and applies current settings
	public void RegisterAudioSpeaker(AudioSpeaker speaker){

		UpdateVolume += speaker.SetVolume;
		UpdateMute += speaker.Mute;

		speaker.SetVolume(currentVolume);
		speaker.Mute(mute);
	}

	// Unregisters speaker's volume and mute delegates
	public void UnregisterAudioSpeaker(AudioSpeaker speaker){
		UpdateVolume -= speaker.SetVolume;
		UpdateMute -= speaker.Mute;
	}

	public AudioClip GetClip(){
		if(clip == null) LoadClip();
		return clip;
	}
	
	public void Reset(){
		Debug.Log("Reset: " + nameInResources);
		audioClass = (AudioClass) 0;
		clip = null;
		nameInResources = "";
		preload = false;
		defaultVolume = 1.0f;
		currentVolume = 1.0f;
	}
}
