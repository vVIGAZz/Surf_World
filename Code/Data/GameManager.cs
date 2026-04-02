using Sandbox;
using Sandbox.Services;
using System;
using System.Threading.Tasks;
public sealed class GameManager : Component
{
	public event Action OnLeaderboardUpdated;
	//Spawn
	[Property] public GameObject spawnPoint;
	//Map name
	[Property] public string MapName { get; set; }
	//UI
	[Property] public List<Leaderboards.Board2.Entry> Entries = new();
	protected override void OnAwake()
	{
		GameObject.Flags = GameObjectFlags.DontDestroyOnLoad;
	}
	protected override async void OnStart()
	{
		//_ = LeaderboardUpdate();
	}
	public void Respawn(Movement player)
	{
		player.GameObject.WorldPosition = spawnPoint.WorldPosition;
	}
	public async Task LeaderboardUpdate()
	{
		if ( string.IsNullOrEmpty( MapName ) )
		{
			Log.Warning( "MapName is not set for GameManager." );
			return;
		}
		try
		{
			var board = Leaderboards.GetFromStat( MapName );
			board.SetAggregationMin();
			board.SetSortAscending();
			board.MaxEntries = 10;
			await board.Refresh();
			Entries = board.Entries.Take( 10 ).ToList();
			OnLeaderboardUpdated?.Invoke();
		}
		catch ( Exception e )
		{
			Log.Warning( $"LeaderboardUpdate failed: {e.Message}" );
		}
	}
	protected override void OnEnabled()
	{
		EventManager.OnRespawn += Respawn;
	}
	protected override void OnDisabled()
	{
		EventManager.OnRespawn -= Respawn;
	}
}
