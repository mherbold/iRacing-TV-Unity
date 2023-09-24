
using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class LiveDataTrackMap
{
	public bool show = false;

	public bool showPaceCar = false;
	public Vector3 paceCarOffset = Vector3.zero;

	public int trackID = 0;

	public float width = 0;
	public float height = 0;

	public Vector3 startFinishLine = Vector3.zero;

	public List<Vector3> drawVectorList;

	public LiveDataTrackMapCar[] liveDataTrackMapCars = new LiveDataTrackMapCar[ LiveData.MaxNumDrivers ];
}
