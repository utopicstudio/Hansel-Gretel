using UnityEngine;
using System.Collections;

// It starts deactivated.
// public: Show(), Hide(), ShowInstantly(), HideInstantly().
public class TextUtopic : TextVR {

    public TextController textController;
	public AudioSource textSound;
    
    public override void ShowAndHideOthers()
    {
        print("activando "+gameObject);
        //textController.HideAll();
        gameObject.SetActive(true);
        print(gameObject.activeSelf + " :: " + gameObject.name);
        textSound.Play();
    }
    public override void Show()
    {
        gameObject.SetActive(true);
        textSound.Play();
    }
    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
