using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// All audio clips are enumerated here
// Enum name should match the name of the clip in Resources
public enum Audio{
	bump = 0,
	sword,
	playerHurt,
	enemyHurt,
	enemyDie,
	bumpSword,
	fireball,
	playerDie,
	warningBeep,
	lootPoint,
	Length
}

// Every Audio clip belongs an enumerated AudioClass
// See AudioClassSetting.cs for details
public enum AudioClass{
	Test,
	TestTwo,
	Length
}

// AudioHandler provides optimized services for playing multiple 2D and 3D sounds.
// AudioHandler contains a collection of AudioClassSettings, AudioObjects, and AudioSpeakers
// AudioObjects belong to a particular AudioClassSetting. AudioSpeakers register to AudioObjects
// AudioClassSetting, AudioObject, and AudioSpeaker allow volume and mute to be set independently,
// Although AudioClassSetting's mute overrides AudioObject's, and AudioObject's overrides AudioSpeaker's (cascading)
// Volumes are combined multiplicatively
public class AudioHandler : BaseBehaviour {
	
	// Singleton reference
	public static AudioHandler Instance;
	private bool initialized;

	[SerializeField] //[HideInInspector]
	private bool editorInitialized;
	[SerializeField] [HideInInspector]
	private AudioClassSetting[] settings;
	[SerializeField] [HideInInspector]
	private AudioObject[] objects;

	private bool globalMute = false;
	public bool mute{
		get{
			return globalMute;
		}
	}

	// 2D
	public int poolSize2D;
	private GameObject Root_2D;
	private Queue<AudioSpeaker> speakers2D_Open;
	private LinkedList<AudioSpeaker> speakers2D_Playing;

	// 3D
	public int poolSize3D;
	private GameObject Root_3D;
	private Queue<AudioSpeaker> speakers3D_Open;
	private LinkedList<AudioSpeaker> speakers3D_Playing;

	// Dedicated Audio Speakers
	public int poolSizeDedicated;
	private GameObject Root_Dedicated;
	private Queue<AudioSpeaker> speakersDedicated;

	// Utility list for recycling process
	List<LinkedListNode<AudioSpeaker>> toRemove;

	// Use this for initialization
	void Awake () {
		Init();
	}

	// Set up the AudioHandler singleton and internal vars
	public void Init(){
		if(!initialized){

			Instance = this;
			initialized = true;

			Setup2D();
			Setup3D();
			SetupDedicated();

			InitAudioObjects();

			PreloadClips();
		}
		else if(Instance != this){
			GameObject.Destroy(this.gameObject);	
		}
	}

	private void Setup2D(){

		Root_2D = new GameObject("Root_2D");
		Root_2D.transform.position = transform.position;
		Root_2D.transform.parent = transform;

		speakers2D_Open = new Queue<AudioSpeaker>(poolSize2D);
		speakers2D_Playing = new LinkedList<AudioSpeaker>();

		Increase2DQueueSize(poolSize2D);
	}

	private void Setup3D(){

		Root_3D = new GameObject("Root_3D");
		Root_3D.transform.position = transform.position;
		Root_3D.transform.parent = transform;
		
		speakers3D_Open = new Queue<AudioSpeaker>(poolSize3D);
		speakers3D_Playing = new LinkedList<AudioSpeaker>();

		Increase3DQueueSize(poolSize3D);
	}

	private void SetupDedicated(){

		Root_Dedicated = new GameObject("Root_Dedicated");
		Root_Dedicated.transform.position = transform.position;
		Root_Dedicated.transform.parent = transform;

		speakersDedicated = new Queue<AudioSpeaker>(poolSizeDedicated);

		GameObject speakerDedicated;
		AudioSpeaker speaker;
		for(int i = 0; i < poolSizeDedicated; i++){
			speakerDedicated = new GameObject("Speaker_Dedicated", typeof(AudioSource));
			speakerDedicated.SetActive(false);
			speakerDedicated.transform.parent = Root_Dedicated.transform;
			speaker = speakerDedicated.AddComponent<AudioSpeaker>();
			speaker.Init();
			speakersDedicated.Enqueue(speaker);
		}
	}

	private void InitAudioObjects(){
		for(int i = 0; i < objects.Length; i++){
			objects[i].Init();
		}
	}

	private void PreloadClips(){
		for(int i = 0; i < (int) Audio.Length; i++){
			if(objects[i].preload && objects[i].nameInResources != ""){
				objects[i].LoadClip();
			}
		}
	}

	public override void Update(){
		RecycleAudio2D();
		RecycleAudio3D();
	}

	// Runs through speakers and flags any that have finished playing for recycling,
	// then removes them and enters them back into open queue
	private void RecycleAudio2D(){
		if(speakers2D_Playing.Count > 0){
			
			toRemove = new List<LinkedListNode<AudioSpeaker>>();
			
			for(LinkedListNode<AudioSpeaker> speaker = speakers2D_Playing.First; speaker != speakers2D_Playing.Last.Next; speaker = speaker.Next){
				if(!speaker.Value.IsPlaying()){
					toRemove.Add(speaker);
				}
			}
			
			for(int i = 0; i < toRemove.Count; i++){
				speakers2D_Playing.Remove(toRemove[i]);
				toRemove[i].Value.SetActive(false);
				toRemove[i].Value.Reset();
				speakers2D_Open.Enqueue(toRemove[i].Value);
			}
		}
	}


	// Runs through speakers and flags any that have finished playing for recycling,
	// then removes them and enters them back into open queue
	private void RecycleAudio3D(){
		if(speakers3D_Playing.Count > 0){
			
			toRemove = new List<LinkedListNode<AudioSpeaker>>();
			
			for(LinkedListNode<AudioSpeaker> speaker = speakers3D_Playing.First; speaker != speakers3D_Playing.Last.Next; speaker = speaker.Next){
				if(!speaker.Value.IsPlaying()){
					toRemove.Add(speaker);
				}
			}
			
			for(int i = 0; i < toRemove.Count; i++){
				speakers3D_Playing.Remove(toRemove[i]);
				toRemove[i].Value.SetGameobjectActive(false);
				toRemove[i].Value.Reset();
				speakers3D_Open.Enqueue(toRemove[i].Value);
			}
		}
	}
	
	// Individual play functions

	public static void Play(Audio sound){
		AudioSpeaker speaker = Instance.PullSpeaker2D();

		speaker.SetAudioObject(sound);
		speaker.SetLooped(false);
		speaker.Play();
	}

	public static void Play(Audio sound, Vector3 location){
		AudioSpeaker speaker = Instance.PullSpeaker3D();
		speaker.SetAudioObject(sound);
		speaker.SetLooped(false);
		speaker.SetPosition(location);
		speaker.Play();
	}

	public static AudioSpeaker PlayLooped(Audio sound){
		AudioSpeaker speaker = Instance.PullSpeaker2D();
		speaker.SetAudioObject(sound);
		speaker.SetLooped(true);
		speaker.Play();
		return speaker;
	}

	public static AudioSpeaker PlayLooped(Audio sound, Vector3 location){
		AudioSpeaker speaker = Instance.PullSpeaker3D();
		speaker.SetAudioObject(sound);
		speaker.SetLooped(true);
		speaker.SetPosition(location);
		speaker.Play();
		return speaker;
	}

	public AudioSpeaker PullSpeaker2D(){
		if(speakers2D_Open.Count == 0) Increase2DQueueSize(poolSize2D);
		AudioSpeaker speaker = speakers2D_Open.Dequeue();
		speaker.SetActive(true);
		speakers2D_Playing.AddLast(speaker);
		return speaker;
	}

	public AudioSpeaker PullSpeaker3D(){
		if(speakers3D_Open.Count == 0) Increase3DQueueSize(poolSize3D);
		AudioSpeaker speaker = speakers3D_Open.Dequeue();
		speaker.SetGameobjectActive(true);
		speakers3D_Playing.AddLast(speaker);
		return speaker;
	}

	public static AudioSpeaker GetDedicatedAudioSpeaker(){
		AudioSpeaker speaker = Instance.speakersDedicated.Dequeue();
		speaker.SetActive(true);
		return speaker;
	}

	public static void ReturnDedicatedAudioSpeaker(ref AudioSpeaker speaker){
		speaker.SetActive(false);
		speaker.Reset();
		speaker.transform.parent = Instance.Root_Dedicated.transform;
		Instance.speakersDedicated.Enqueue(speaker);
		speaker = null;
	}

	private AudioObject RetrieveAudio(Audio sound){

		AudioObject audioInfo = objects[(int) sound];

		if(audioInfo.clip == null){
			audioInfo.LoadClip();
		}

		return audioInfo;
	}

	// Other

	public static void RegisterAudioObject(AudioObject audioObject){
		Instance.settings[(int) audioObject.audioClass].UpdateVolume += audioObject.SetVolume;
		Instance.settings[(int) audioObject.audioClass].UpdateMute += audioObject.Mute;
	}

	public static void UnregisterAudioObject(AudioObject audioObject){
		Instance.settings[(int) audioObject.audioClass].UpdateVolume -= audioObject.SetVolume;
		Instance.settings[(int) audioObject.audioClass].UpdateMute -= audioObject.Mute;
	}

	public static void SetClassVolume(AudioClass audioClass, float volume){
		Instance.settings[(int) audioClass].SetVolume(volume);
	}

	public static void SetAudioVolume(Audio audio, float volume){
		Instance.objects[(int) audio].SetDefaultVolume(volume);
	}
	
	public static void MuteVolume(bool muteOn){
		Instance.SetMute(muteOn);
	}

	private void SetVolume(float volume){
		for(int i = 0; i < settings.Length; i++){
			settings[i].SetVolume(settings[i].volume * volume);
		}
	}
	
	private void SetMute(bool on){

		globalMute = on;

		for(int i = 0; i < settings.Length; i++){
			settings[i].Mute(globalMute);
		}
	}

	#region Queue

	private void Increase2DQueueSize(int count){

		AudioSource source;
		AudioSpeaker speaker;

		for(int i = 0; i < count; i++){
			source = Root_2D.AddComponent<AudioSource>();
			source.enabled = false;
			speaker = Root_2D.AddComponent<AudioSpeaker>();
			speaker.Init(source);
			speakers2D_Open.Enqueue(speaker);
		}
	}

	private void Increase3DQueueSize(int count){

		GameObject speaker3D;
		AudioSpeaker speaker;

		for(int i = 0; i < count; i++){
			speaker3D = new GameObject("Speaker_3D", typeof(AudioSource));
			speaker3D.SetActive(false);
			speaker3D.transform.parent = Root_3D.transform;
			speaker = speaker3D.AddComponent<AudioSpeaker>();
			speaker.Init();
			speakers3D_Open.Enqueue(speaker);
		}
	}

	#endregion

	#region Editor

	public void EditorInit(){

		Instance = this;

		if(!editorInitialized){

			editorInitialized = true;

			Debug.Log("Audio Handler Editor Initialize");
			
			settings = new AudioClassSetting[(int) AudioClass.Length];
			for(int i = 0; i < (int) AudioClass.Length; i++){
				settings[i] = new AudioClassSetting();
			}
			
			objects = new AudioObject[(int) Audio.Length];
			for(int i = 0; i < (int) Audio.Length; i++){
				objects[i] = new AudioObject((Audio) i);
			}
		}
	}
	
	public void EditorRefresh(){
		Debug.Log("Audio Handler Editor Refresh");
		RefreshClassArray(ref settings);
		RefreshAudioArray(ref objects);
		CheckAudioClipExistance();
	}

	private void RefreshClassArray(ref AudioClassSetting[] array){
		int length = (int) AudioClass.Length;
		if(array.Length != length){
			AudioClassSetting[] newArray = new AudioClassSetting[length];
			if(length > array.Length){
				array.CopyTo(newArray, 0);
			}
			else{
				for(int i = 0; i < length; i++){
					newArray[i] = array[i];
				}
			}
			array = newArray;
		}
	}

	private void RefreshAudioArray(ref AudioObject[] array){
		int length = (int) Audio.Length;
		if(array.Length != length){
			AudioObject[] newArray = new AudioObject[length];
			if(length > array.Length){
				array.CopyTo(newArray, 0);
			}
			else{
				for(int i = 0; i < length; i++){
					newArray[i] = array[i];
				}
			}
			array = newArray;
		}
	}

	private void CheckAudioClipExistance(){
		for(int i = 0; i < objects.Length; i++){
			if(objects[i].nameInResources != ""){
				AudioClip clip = Resources.Load(objects[i].nameInResources) as AudioClip;
				if(clip == null){
					objects[i].Reset();
				}
			}
		}
	}

	public bool GetAudioClassMute(AudioClass audioClass){
		return settings[(int) audioClass].mute;
	}

	public float GetAudioClassVolume(AudioClass audioClass){
		return settings[(int) audioClass].volume;
	}

	public void SetAudioClassMute(AudioClass audioClass, bool state){
		settings[(int) audioClass].Mute(state);
	}
	
	public void SetAudioClassVolume(AudioClass audioClass, float volume){
		settings[(int) audioClass].SetVolume(volume);
	}

	public AudioClass GetAudioObjectClass(Audio audio){
		return objects[(int) audio].audioClass;
	}

	public void SetAudioObjectClass(Audio audio, AudioClass audioClass){
		objects[(int) audio].audioClass = audioClass;
	}

	public AudioObject[] GetAudioObjects(){
		return objects;
	}

	public void SetAudioObjects(AudioObject[] objects){
		this.objects = objects;
	}

	public AudioObject GetAudioObject(Audio audio){
		return objects[(int) audio];
	}

	public AudioClip GetAudioObjectClip(Audio audio){
		return objects[(int) audio].clip;
	}

	public bool GetAudioObjectMute(Audio audio){
		return objects[(int) audio].mute;
	}

	public void SetAudioObjectClip(Audio audio, AudioClip clip){
		objects[(int) audio].clip = clip;
	}

	public string GetAudioObjectName(Audio audio){
		return objects[(int) audio].nameInResources;
	}
	
	public void SetAudioObjectName(Audio audio, string name){
		objects[(int) audio].nameInResources = name;
	}

	public bool GetAudioObjectPreload(Audio audio){
		return objects[(int) audio].preload;
	}

	public void SetAudioObjectPreload(Audio audio, bool state){
		objects[(int) audio].preload = state;
	}

	public float GetAudioObjectDefaultVolume(Audio audio){
		return objects[(int) audio].defaultVolume;
	}

	public void SetAudioObjectDefaultVolume(Audio audio, float volume){
		objects[(int) audio].SetDefaultVolume(volume);
	}

	public void ResetAudioObject(Audio audio){
		objects[(int) audio].Reset();
	}

	#endregion
}
