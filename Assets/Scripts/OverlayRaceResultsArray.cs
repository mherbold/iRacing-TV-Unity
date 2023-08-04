
using UnityEngine;

public class OverlayRaceResultsArray : MonoBehaviour
{
	public GameObject firstRaceResults;

	public void Awake()
	{
		var raceResults = new GameObject[ LiveData.MaxNumClasses ];

		for ( var raceResultsIndex = 1; raceResultsIndex < raceResults.Length; raceResultsIndex++ )
		{
			raceResults[ raceResultsIndex ] = Instantiate( firstRaceResults );

			raceResults[ raceResultsIndex ].transform.SetParent( firstRaceResults.transform.parent, false );

			var overlayRaceResults = raceResults[ raceResultsIndex ].GetComponent<OverlayRaceResults>();

			overlayRaceResults.classIndex = raceResultsIndex;

			raceResults[ raceResultsIndex ].SetActive( true );
		}

		firstRaceResults.SetActive( true );
	}
}
