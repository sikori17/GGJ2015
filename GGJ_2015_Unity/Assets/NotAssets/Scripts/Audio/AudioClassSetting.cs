using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioClassSetting{
	
	public bool mute;
	public float volume = 1.0f;

	public FloatParamDelegate UpdateVolume;
	public BoolParamDelegate UpdateMute;
	
	public void SetVolume(float volume){
		this.volume = Mathf.Clamp01(volume);
		if(UpdateVolume != null){ UpdateVolume(volume);}
	}
	
	public void Mute(bool state){
		mute = state;
		if(UpdateMute != null){ UpdateMute(state);}
	}
}
