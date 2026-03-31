using Sandbox;

public sealed class LeaderboardPlace : Component
{
	protected override void OnStart()
	{
		var g = Scene.GetAllComponents<GameManager>().FirstOrDefault();
		if ( g != null ) g.WorldPosition = WorldPosition;
	}
}
