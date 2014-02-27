using UnityEngine;
using System.Collections;

public class FlashLightScript : MonoBehaviour {

	private float i = 5.0f;
	private float degrade = .075f;
	
	public void degradeLight() {
		if (i > 0) {
			i = i - (degrade * Time.deltaTime);
			this.light.intensity = i;
		}
	}
	
	public float getProgress () {
		return i/5.0f;
	}
}
