using Sandbox;

public sealed class Timer : Component, EventTimer, FinishEvent
{
	private bool _IsStarted = false;
	private bool _IsFinished;
	private double _time;
	[Property] private HUD hud;
	protected override void OnUpdate()
	{
		if ( !_IsStarted )
		{
			_time = 0;
			hud.UpdateTime( _time );
			return;
		}
		else
		{
			_time += Time.Delta;
			hud.UpdateTime( _time );
		}
	}
	public void StartTimer()
	{
		_IsStarted = true;
	}
	public void StopTimer()
	{
		_IsStarted = false;
	}
	public void IsFinished(bool flag)
	{
		if ( flag )
		{
			_IsFinished = flag;
			SetTime.Post( x => x.SetTime( _time ) );
		}
	}
}
