using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhaseHandler : MonoBehaviour {

	[HideInInspector]
	public static bool GameOver;

	[SerializeField]
	private float PercentNeededForLose;

	[SerializeField]
	private SlimeController slime;

	[SerializeField]
	private ScoopAndDump scoopAndDump;

	[SerializeField]
	private Text slimeText;

	[SerializeField]
	private Transform gameOverGroup;
	
	private Text longestGameText;
	private Text gameOverText;
	private Text fastestWinText;

	private EventHandler eh;

	private void Start( ) {
		eh = GetComponent<EventHandler>( );
		gameOverGroup.gameObject.SetActive( false );
		fastestWinText = gameOverGroup.Find( "FastestWin" ).GetComponent<Text>( );
		gameOverText = gameOverGroup.Find( "GameOverLabel" ).GetComponent<Text>( );
		longestGameText = gameOverGroup.Find( "LongestGame" ).GetComponent<Text>( );
	}

	private void Update( ) {
		float percent = slime.SlimeLeft / (float)slime.SlimeCapacity;
		slimeText.text = $"{Mathf.Min( 100, Mathf.Floor( ( percent / PercentNeededForLose ) * 100 ) ) }%";;
		if ( percent < PercentNeededForLose && !( slime.SlimeLeft == 0 && scoopAndDump.BucketCurrent == 0 ) ) {
			eh.StartTickEvent( );
		} else if ( !GameOver ) {
			OnGameOver( );
		}
		if ( ( GameOver && Input.GetButtonDown( "Fire1" ) ) || Input.GetKeyDown( KeyCode.R ) )
			SceneManager.LoadScene( SceneManager.GetActiveScene( ).name );
	}

	private void OnDestroy( ) {
		GameOver = false;
	}

	private void OnGameOver( ) {
		eh.StopTickEvent( );
		GameOver = true;
		gameOverGroup.gameObject.SetActive( true );
		int time = (int)Timer.TimerTime;
		if ( slime.SlimeLeft == 0 && scoopAndDump.BucketCurrent == 0 ) {
			gameOverText.text = "Victory";
			if ( !PlayerPrefs.HasKey( "fastestWin" ) || time < PlayerPrefs.GetInt( "fastestWin" ) )
				PlayerPrefs.SetInt( "fastestWin", time );
		} else if ( time > PlayerPrefs.GetInt( "longestGame" ) ) {
			PlayerPrefs.SetInt( "longestGame", time );
		}
		fastestWinText.text = $"Fastest Win  - {TimeSpan.FromSeconds( PlayerPrefs.HasKey( "fastestWin" ) ? PlayerPrefs.GetInt( "fastestWin" ) : 3599 ).ToString( @"m\:ss" )}";
		longestGameText.text = $"Longest Game  - {TimeSpan.FromSeconds( PlayerPrefs.HasKey( "longestGame" ) ? PlayerPrefs.GetInt( "longestGame" ) : 0 ).ToString( @"m\:ss" )}";
	}

}
