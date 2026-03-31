using Sandbox;

public sealed class Spawn : Component
{
	protected override void OnStart()
	{
		var g = Scene.GetAllComponents<GameManager>().FirstOrDefault();
		if ( g != null ) g.spawnPoint = GameObject;
	}
}
