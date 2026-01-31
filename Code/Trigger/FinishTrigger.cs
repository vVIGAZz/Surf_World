using Sandbox;

public sealed class FinishTrigger : Component, Component.ITriggerListener
{
	[Property] public GameObject spawn;
	public void OnTriggerEnter(Collider other )
	{
		if ( other.Tags.Has( "player" ) )
		{
			FinishEvent.Post( x => x.IsFinished( true ) );
			EventTimer.Post( x => x.StopTimer() );
			float timer = 3;
			timer -= Time.Delta;
			if ( timer <= 0 )
			{
				other.GameObject.WorldPosition = spawn.WorldPosition;
				timer = 3;
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
