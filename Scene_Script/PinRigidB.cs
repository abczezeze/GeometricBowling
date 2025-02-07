using Godot;
using System;

public partial class PinRigidB : RigidBody3D
{
	AudioStreamPlayer3D _sfxPin;
	public override void _Ready()
	{
		_sfxPin = GetNode<AudioStreamPlayer3D>("SFXPin");
		BodyEntered += (body) => OnBodyEntered(body);
	}

	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("BallGroup"))
		{
			_sfxPin.Play();
		}
	}
}
