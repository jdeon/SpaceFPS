using UnityEngine;
using System.Collections;

public class UtilsObjet : MonoBehaviour {

	public static void setWorldScale (Transform transformObjet, Vector3 taille){
		Transform transformParent = transformObjet.parent;
		Vector3 actualWorldScale = transformObjet.localScale;

		while (null != transformParent) {
			for (int indexVector = 0; indexVector < 3; indexVector++) {
				actualWorldScale [indexVector] *= transformParent.localScale [indexVector];
			}
			transformParent = transformParent.parent;
		}

		Vector3 newLocalScale = Vector3.one;

		for (int indexVector = 0; indexVector < 3; indexVector++) {
			newLocalScale[indexVector] = transformObjet.localScale[indexVector] * taille[indexVector] / actualWorldScale[indexVector];
		}

		transformObjet.localScale = newLocalScale;
	}
}