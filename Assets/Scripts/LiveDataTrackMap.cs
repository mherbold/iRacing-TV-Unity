﻿
using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class LiveDataTrackMap
{
	public const int MaxNumCars = 63;

	public bool show;

	public int trackID;

	public float width;
	public float height;

	public Vector3 startFinishLine;

	public List<Vector3> drawVectorList;

	public LiveDataTrackMapCar[] liveDataTrackMapCars = new LiveDataTrackMapCar[ MaxNumCars ];
}