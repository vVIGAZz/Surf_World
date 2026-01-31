using Sandbox;

public sealed class Pause : Component
{
	public static bool IsPaused = true;
	[Property] private Movement _playerMovement;
	[Property] private Timer timer;
	[Property] private PauseMenu pause;

	protected override void OnStart()
	{
		IsPaused = false;
	}
	protected override void OnUpdate()
	{
		if ( Input.EscapePressed )
		{
			Input.EscapePressed = false;
			IsPaused = !IsPaused;
		}
		if ( IsPaused )
		{
			_playerMovement.Enabled = false;
			timer.Enabled = false;
			pause.Enabled = true;
		}
		else
		{
			_playerMovement.Enabled = true;
			timer.Enabled = true;
			pause.Enabled = false;
		}
	}
}
