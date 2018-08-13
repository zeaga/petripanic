using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( SlimeRects ) )]
[CanEditMultipleObjects]
public class SlimeHandles : Editor {
	private void OnSceneGUI( ) {
		if ( target == null )
			return;
		SlimeRects slimeRects = (SlimeRects)target;
		Vector3 pos = slimeRects.transform.position;
		float s = ( Camera.main.orthographicSize / 360 ) * 12;
		foreach ( Rect rect in slimeRects.ForbiddenAreas ) {
			Vector2 origin = new Vector2( pos.x + rect.x * s, pos.y + rect.y * s );
			Vector3[] area = new Vector3[] {
				new Vector3( origin.x, origin.y, pos.z ),
				new Vector3( origin.x, origin.y + rect.height * s, pos.z ),
				new Vector3( origin.x + rect.width * s, origin.y + rect.height * s, pos.z ),
				new Vector3( origin.x + rect.width * s, origin.y, pos.z )
			};
			Handles.DrawSolidRectangleWithOutline( area, slimeRects.FaceColor, slimeRects.OutlineColor );
		}
	}
}
