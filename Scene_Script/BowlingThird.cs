using Godot;

public partial class BowlingThird : Node3D
{
	RigidBody3D _ballRigid;
	DirectionalLight3D _light;
	WorldEnvironment _world;

	Camera3D _camera3D;
	Area3D _checkY;
	MeshInstance3D _backgroundMesh;
	MeshInstance3D _batBar;
	AnimationPlayer _batAnim;
	RandomNumberGenerator _rng = new RandomNumberGenerator();
	OmniLight3D _lightningLight;

	public override void _Ready()
	{
		_ballRigid = GetNode<RigidBody3D>("BallRigidA");
		_camera3D = GetNode<Camera3D>("Camera3D");
		Global.Camera3D = _camera3D;
		_checkY = GetNode<Area3D>("CheckY");
		_checkY.BodyEntered += OnCheckYBodyEntered;
		_light = GetNode<DirectionalLight3D>("DirectionalLight3D");
		_world = GetNode<WorldEnvironment>("WorldEnvironment");

		//_backgroundMesh = GetNode<MeshInstance3D>("BackgroudMesh");
		_batBar = GetNode<MeshInstance3D>("BatBar");
		_batAnim = GetNode<AnimationPlayer>("AnimationPlayer");
		_batAnim.Play("BatIdle");

		Tween _tween = GetTree().CreateTween();
		_tween.TweenProperty(_light,"rotation",new Vector3(0,0,0),3.0f).SetTrans(Tween.TransitionType.Linear);
		
		//Global.SceneLevel = 3;
	}

    public override void _Process(double delta)
	{
		_batBar.Translate(new Vector3(0,0,(float)(delta*Global.SpeedX)));
		if(_batBar.Transform.Origin.X<-35)
		{
			_batBar.Position = new Vector3(_rng.RandfRange(35,50),_rng.RandfRange(3,10),_rng.RandfRange(-15,-22));
		}
		
		if(_batBar.Transform.Origin.X<0 && _batBar.Transform.Origin.X>-5)
		{
			_batAnim.Play("Glance");
		}
		else
		{
			_batAnim.Play("BatIdle");
		}
	}

	public void NextRoll()
	{
		if (BallRigid._ballMesh.GetActiveMaterial(0) is StandardMaterial3D ballMat)
		{
			ballMat.AlbedoColor = new Color(_rng.RandfRange(0.2f,1), _rng.RandfRange(0.2f,1), _rng.RandfRange(0.2f,1));
		}
	}

	public void OnCheckYBodyEntered(Node3D body)
	{
		if(body.IsInGroup("BallGroup"))
		{
			NextRoll();   
			_ballRigid.Position = new Vector3(0,1,0);
			_ballRigid.Sleeping = true;
			// Global.RollsCurrent+=1;
			// Global.GenPinBt.Disabled = false;
			// Global.SweepPinBt.Disabled = false;
			// Global.JustAMomentLb.Visible = false;
			Global.IfBallGrop();
			if(Global.RollsCurrent%3==0)
				TriggerLightning();
		}
		if(body.IsInGroup("PinGroup"))
		{
			Global.ScoreCurrent+=1;
		}
		if(body.IsInGroup("SweepPin"))
		{
			Global.SweepPinRigid.QueueFree();
		}
	}

	public void TriggerLightning()
	{
		_world.Environment.BackgroundEnergyMultiplier = 8;
		GetTree().CreateTimer(0.2f).Timeout += () => _world.Environment.BackgroundEnergyMultiplier = 1;
	}
}
