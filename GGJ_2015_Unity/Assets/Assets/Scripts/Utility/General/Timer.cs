using UnityEngine;
using System.Collections;

[System.Serializable]
public class Timer{
	
	private float startTime;
	[SerializeField]
	private float stopwatchTime;
	
	public Timer(){
		Reset();	
	}
	
	public Timer(float time){
		NewStopwatch(time);	
	}
	
	public void NewStopwatch(float time){
		Reset();
		stopwatchTime = time;
	}
	
	public bool StopwatchDone(){
		return (GetTime() > stopwatchTime);	
	}
	
	public float GetTime(){
		return Time.time - startTime;	
	}
	
	public float GetNormalizedTime(){
		return GetNormalizedTime(stopwatchTime);	
	}
	
	public float GetNormalizedTime(float timePeriod){
		if (timePeriod != 0f){
			return Mathf.Clamp(GetTime() / timePeriod, 0, 1.0f);	
		}
		else{
			return 0;
		}
	}
	
	public float GetRawNormalizedTime(){
		return GetRawNormalizedTime(stopwatchTime);	
	}
	
	public float GetRawNormalizedTime(float timePeriod){
		return GetTime() / timePeriod;
	}
	
	public void Reset(){
		startTime = Time.time;	
	}

	// offset essentially delays the start of the timer in seconds
	public void ResetWithOffset(float offset){
		startTime = Time.time + offset;
	}

	// percentOffset delays the start of the timer by a percent of current timer length)
	public void ResetPercentOffset(float percentOffset){
		startTime = Time.time + (stopwatchTime * percentOffset);
	}
	
	public float GetStopwatchLength(){
		return stopwatchTime;	
	}
}
