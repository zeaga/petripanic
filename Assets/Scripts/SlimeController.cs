using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SlimeController : MonoBehaviour {

	public int SlimeLeft { get; private set; }

	public int SlimeCapacity { get; private set; }

	public Text DebugText;

	[HideInInspector]
	public SlimeEntity[,] Ents;

	[SerializeField]
	private Vector2Int slimeStart;

	[SerializeField]
	private int slimeIterations;

	[SerializeField]
	private Color slimeColor;

	[SerializeField]
	private Sprite slimeSprite;

	[SerializeField]
	private double growChance;

	private int size = 12;
	private int w;
	private int h;
	private float pixelSize;

	private SlimeRects slimeRects;

	private void Awake( ) {
		Camera cam = Camera.main;
		w = 1080 / size; // I know "magic numbers" are bad, but let's roll with it for now
		h = 720 / size;
		pixelSize = cam.orthographicSize / 360;
		Ents = new SlimeEntity[w, h];
	}

	private void Start( ) {
		slimeRects = GetComponent<SlimeRects>( );
		for ( int y = 0; y < h; y++ ) {
			for ( int x = 0; x < w; x++ ) {
				float realSize = pixelSize * size;
				GameObject go = new GameObject( $"slime[{x},{y}]" );
				go.transform.SetParent( transform );
				go.transform.localPosition = new Vector2( realSize * x, realSize * y );
				go.transform.localScale = new Vector2( size, size );
				SpriteRenderer sr = go.AddComponent<SpriteRenderer>( );
				sr.sprite = slimeSprite;
				sr.color = slimeColor;
				sr.sortingLayerName = "Slime";
				sr.enabled = Ents[x, y];
				SlimeEntity se = go.AddComponent<SlimeEntity>( );
				se.X = x;
				se.Y = y;
				se.State = Ents[x, y];
				se.Size = realSize;
				Ents[x, y] = se;
			}
		}
		SetState( slimeStart.x, slimeStart.y, true );
		for ( int i = 0; i < slimeIterations; i++ )
			Grow( );
		EventHandler.RegisterTickEvent( Grow );
	}

	private void Update( ) {
		Vector3 mousePos = Input.mousePosition;
		DebugText.rectTransform.SetPositionAndRotation( mousePos + new Vector3( 16, -16, 0 ), Quaternion.identity );
		Vector3 worldPos = Camera.main.ScreenToWorldPoint( mousePos ) - transform.position;
		float mult = ( 1 / ( pixelSize * size ) );
		DebugText.text = $"{(int)( worldPos.x * mult )},{(int)( worldPos.y * mult )}";
	}

	public bool IsForbidden( int x, int y ) {
		foreach ( Rect area in slimeRects.ForbiddenAreas ) {
			if ( area.Contains( new Vector2( x, y ) ) )
				return true;
		}
		return false;
	}

	public bool SetState( int x, int y, bool state ) {
		bool forbidden = false;
		if ( IsForbidden( x, y ) ) {
			forbidden = true;
			state = false;
		}
		SlimeEntity ent = Ents[x, y];
		ent.State = state;
		ent.GetComponent<SpriteRenderer>( ).enabled = state;
		return forbidden;
	}

	private void Grow( ) {
		bool[,] map = new bool[w, h];
		for ( int y = 0; y < h; y++ ) {
			for ( int x = 0; x < w; x++ ) {
				map[x, y] = Ents[x, y].State;
			}
		}
		SlimeGrowth.Grow( ref map, growChance );
		int coveredCells = 0;
		int totalCells = 0;
		for ( int y = 0; y < h; y++ ) {
			for ( int x = 0; x < w; x++ ) {
				if ( !SetState( x, y, map[x, y] ) )
					totalCells++;
				if ( map[x, y] )
					coveredCells++;
			}
		}
		SlimeLeft = coveredCells;
		SlimeCapacity = totalCells;
	}

}
