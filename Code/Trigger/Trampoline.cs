using Sandbox;

public sealed class Trampoline : Component, Component.ITriggerListener
{
	[Property] public float PunchPower;
	public void OnTriggerEnter(Collider other )
	{
		if ( other.Tags.Has( "player" ) )
		{
			var p = other.GetComponent<Movement>();
			if ( p != null )
			{
				p.controller.Punch( Vector3.Up * PunchPower);
			}
		}
	}
}
