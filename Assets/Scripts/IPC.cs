
using System.IO.MemoryMappedFiles;

using UnityEngine;

public class IPC : MonoBehaviour
{
	public MemoryMappedFile memoryMappedFile;

	public void Awake()
	{
		memoryMappedFile = MemoryMappedFile.CreateOrOpen( "iRacing-TV", 1024 );
	}

	public void Update()
	{
		
	}
}
