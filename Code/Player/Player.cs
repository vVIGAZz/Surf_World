using Sandbox;
using Sandbox.Services;
public sealed class Player : Component
{
	//Game Manager
	[Property] public GameManager G;
	[Property] public double WorldRecord;
	//Timer
	[Property] public double Timer;
	[Property] public bool StartTimer;
	[Property] public HUD hud;
	[Property] public PlayerName Tag;
	//Save Data
	[Property] public string MapName { get; set; }
	[Property] public double PersonTime { get; set; }
	[Property] public bool IsFirstRun { get; set; }
	protected override void OnStart()
	{
		Tag.GameObject.WorldPosition.WithZ( 100 );
		if ( IsProxy )
		{
			if (hud != null )
			{
				hud.Enabled = false;
			}
			return;
		}
		G = Scene.GetAllComponents<GameManager>().FirstOrDefault();
		if ( G != null )
		{
			MapName = G.MapName;
			if ( G.Entries.Count > 0 )
			{
				WorldRecord = G.Entries[0].Value;
			}
		}
		Load();
		hud.UpdateRecords( PersonTime );
		hud.UpdateWorldRecords( WorldRecord );
		G.OnLeaderboardUpdated += OnLeaderboardUpdated;
		Tag.Enabled = false;

	}
	protected override void OnUpdate()
	{
		if ( IsProxy ) return;
		if ( StartTimer )
		{
			Timer += Time.Delta;
			hud.UpdateTime( Timer );
		}
		else
		{
			Timer = 0;
			hud.UpdateTime( Timer );
		}
	}
	public void SetTime( double PR )
	{
		if ( IsProxy ) return;
		if ( PR <= 0 ) return;
		if ( IsFirstRun || PR < PersonTime )
		{
			IsFirstRun = false;
			PersonTime = PR;
			Stats.SetValue( MapName, PersonTime );
			Save();
			hud.UpdateRecords( PersonTime );
		}
	}
	public void Save()
	{
		var saveData = SaveLoadSystem.Load() ?? new SaveFile();
		saveData.PersonalRecords[MapName] = PersonTime;
		SaveLoadSystem.Save( saveData );
	}
	public void Load()
	{
		var loadData = SaveLoadSystem.Load();
		if ( loadData != null && loadData.PersonalRecords.ContainsKey( MapName ) )
		{
			PersonTime = loadData.PersonalRecords[MapName];
			IsFirstRun = PersonTime == 0;
		}
		else
		{
			PersonTime = 0;
			IsFirstRun = true;
		}
	}

	private void OnLeaderboardUpdated()
	{
		if ( G.Entries.Count > 0 )
		{
			WorldRecord = G.Entries[0].Value;
			hud.UpdateWorldRecords( WorldRecord );
		}
	}
	private void StartRun( Player player )
	{
		if ( player != this ) return;
		StartTimer = true;
	}
	private void StopRun( Player player )
	{
		if ( player != this ) return;
		StartTimer = false;
	}
	private async void FinishRun( Player player )
	{
		if ( player != this ) return;
		StartTimer = false;
		SetTime( Timer );
		await G.LeaderboardUpdate();
		//OnLeaderboardUpdated();
	}
	protected override void OnEnabled()
	{
		EventManager.OnStart += StartRun;
		EventManager.OnStop += StopRun;
		EventManager.OnFinish += FinishRun;

	}
	protected override void OnDisabled()
	{
		Networking.Disconnect();
		GameObject.Destroy();
		EventManager.OnStart -= StartRun;
		EventManager.OnStop -= StopRun;
		EventManager.OnFinish -= FinishRun;
		if ( G != null ) G.OnLeaderboardUpdated -= OnLeaderboardUpdated;
	}
}
