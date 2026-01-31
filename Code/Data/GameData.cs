using Sandbox;
using Sandbox.Services;
public sealed class GameData : Component, SetTime
{
	//UI
	[Property] private HUD _hud;
	[Property] private Leaderboard _leaderboard;
	//Map ID
	[Property] public MapIdent mapConfig;
	[Property] public string MapName { get; set; }
	//Player
	[Property] public GameObject player;
	//Spawn
	[Property] public GameObject spawn;
	[Property]private double _bestTime;
	[Property] private bool _isFirstRun = true;
	protected override async void OnStart()
	{
		MapName = mapConfig.MapID;
		LoadTime();
		Spawn();
		await _leaderboard.SetStatistic( MapName );
		LoadLeaderboard();
		_hud.UpdateRecords( _bestTime );
		_hud.UpdateWorldRecords( _leaderboard.WR );

	}
	public void Spawn()
	{
		if ( player != null ) player.WorldPosition = spawn.WorldPosition;
	}
	public void SetTime( double bestTime)
	{
		if (_isFirstRun || bestTime < _bestTime )
		{
			_isFirstRun = false;
			_bestTime = bestTime;
			_hud.UpdateRecords( _bestTime );
			SaveTime();
			SetValueToStat();
			LoadLeaderboard();
			_hud.UpdateWorldRecords( _leaderboard.WR );
		}
	}
	public void SetValueToStat()
	{
		Stats.SetValue( MapName, _bestTime );
	}
	public void LoadLeaderboard()
	{
		_leaderboard.SetStatistic( MapName );
	}

	private void SaveTime()
	{
		var saveData = new PlayerSaveLoad
		{
			PersonalRecords = _bestTime
		};
		SaveLoadSystem.Save( saveData );
	}
	private void LoadTime()
	{
		var loadData = SaveLoadSystem.Load();
		if ( loadData != null )
		{
			_bestTime = loadData.PersonalRecords;
			_isFirstRun = _bestTime == 0;
		}
		else
		{
			_bestTime = 0;
			_isFirstRun = true;
		}
	}
}
