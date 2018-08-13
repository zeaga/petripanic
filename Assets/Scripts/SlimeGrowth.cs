using System;

public static class SlimeGrowth {

	private static Random rng = new Random( );

	public static void Grow( ref bool[,] map, double chance ) {
		int w = map.GetLength( 0 );
		int h = map.GetLength( 1 );
		bool[,] newMap = new bool[w, h];
		for ( int y = 0; y < h; y++ ) {
			for ( int x = 0; x < w; x++ ) {
				if ( map[x, y] == true ) {
					newMap[x, y] = true;
					for ( int d = -1; d <= 1; d += 2 ) {
						if ( y + d >= 0 && y + d < h && rng.NextDouble( ) <= chance )
							newMap[x, y + d] = true;
						if ( x + d >= 0 && x + d < w && rng.NextDouble( ) <= chance )
							newMap[x + d, y] = true;
					}
				}
			}
		}
		map = newMap;
	}
}