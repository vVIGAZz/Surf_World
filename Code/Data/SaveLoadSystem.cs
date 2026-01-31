using Sandbox;

public sealed class SaveLoadSystem : Component
{
	public static void Save(PlayerSaveLoad player )
	{
		FileSystem.Data.WriteJson( "PlayerRecords.json", player );
	}
	public static PlayerSaveLoad Load()
	{
		return FileSystem.Data.ReadJson<PlayerSaveLoad>( "PlayerRecords.json" );
	}
}
public class PlayerSaveLoad
{
	public double PersonalRecords { get; set; } = 0;
}
