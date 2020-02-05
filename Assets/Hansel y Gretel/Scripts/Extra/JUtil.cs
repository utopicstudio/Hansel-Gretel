using UnityEngine;

namespace J
{

	public class JUtil : Object {

		/*
		 * Creates empty object if name is not already in the scene.
		 * If it was already there, the 'setParent' is not used to change it's parent.
		 */
		public static GameObject JCreateEmptyGameObjectNoDuplicates(string name, Transform setParent = null) {
			GameObject newGameObject;
			newGameObject = GameObject.Find(name);
			if (!newGameObject) {
				newGameObject = new GameObject(name);
				if (setParent)
					newGameObject.transform.parent = setParent;
			}
			return newGameObject;
		}
	}

}