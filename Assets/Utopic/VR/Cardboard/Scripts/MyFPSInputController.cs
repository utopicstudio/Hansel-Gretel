using UnityEngine;

namespace Utopic.VR.UtopicCardboard
{

	/*
	 * Adaptacion mia (de .js a .cs) de FPSInputController, parte de standard assets de Unity 
	 */

	[DisallowMultipleComponent]
	public class MyFPSInputController : MonoBehaviour {
		
		public enum FPSInputMode {AutoTurning, ManualTurning, HeadTurning, TabletMode}
		public FPSInputMode fpsInputMode;

		[SerializeField] bool showCursor = false;
		private Vector3 directionVector;
		private float directionLength;
		private float yRotation;


		KeyCode optionsButton, L1, R1;

		void Start () {
			Cursor.visible = showCursor;

			// Teclas para dualshock en Android de Pedro
			optionsButton = KeyCode.JoystickButton6;
			L1 = KeyCode.JoystickButton3;
			R1 = KeyCode.JoystickButton14;

			if (Application.isEditor) {
				optionsButton = KeyCode.JoystickButton8;
				L1 = KeyCode.JoystickButton4;
				R1 = KeyCode.JoystickButton5;
			}
		}

		// Update is called once per frame
		void Update () {
			
			if (Input.GetKeyDown (KeyCode.M) || Input.GetKeyDown (optionsButton))
				fpsInputMode = (FPSInputMode)( ((int)fpsInputMode + 1) % System.Enum.GetValues(typeof(FPSInputMode)).Length );

			

			switch (fpsInputMode) {
			case FPSInputMode.AutoTurning:
				directionVector = new Vector3 (0, 0, Input.GetAxis ("Vertical"));
				yRotation = Input.GetAxis ("Horizontal");
				break;

			case FPSInputMode.ManualTurning:
				directionVector = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));	

				if (Input.GetKey (L1) && Input.GetKey (R1))
					yRotation = yRotation < 0 ? 1f : -1f;
				else {
					yRotation = Input.GetKey (L1) ? -1f : 0f;
					yRotation = Input.GetKey (R1) ? 1f : yRotation;
				}
				if (Input.GetKey (KeyCode.Q) && Input.GetKey (KeyCode.E))
					yRotation = yRotation < 0 ? 1f : -1f;
				else if (!Input.GetKey (L1) && !Input.GetKey (R1)) {
					yRotation = Input.GetKey (KeyCode.Q) ? -1f : 0f;
					yRotation = Input.GetKey (KeyCode.E) ? 1f : yRotation;
				}
				break;

			case FPSInputMode.HeadTurning:
				directionVector = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));	
				break;
			case FPSInputMode.TabletMode:

				directionVector = new Vector3 (UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis ("Horizontal"), 0, UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis ("Vertical"));
				break;
			default:
				break;
			}

			if (directionVector != Vector3.zero) {
				// Get the length of the directon vector and then normalize it
				// Dividing by the length is cheaper than normalizing when we already have the length anyway
				directionLength = directionVector.magnitude;
				directionVector = directionVector / directionLength;

				// Make sure the length is no bigger than 1
				directionLength = Mathf.Min(1, directionLength);

				// Make the input vector more sensitive towards the extremes and less sensitive in the middle
				// This makes it easier to control slow speeds when using analog sticks
				directionLength = directionLength * directionLength;

				// Multiply the normalized direction vector by the modified length
				directionVector = directionVector * directionLength;
			}
			// Apply the direction to the CharacterMotor
			SendMessage("setInputMoveDirection", transform.rotation * directionVector);
			if (fpsInputMode == FPSInputMode.TabletMode)
				SendMessage("setInputJump", UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButton ("Jump"));
			else
				SendMessage("setInputJump", Input.GetButton ("Jump"));
			//SendMessage ("setYRotationAmmount", yRotation);
		}//END_UPDATE()
	}


}//END_NAMESPACE