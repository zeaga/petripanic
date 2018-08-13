using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToStart : MonoBehaviour {

	[SerializeField]
	private Scene scene;

	void Update () {
		if ( Input.GetButton( "Fire1" ) )
			SceneManager.LoadScene( 1 );
	}
}
