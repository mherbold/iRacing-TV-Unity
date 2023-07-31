
using UnityEngine;

public class OverlayLeaderboardArray : MonoBehaviour
{
	public GameObject firstLeaderboard;

	public void Awake()
	{
		var leaderboards = new GameObject[ LiveData.MaxNumClasses ];

		for ( var leaderboardIndex = 1; leaderboardIndex < leaderboards.Length; leaderboardIndex++ )
		{
			leaderboards[ leaderboardIndex ] = Instantiate( firstLeaderboard );

			leaderboards[ leaderboardIndex ].transform.SetParent( firstLeaderboard.transform.parent, false );

			var overlayLeaderboard = leaderboards[ leaderboardIndex ].GetComponent<OverlayLeaderboard>();

			overlayLeaderboard.classIndex = leaderboardIndex;

			leaderboards[ leaderboardIndex ].SetActive( true );
		}

		firstLeaderboard.SetActive( true );
	}
}
