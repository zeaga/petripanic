using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoopAndDump : MonoBehaviour {

	[SerializeField]
	private Transform toilet;

	[SerializeField]
	private Text bucketText;

	[SerializeField]
	private int bucketCapacity;

	public float BucketCurrent { get; private set; }

	[SerializeField]
	private SlimeController slime;

	[SerializeField]
	private float scoopRadius;

	[SerializeField]
	private float dumpRadius;

	[SerializeField]
	private float growRate;

	private SpriteRenderer bucketSR;
	private Sprite[] bucketSprites;

	private void Start( ) {
		bucketSR = transform.Find( "bucket" ).GetComponent<SpriteRenderer>( );
		bucketSprites = Resources.LoadAll<Sprite>( "Sprites/bucket" );
		EventHandler.RegisterTickEvent( Grow );
	}

	private void Update( ) {
		if ( !PhaseHandler.GameOver ) {
			if ( BucketCurrent < bucketCapacity )
				Scoop( );
			Dump( );
			SetBucketSprite( );
		bucketText.text = $"{Mathf.Floor( ( BucketCurrent / bucketCapacity ) * 100 ) }%";
		}
	}

	private void Scoop( ) {
		SlimeEntity[,] map = slime.Ents;
		for ( int y = 0; y < map.GetLength( 1 ); y++ ) {
			for ( int x = 0; x < map.GetLength( 0 ); x++ ) {
				if ( BucketCurrent + 1 <= bucketCapacity && map[x, y].State == true && Vector2.Distance( transform.position, map[x, y].transform.position ) <= scoopRadius ) {
					BucketCurrent++;
					slime.SetState( x, y, false );
				}
			}
		}
	}

	private void Dump( ) {
		if ( BucketCurrent > 0 && Vector2.Distance( transform.position, toilet.position ) <= dumpRadius ) {
			BucketCurrent = 0;
			toilet.GetComponent<AudioSource>( ).Play( );
			StartCoroutine( "DumpBucket" );
		}
	}

	private IEnumerator DumpBucket( ) {
		bucketSR.transform.localEulerAngles = new Vector3( 0, 0, 90 );
		yield return new WaitForSeconds( 0.5f );
		bucketSR.transform.localEulerAngles = Vector3.zero;
	}

	private void SetBucketSprite( ) {
		float percent = BucketCurrent / bucketCapacity;
		if ( percent <= 0.333333f )
			bucketSR.sprite = bucketSprites[0];
		else if ( percent < 0.666667f )
			bucketSR.sprite = bucketSprites[1];
		else if ( percent < 1f )
			bucketSR.sprite = bucketSprites[2];
		else
			bucketSR.sprite = bucketSprites[3];
	}

	private void Grow( ) {
		BucketCurrent *= growRate;
		if ( BucketCurrent > bucketCapacity ) {
			float overflow = BucketCurrent - bucketCapacity;
			BucketCurrent = bucketCapacity;
			List<SlimeEntity> spillSpots = new List<SlimeEntity>( );
			SlimeEntity[,] map = slime.Ents;
			for ( int y = 0; y < map.GetLength( 1 ); y++ ) {
				for ( int x = 0; x < map.GetLength( 0 ); x++ ) {
					if ( map[x, y].State == false && !slime.IsForbidden( x, y ) && Vector2.Distance( transform.position, map[x, y].transform.position ) <= scoopRadius ) {
						spillSpots.Add( map[x, y] );
					}
				}
			}
			while ( overflow >= 1 && spillSpots.Count > 0 ) {
				SlimeEntity spillSpot = spillSpots[Random.Range( 0, spillSpots.Count )];
				spillSpot.SetState( true );
				overflow -= 1;
				spillSpots.Remove( spillSpot );
			}
		}
	}

}
