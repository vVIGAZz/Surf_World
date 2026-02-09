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
	[Property] private double _bestTime;
	[Property] private bool _isFirstRun = true;
	protected override async void OnStart()
	{
		MapName = mapConfig.MapID;
		LoadTime();
		Spawn();
		await _leaderboard.SetStatistic( MapName );
		_hud.UpdateRecords( _bestTime );
		_hud.UpdateWorldRecords( _leaderboard.WR );

	}
	protected override void OnUpdate()
	{
		if ( Input.Pressed( "reload" ) )
		{
			Spawn();
		}
	}
	public async void Spawn()
	{
		await _leaderboard.SetStatistic( MapName );
		if ( player != null )
		{
			player.WorldPosition = spawn.WorldPosition;
		}
	}
	public async void SetTime( double bestTime)
	{
		if ( bestTime <= 0 )
		{
			return;
		}
		if (_isFirstRun || bestTime < _bestTime )
		{
			_isFirstRun = false;
			_bestTime = bestTime;
			_hud.UpdateRecords( _bestTime );
			SaveTime();
			Stats.SetValue( MapName, _bestTime );
			await _leaderboard.SetStatistic( MapName );
			_hud.UpdateWorldRecords( _leaderboard.WR );
		}
	}
	private void SaveTime()
	{
		var saveData = SaveLoadSystem.Load() ?? new PlayerSaveLoad();
		saveData.PersonalRecords[MapName] = _bestTime;
		SaveLoadSystem.Save( saveData );
	}
	private void LoadTime()
	{
		var loadData = SaveLoadSystem.Load();
		if ( loadData != null && loadData.PersonalRecords.ContainsKey( MapName ) )
		{
			_bestTime = loadData.PersonalRecords[MapName];
			_isFirstRun = _bestTime == 0;
		}
		else
		{
			_bestTime = 0;
			_isFirstRun = true;
		}
	}
}
