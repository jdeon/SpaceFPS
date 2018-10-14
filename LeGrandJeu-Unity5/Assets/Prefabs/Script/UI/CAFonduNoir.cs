using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CAFonduNoir : CustomActionScript {

	public float tempsFondu;
	public Image fadeImage;
	public Text fadeText;
	public bool apparission;

	private float alpha;
	private int fonduDirection;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		alpha = apparission ? 0.0f : 1.0f;
		fonduDirection = apparission ? 1 : -1;

		if (tempsFondu > 0){
			float tempsRestant = tempsFondu;

			while (tempsRestant > 0) {
				alpha += fonduDirection * Time.deltaTime / tempsFondu;

				alpha = Mathf.Clamp01 (alpha);
						
				if (null != fadeImage) {
					fadeImage.color = new Color (fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
				}

				if (null != fadeText) {
					fadeText.color = new Color (fadeText.color.r, fadeText.color.g, fadeText.color.b, alpha);
				}
					
				tempsRestant -= Time.deltaTime;
				yield return null;
			}
		} else {
			alpha = Mathf.Clamp01 (alpha + fonduDirection);
				
			if (null != fadeImage) {
				fadeImage.color = new Color (fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
			}

			if (null != fadeText) {
				fadeText.color = new Color (fadeText.color.r, fadeText.color.g, fadeText.color.b, alpha);
			}
		}
		yield return null;
	}
}
