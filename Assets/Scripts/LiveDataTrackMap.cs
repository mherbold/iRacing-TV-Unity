
using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class LiveDataTrackMap
{
	public const int MaxNumCars = 63;

	public int trackID;

	public float width;
	public float height;

	public List<Vector3> drawVectorList;

	public LiveDataTrackMapCar[] liveDataTrackMapCars = new LiveDataTrackMapCar[ MaxNumCars ];
}
