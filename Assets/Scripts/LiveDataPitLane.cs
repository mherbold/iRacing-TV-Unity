
using System;

[Serializable]
public class LiveDataPitLane
{
	public bool show = false;

	public LiveDataPitLaneCar[] liveDataPitLaneCars = new LiveDataPitLaneCar[ LiveData.MaxNumDrivers ];
}
