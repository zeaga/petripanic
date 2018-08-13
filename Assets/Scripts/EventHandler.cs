using System;
using System.Collections;
using UnityEngine;

public class EventHandler : MonoBehaviour {

	[SerializeField]
	private float tickLength;

	private static Action everyTick;

	private bool tickEventGoing;

	private void OnDestroy( ) {
		everyTick = null;
	}

	private IEnumerator TickEvent( ) {
		tickEventGoing = true;
		while( true ) {
			yield return new WaitForSeconds( tickLength );
			everyTick?.Invoke( );
		}
	}

	public void StartTickEvent( ) {
		if ( !tickEventGoing )
			StartCoroutine( "TickEvent" );
	}

	public void StopTickEvent( ) {
		if ( tickEventGoing )
			StopCoroutine( "TickEvent" );
	}

	public static void RegisterTickEvent( Action cb ) => everyTick += cb;

}
