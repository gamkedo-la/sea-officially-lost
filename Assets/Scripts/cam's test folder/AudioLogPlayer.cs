using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLogPlayer : MonoBehaviour
{

    public string soundName;
	
	public void ClickAction ()
    {
        Debug.Log("play audio log: " + soundName );
	}
}
