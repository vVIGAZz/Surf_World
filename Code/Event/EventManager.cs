using Sandbox;
using System;

public sealed class EventManager : Component
{
	public static event Action<Movement> OnRespawn;
	public static event Action<Player> OnStart;
	public static event Action<Player> OnStop;
	public static event Action<Player> OnFinish;

	public static void Respawn(Movement player)
	{
		OnRespawn?.Invoke(player);
	}
	public static void StartRun( Player player )
	{
		OnStart?.Invoke( player );
	}
	public static void StopRun( Player player )
	{
		OnStop?.Invoke( player );
	}
	public static void FinishRun( Player player )
	{
		OnFinish?.Invoke( player );
	}
}
