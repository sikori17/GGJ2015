using UnityEngine;
using System.Collections;

// NOTE: Need custom editor for edits of volume and mute in editor to apply immediately
public class AudioSpeaker : MonoBehaviour{
	[SerializeField]
	private AudioSource speaker;
	[SerializeField]
	private AudioObject info;
	[SerializeField]
	private float volume = 1.0f;
	[SerializeField]
	private bool mute = false;
	
	public void Init(){
		speaker = gameObject.GetComponent<AudioSource>();
	}
	
	public void Init(AudioSource source){
		speaker = source;
	}

	public void Reset(){
		volume = 1.0f;
		mute = false;
	}
	
	public void SetAudioObject(Audio audio){

		info = AudioHandler.Instance.GetAudioObject(audio);
		info.RegisterAudioSpeaker(this);
		
		speaker.clip = info.GetClip();
	}
	
	public void SetVolume(float percentOfVolume){
		speaker.volume = volume * percentOfVolume;
	}

	public void SetDefaultVolume(float volume){
		this.volume = volume;
		SetVolume(info.currentVolume);
	}
	
	public void Mute(bool state){
		// If global mute, class mute, or personal mute is on, mute the speaker
		if(AudioHandler.Instance.mute || info.mute || state){
			speaker.mute = true;
		}
		else{
			speaker.mute = mute;
		}
	}

	public void SetDefaultMute(bool state){
		mute = state;
		Mute(mute);
	}
	
	public void SetLooped(bool state){
		speaker.loop = state;
	}
	
	public bool IsPlaying(){
		return speaker.isPlaying;
	}
	
	public bool IsActive(){
		return speaker.gameObject.activeInHierarchy;
	}
	
	public void SetActive(bool state){
		speaker.enabled = state;
		ManageVolumeEvent(state);
	}
	
	public void SetGameobjectActive(bool state){
		speaker.gameObject.SetActive(state);
		ManageVolumeEvent(state);
	}
	
	private void ManageVolumeEvent(bool state){
		if(!state){ 
			if(info != null) info.UnregisterAudioSpeaker(this);
		}
	}
	
	public void SetPosition(Vector3 location){
		speaker.gameObject.transform.position = location;
	}
	
	public void Play(){
		speaker.Play();
	}
	
	public void Stop(){
		speaker.Stop();
	}
}
