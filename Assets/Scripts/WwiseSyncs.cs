using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseSyncs : MonoBehaviour {

	public void VolumeMaster(float vol) {
		AkSoundEngine.SetRTPCValue("Volume_Master", vol);
	}

	public void VolumeMusic(float vol) {
		AkSoundEngine.SetRTPCValue("Volume_Music", vol);
	}

	public void VolumeSFX(float vol) {
		AkSoundEngine.SetRTPCValue("Volume_SFX", vol);
	}

}
