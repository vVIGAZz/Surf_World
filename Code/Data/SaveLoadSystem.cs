using Sandbox;
using System.Collections.Generic;

public sealed class SaveLoadSystem : Component
{
	public static void Save(PlayerSaveLoad player )
	{
		FileSystem.Data.WriteJson( "player.json", player );
	}
	public static PlayerSaveLoad Load()
	{
		return FileSystem.Data.ReadJson<PlayerSaveLoad>( "player.json" );
	}
}

public class PlayerSaveLoad
{
	public Dictionary<string, double> PersonalRecords { get; set; } = new Dictionary<string, double>();
}
