
using System;

[Serializable]
public class LiveDataRaceStatus
{
	public bool showBlackLight = false;
	public bool showGreenLight = false;
	public bool showWhiteLight = false;
	public bool showYellowLight = false;

	public string textLayer1 = string.Empty;
	public string textLayer2 = string.Empty;
	public string textLayer3 = string.Empty;
	public string textLayer4 = string.Empty;

	public bool showGreenFlag = false;
	public bool showYellowFlag = false;
	public bool showCheckeredFlag = false;
}
