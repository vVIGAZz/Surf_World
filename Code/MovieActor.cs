using Sandbox;

public sealed class MovieActor : Component
{
	public CharacterController controller;
	[Property] public GameObject head;
	[Property] public GameObject body;
	private Vector3 gravity = new Vector3( 0, 0, 800 );
	private float jumpForce = 268f;
	[Sync] public Vector3 WishVelocity { get; set; }
	[Sync] public Angles EyeAngle { get; set; }
	[Sync] public bool IsGrounded { get; set; }
	[Sync] public bool IsJumping { get; set; }
	//Speed
	private float speed = 420f;
	// Animation and cloth
	[Property] public SkinnedModelRenderer animation;
	[Property] public Dresser dress;

	protected override void OnStart()
	{
		controller = GetComponent<CharacterController>();
		controller.Acceleration = 60f;
		controller.GroundAngle = 45;
		controller.StepHeight = 15;
		dress.Source = Dresser.ClothingSource.Manual;
		dress.BodyTarget = animation;
		dress.ApplyHeightScale = true;
		dress.Randomize();
	}
	protected override void OnUpdate()
	{
		var animVelocity = body.WorldRotation.Inverse * WishVelocity;
		animation.Parameters.Set( "move_x", animVelocity.x );
		animation.Parameters.Set( "move_y", -animVelocity.y );
		Animation();
		if ( IsProxy ) return;

		Angles look = EyeAngle;
		look += Input.AnalogLook;
		look.roll = 0;
		look.pitch = look.pitch.Clamp( -89f, 89f );
		EyeAngle = look;
		Rotation lookDir = EyeAngle.ToRotation();
		head.WorldRotation = lookDir;
		WorldRotation = Rotation.FromYaw( EyeAngle.yaw );
		body.WorldRotation = Rotation.FromYaw( EyeAngle.yaw );
	}
	protected override void OnFixedUpdate()
	{
		if ( IsProxy ) return;
		BuildWishVelocity();
		IsGrounded = controller.IsOnGround;
		if ( controller.IsOnGround && Input.Pressed( "jump" ) )
		{
			IsJumping = true;
			controller.Punch( Vector3.Up * jumpForce );
		}
		else
		{
			IsJumping = false;
		}
		if ( controller.IsOnGround )
		{
			controller.Velocity = controller.Velocity.WithZ( 0 );
			controller.Accelerate( WishVelocity );
			controller.ApplyFriction( 4f );
		}
		else
		{
			controller.Velocity -= gravity * Time.Delta;
			controller.Accelerate( WishVelocity.ClampLength( 150 ) );
			//controller.ApplyFriction( 0f );
		}
		controller.Move();
	}
	private void BuildWishVelocity()
	{
		if ( IsProxy ) return;
		var camPos = head.WorldRotation.Angles();
		WishVelocity = 0;
		Rotation lookDir = camPos.ToRotation();
		WishVelocity = Input.AnalogMove * lookDir;
		WishVelocity = WishVelocity.WithZ( 0 );
		if ( !WishVelocity.IsNearZeroLength ) WishVelocity = WishVelocity.Normal;
		WishVelocity *= speed;
	}

	private void Animation()
	{
		if ( IsJumping ) animation.Parameters.Set( "b_jump", true );
		if ( IsGrounded )
		{
			animation.Parameters.Set( "b_grounded", true );
		}
		else
		{
			animation.Parameters.Set( "b_grounded", false );
		}
	}
}
