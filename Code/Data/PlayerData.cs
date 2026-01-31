using Sandbox;

public class PlayerData
{
	public float BestSeconds { get; set; }
	public float BestMinutes { get; set; }
	public bool HasFinish { get; set; }
	public static void Save(PlayerData data )
	{
		FileSystem.Data.WriteJson( "player.json", data );
	}
	public static PlayerData Load()
	{
		return FileSystem.Data.ReadJson<PlayerData>( "player.json" );
	}
}
