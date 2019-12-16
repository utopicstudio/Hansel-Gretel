using UnityEngine;
using System.Collections;

public class MyManager : MonoBehaviour {

	//this class is a singleton)
	public static MyManager Instance {get; private set;}


	[RangeAttribute(0f,10f)]
	public float timeScale = 1f;
	public bool findTagPlayer = true;

	public GameObject playerGameObject { get; set;}
	public MyCameraManager cameraManager { get; private set;}
	public MyLevelManager levelManager { get; private set;}
	public MyAudioManager audioManager { get; private set;}




	void Awake () {
		// Singleton:
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy (this.gameObject); // Nota: Esto podria eliminar todo un objeto, con otros componentes
		DontDestroyOnLoad (gameObject);
		// End Singleton


		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLevelLoaded;


		levelManager = GetComponent<MyLevelManager> ();
		cameraManager = GetComponent<MyCameraManager> ();
		audioManager = GetComponent<MyAudioManager> ();

		if (!(levelManager && cameraManager && audioManager)) {
			Debug.LogError ("MyError: MyManager coudn't find all the components and references.");
		}
	}


	// Runs each time a scene is loaded, even when script is using DontDestroyOnLoad().
	// Finds a GameObject tagged with 'Player'. Needs to find exactly one.
	void OnLevelLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode) {
		if (findTagPlayer) {
			playerGameObject = null;
			GameObject[] goArr = GameObject.FindGameObjectsWithTag ("Player");
			if (goArr.Length > 0) {
				playerGameObject = goArr [0];
			}
		}

		Time.timeScale = this.timeScale;
	}

	void OnValidate() {
		Time.timeScale = this.timeScale;
	}
}
