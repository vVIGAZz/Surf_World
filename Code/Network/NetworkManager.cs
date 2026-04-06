using Sandbox;
using System.Threading.Tasks;

public sealed class NetworkManager : Component, Component.INetworkListener
{
	[Property] public GameObject playerPrefab;
	[Property] public GameObject SpawnPoint;
	public void OnActive(Connection connection )
	{
		var player = playerPrefab.Clone( SpawnPoint.WorldPosition );
		player.NetworkSpawn( connection );
		//player.Network.SetOrphanedMode( NetworkOrphaned.Host );
	}
}
