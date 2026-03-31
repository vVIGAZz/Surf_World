using Sandbox;
using System.Collections.Generic;

public sealed class SaveLoadSystem : Component
{
	public static void Save( SaveFile player )
	{
		FileSystem.Data.WriteJson( "player.json", player );
	}
	public static SaveFile Load()
	{
		return FileSystem.Data.ReadJson<SaveFile>( "player.json" );
	}
}
public class SaveFile
{
	public Dictionary<string, double> PersonalRecords { get; set; } = new Dictionary<string, double>();
}
