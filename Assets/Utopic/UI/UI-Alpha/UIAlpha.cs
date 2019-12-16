using UnityEngine;

[RequireComponent(typeof(UnityEngine.CanvasGroup))]
[RequireComponent(typeof(Animator))]
public class UIAlpha : MonoBehaviour {

	enum UIAlphaStartMode {invisible, visible, visibleFadeIn}
	[SerializeField]
	UIAlphaStartMode startVisibility = UIAlphaStartMode.visible;

	[HideInInspector]
	public Animator animator;
	public float fadeSpeedMultiplier = 1f;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		animator.SetFloat ("fadeSpeed", fadeSpeedMultiplier);
		switch (startVisibility) {
			case UIAlphaStartMode.invisible:
				animator.SetTrigger("hideInstantly");
				break;
			case UIAlphaStartMode.visible:
				break;
			case UIAlphaStartMode.visibleFadeIn:
				animator.SetTrigger("hideInstantly");
				animator.SetTrigger("show");
				break;
		}
	}

	public void Show() {
		animator.SetTrigger("show");
	}
	public void Hide() {
		animator.SetTrigger("hide");
	}
	public void ShowInstantly() {
		animator.SetTrigger("showInstantly");
	}
	public void HideInstantly() {
		animator.SetTrigger("hideInstantly");
	}
	public void SetFadeSpeed(float f) {
		this.fadeSpeedMultiplier = f;
		animator.SetFloat ("fadeSpeed", fadeSpeedMultiplier);
	}
}
