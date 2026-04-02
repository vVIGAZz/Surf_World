using Sandbox;
using Sandbox.MovieMaker;
public sealed class MovieRecord : Component
{
	[Property] public bool IsRecording;
	[Property] public MovieRecorder MovieRecorder;

	protected override void OnStart()
	{
		MovieRecorder = new MovieRecorder(Scene);
	}
	protected override void OnUpdate()
	{
		if ( Input.Pressed( "use" ) )
		{
			IsRecording = !IsRecording;
		}
		if ( IsRecording )
		{

			Rec();
		}
		else
		{
			UnRec();
		}
	}

	public void Rec()
	{
		Log.Info( "Start Rec" );
			MovieRecorder.Start();
	}
	public void UnRec()
	{
		Log.Info( "Unrec" );
		var clip = MovieRecorder.ToClip();
		Log.Info( $"{clip} is found" );
		FileSystem.Data.WriteJson( $"movies/gameplay.movie", clip.ToResource() );
		Log.Info( $"Save Clip" );

	}
}
