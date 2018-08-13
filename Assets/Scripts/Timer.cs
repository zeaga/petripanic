using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	
	public static float TimerTime { get; private set; }

	private Text text;

	private void Start( ) {
		text = GetComponent<Text>( );
	}

	private void Update( ) {
		if ( !PhaseHandler.GameOver )
			TimerTime += Time.deltaTime;
		text.text = TimeSpan.FromSeconds( TimerTime ).ToString( @"m\:ss" );
	}

	private void OnDestroy( ) {
		TimerTime = 0;
	}

}
