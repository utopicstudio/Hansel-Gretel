using UnityEngine;

namespace J
{

	[AddComponentMenu("J/Util/JPrintInConsole")]
	public class JPrint : MonoBehaviour {

		[Tooltip("(Optional) Input Name of a button in Proyect Settings -> Input. If not empty, printText is shown after pressing this key")]
		[SerializeField]	string inputName;
		[SerializeField]	string printText;
		[SerializeField]	PrintType printType;

		public void printTheText() {
			switch (printType) {
			case PrintType.Log:
				Debug.Log (printText);
				break;
			case PrintType.Warning:
				Debug.LogWarning (printText);
				break;
			case PrintType.Error:
				Debug.LogError (printText);
				break;
			default:
				break;
			}

		}

		void Update () {
			if (inputName.Trim().Length > 0 && Input.GetButtonDown(inputName)) {
				printTheText ();
			}

		}

	}


	enum PrintType
	{
		Log, Warning, Error
	}
}