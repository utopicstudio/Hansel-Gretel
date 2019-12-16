using UnityEngine;
using UnityEngine.SceneManagement;

public class MyLevelManager : MonoBehaviour {

	public bool fadeIn = true;
	public bool fadeOut = true;

	private AsyncOperation asyncOperation;

	void Awake () {
		SceneManager.sceneLoaded += OnLevelLoaded;
	}

	// Runs each time a scene is loaded, even when script is using DontDestroyOnLoad().
	void OnLevelLoaded(Scene scene, LoadSceneMode mode) {
		if (fadeIn)
			MyManager.Instance.cameraManager.FadeIn ();
	}

	void Update () {
		if (asyncOperation != null && asyncOperation.isDone) {
			Debug.Log ("Async Loading of the next level is complete");
		}
		if (asyncOperation != null) {
			Debug.Log ("Async Loading Progress: " + (int)(asyncOperation.progress * 100) + "%");
		}
	}

	public void PreLoadScene (string name) {
		asyncOperation = SceneManager.LoadSceneAsync (name, LoadSceneMode.Single);
		asyncOperation.allowSceneActivation = false;
	}

	public void LoadScene (string name, bool fadeOutMusic = false) {
		StartCoroutine (_LoadScene (name, fadeOutMusic));
	}
	System.Collections.IEnumerator _LoadScene (string name, bool fadeOutMusic) {
		if (fadeOut) {
			MyManager.Instance.cameraManager.FadeOut ();
			if (fadeOutMusic)
				MyManager.Instance.audioManager.StopMusic (MyManager.Instance.cameraManager.fadeOutTime);
			yield return new WaitForSeconds (MyManager.Instance.cameraManager.fadeOutTime);
		}
		SceneManager.LoadScene (name);
	}


	public void ReplayLevel() {
		this.LoadScene (GetCurrentSceneName());
	}


	public bool IsThisTheLastLevel () {
		return SceneManager.sceneCountInBuildSettings - 1 == GetCurrentSceneIndex ();
	}

	private int GetCurrentSceneIndex()
	{
		return SceneManager.GetActiveScene ().buildIndex;
	}
	private string GetCurrentSceneName()
	{
		return SceneManager.GetActiveScene ().name;
	}

}
