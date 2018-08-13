using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SlimeEntity : MonoBehaviour {

	public int X;
	public int Y;
	public bool State;
	public float Size;

	private SlimeController sc;

	private void Start( ) {
		sc = GetComponentInParent<SlimeController>( );
	}

	public void SetState( bool state ) {
		State = state;
		sc.SetState( X, Y, state );
	}

}
