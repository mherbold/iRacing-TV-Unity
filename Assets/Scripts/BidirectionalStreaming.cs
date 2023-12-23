
using UnityEngine;
using UnityEngine.UI;

using Unity.RenderStreaming;
using Unity.RenderStreaming.Signaling;

using System;
using System.Threading;

public class BidirectionalStreaming : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] private SignalingManager signalingManager;
	[SerializeField] private RawImage localVideoImage;
	[SerializeField] private RawImage remoteVideoImage;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private VideoStreamSender videoStreamSender;
	[SerializeField] private VideoStreamReceiver videoStreamReceiver;
	[SerializeField] private AudioStreamSender audioStreamSender;
	[SerializeField] private AudioStreamReceiver audioStreamReceiver;
	[SerializeField] private SingleConnection singleConnection;
#pragma warning restore 0649

	[NonSerialized] public string roomCode = string.Empty;
	[NonSerialized] public bool started = false;
	[NonSerialized] public long indexLiveData = 0;

	void Awake()
	{
		videoStreamSender.OnStartedStream += id => videoStreamReceiver.enabled = true;
		videoStreamSender.OnStartedStream += _ => localVideoImage.texture = videoStreamSender.sourceWebCamTexture;

		videoStreamReceiver.OnUpdateReceiveTexture += texture => remoteVideoImage.texture = texture;

		audioStreamReceiver.targetAudioSource = audioSource;

		audioStreamReceiver.OnUpdateReceiveAudioSource += source =>
		{
			source.loop = true;

			source.Play();
		};
	}

	void Update()
	{
		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			var liveDataWebcamStreaming = LiveData.Instance.liveDataWebcamStreaming;

			var enable = ( liveDataWebcamStreaming.enabled && ( liveDataWebcamStreaming.webserverURL != string.Empty ) && ( liveDataWebcamStreaming.roomCode != string.Empty ) );

			if ( enable )
			{
				if ( !started )
				{
					started = true;

					var webSocketSignalingSettings = new WebSocketSignalingSettings( liveDataWebcamStreaming.webserverURL, new[] { new IceServer( urls: new[] { "stun:stun.l.google.com:19302" } ) } );

					var webSocketSignaling = new WebSocketSignaling( webSocketSignalingSettings, SynchronizationContext.Current );

					signalingManager.Run( webSocketSignaling );

					webSocketSignaling.OnStart += _ =>
					{
						videoStreamSender.sourceDeviceIndex = 0;
						videoStreamSender.width = 640;
						videoStreamSender.height = 480;
						videoStreamSender.enabled = true;

						audioStreamSender.sourceDeviceIndex = 0;
						audioStreamSender.enabled = true;

						roomCode = liveDataWebcamStreaming.roomCode;

						singleConnection.CreateConnection( roomCode );
					};
				}
			}
			else
			{
				if ( started )
				{
					started = false;

					singleConnection.DeleteConnection( roomCode );

					signalingManager.Stop();

					remoteVideoImage.texture = null;
					localVideoImage.texture = null;
				}
			}
		}
	}
}
