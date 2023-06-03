
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;

public static class RemoteTexture
{
	public static async Task<Texture2D> Get( string url )
	{
		var unityWebRequest = UnityWebRequestTexture.GetTexture( url );

		var unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();

		while ( !unityWebRequestAsyncOperation.isDone )
		{
			await Task.Delay( 250 );
		}

		if ( unityWebRequest.result != UnityWebRequest.Result.Success )
		{
			return null;
		}

		return DownloadHandlerTexture.GetContent( unityWebRequest );
	}
}
