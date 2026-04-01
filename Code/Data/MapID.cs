using Sandbox;

public sealed class MapID : Component
{
	[Property] public string Name { get; set; }

	protected override void OnStart()
	{
		var g = Scene.GetAllComponents<GameManager>().FirstOrDefault();
		if (g != null )
		{
			g.MapName = Name;
			_ = g.LeaderboardUpdate();
		}
	}
}
