using Sandbox;

public sealed class FinishTrigger : Component, Component.ITriggerListener
{
	[Property] public GameObject spawn;
	private float _respawnCouldown = 0f;

	protected override void OnUpdate()
	{
		if (_respawnCouldown > 0 )
		{
			_respawnCouldown -= Time.Delta;
		}
	}
	public void OnTriggerEnter(Collider other )
	{
		if ( other.Tags.Has( "player" ) )
		{
			FinishEvent.Post( x => x.IsFinished( true ) );
			EventTimer.Post( x => x.StopTimer() );
			_respawnCouldown = 5f;
			if ( _respawnCouldown <= 0 )
			{
				other.WorldPosition = spawn.WorldPosition;
				_respawnCouldown = 0f;
			}
		}
	}
	public void OnTriggerExit( Collider other )
	{
		if ( other.Tags.Has( "player" ) )
		{
			FinishEvent.Post( x => x.IsFinished( false ) );
		}
	}
}
