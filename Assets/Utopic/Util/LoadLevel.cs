using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	public string SceneName;
	public bool fadeOutMusic = false;
	public bool asyncLoad = true;

	void Start () {
		if (asyncLoad && SceneName != null && SceneName != "") {
			MyManager.Instance.levelManager.PreLoadScene (SceneName);
		}
	}

	public void LoadTheLevel () {	
		if (SceneName != null && SceneName != "") {
			MyManager.Instance.levelManager.LoadScene (SceneName, fadeOutMusic);
		}
	}
}
