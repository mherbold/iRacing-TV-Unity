
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

public class StreamedTexture
{
	public string textureUrl = string.Empty;
	private Texture2D texture = null;

	public bool requestPending = false;
	public bool requestCompleted = false;

	public void ChangeTexture( string url )
	{
		textureUrl = url;
		requestPending = true;
	}

	public Texture2D GetTexture()
	{
		return requestCompleted ? texture : null;
	}

	public IEnumerator Fetch()
	{
		if ( requestPending )
		{
			requestPending = false;
			requestCompleted = false;

			texture = null;

			if ( textureUrl != string.Empty )
			{
				using var unityWebRequest = UnityWebRequestTexture.GetTexture( textureUrl );

				yield return unityWebRequest.SendWebRequest();

				if ( unityWebRequest.result != UnityWebRequest.Result.Success )
				{
					Debug.Log( unityWebRequest.error );
				}
				else
				{
					texture = DownloadHandlerTexture.GetContent( unityWebRequest );

					requestCompleted = true;
				}
			}
		}
	}
}
