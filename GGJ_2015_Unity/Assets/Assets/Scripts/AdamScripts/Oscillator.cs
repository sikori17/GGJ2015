using UnityEngine;
using System.Collections;

[System.Serializable]
public class Oscillator {
	public float period, min, max;
	
	private float A, f, mid;
	private float t, disp;
	private float startTime;
	
	public Oscillator(float period, float min, float max) {
		this.period = period;
		this.min = min;
		this.max = max;
		
		Restart();
	}
	
	public Oscillator() : this(1, 0, 1) {}
	
	public void Restart() {
		Calculations();
		startTime = Time.time;
	}
	
	public float Displacement() {
		t = (Time.time - this.startTime);
		disp = mid + ( A * Mathf.Sin( (2 * Mathf.PI * f * t) - 0.5f ));
		return disp;
	}
	
	void Calculations() {
		this.A = (this.max - this.min) / 2f; //amplitude
		this.mid = this.min + this.A; //the middle
		this.f = 1f / this.period; //frequency
	}	
}
