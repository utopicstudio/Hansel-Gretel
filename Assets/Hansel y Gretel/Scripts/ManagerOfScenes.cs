using UnityEngine;
using System.Collections;

public class ManagerOfScenes : Singleton<ManagerOfScenes> {
	protected ManagerOfScenes() {}

	[RangeAttribute(0f,3f)]
	public float loadDelay = 1f;

	public void NextScene() {
		StartCoroutine (loadSceneIEnumerator (1));
	}
	public void PreviousScene() {
		StartCoroutine (loadSceneIEnumerator (-1));
	}
	private IEnumerator loadSceneIEnumerator(int offset) {
		yield return new WaitForSeconds (loadDelay);
		UnityEngine.SceneManagement.Scene activeScene;
		activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene ();
		int index = activeScene.buildIndex;
		if (index != -1) {
			index += offset;
			if (index < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
				UnityEngine.SceneManagement.SceneManager.LoadScene (index);
		}
	}
}
