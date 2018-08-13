using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	private float speed;

	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private Transform bucket;
	private Vector2 bucketPos;

	private Animator animator;

	private void Start( ) {
		sr = GetComponent<SpriteRenderer>( );
		rb = GetComponent<Rigidbody2D>( );
		animator = GetComponent<Animator>( );
		bucket = transform.Find( "bucket" );
		bucketPos = bucket.localPosition;
	}

	private void Update( ) {
		if ( !PhaseHandler.GameOver )
			Move( );
	}

	private void Move( ) {
		float dx = Input.GetAxis( "Horizontal" );
		float dy = Input.GetAxis( "Vertical" );
		rb.MovePosition( rb.position + new Vector2( dx, dy ) * speed );
		animator.SetBool( "Moving", dx != 0 || dy != 0 );
		//transform.position += Time.deltaTime * speed * transform.right * dx;
		//transform.position += Time.deltaTime * speed * transform.up * Input.GetAxis( "Vertical" );
		if ( dx > 0 ) {
			sr.flipX = true;
			bucket.localPosition = new Vector2( -bucketPos.x, bucketPos.y );
		} else if ( dx < 0 ) {
			sr.flipX = false;
			bucket.localPosition = bucketPos;
		}
	}
}
