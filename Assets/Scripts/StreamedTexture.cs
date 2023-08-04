
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
					Debug.Log( $"{textureUrl}: {unityWebRequest.error}" );
				}
				else
				{
					var downloadedTexture = DownloadHandlerTexture.GetContent( unityWebRequest );

					texture = new Texture2D( downloadedTexture.width, downloadedTexture.height, downloadedTexture.format, true );

					texture.LoadImage( unityWebRequest.downloadHandler.data );

					texture.Apply();

					requestCompleted = true;
				}
			}
		}
	}
}
