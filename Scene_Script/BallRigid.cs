using System;
using Godot;

public partial class BallRigid : RigidBody3D
{
	//drag
	Node3D _draggedObject;

	//double click
	float _doubleClickTime = 0.5f;
    float _timeSinceLastClick = 0f;
    bool _isFirstClick = true;

    //random albedo
    public static MeshInstance3D _ballMesh;
    RandomNumberGenerator _rng = new RandomNumberGenerator();

	public override void _Ready()
	{
        AddToGroup("BallGroup");
        InputEvent += (camera, inputEvent, position, normal, shapeIdx) => OnBallRigidInputEvent(camera, inputEvent, position, normal, (int)shapeIdx);
        _ballMesh = GetNode<MeshInstance3D>("MeshInstance3D");
        if (_ballMesh.GetActiveMaterial(0) is StandardMaterial3D ballMat)
		{
			ballMat.AlbedoColor = new Color(_rng.RandfRange(0,1), _rng.RandfRange(0,1), _rng.RandfRange(0,1));
		}
	}

    public override void _Process(double delta)
	{
        
            if (!_isFirstClick)
            {
                _timeSinceLastClick += (float)delta;
            }
            //GD.Print(_timeSinceLastClick);

            if (_draggedObject != null && _draggedObject.IsInGroup("BallGroup"))
            {
                // คำนวณตำแหน่งใหม่ของวัตถุในระหว่างการลาก
                var mousePosition = GetViewport().GetMousePosition();
                var rayOrigin = Global.Camera3D.ProjectRayOrigin(mousePosition);
                var rayDirection = Global.Camera3D.ProjectRayNormal(mousePosition);

                // กำหนดตำแหน่งใหม่ของวัตถุในระยะทางจากกล้อง
                _draggedObject.GlobalTransform = new Transform3D(_draggedObject.GlobalTransform.Basis, rayOrigin + rayDirection * 10);
            }
        
	}
	public void OnBallRigidInputEvent(Node camera, InputEvent inputEvent, Vector3 position, Vector3 normal, int shape_idx)
	{

            if (inputEvent is InputEventMouseButton btn && btn.ButtonIndex == MouseButton.Left && inputEvent.IsPressed())
            {
                if (!_isFirstClick && _timeSinceLastClick <= _doubleClickTime)
                {
                    Sleeping = false;
                    ApplyImpulse(new Vector3(0, 0, Global.PowerValue*-1));
                    _isFirstClick = true;
                    Global.JustAMomentLb.Visible = true;
                    Tween _tween = GetTree().CreateTween();
                    _tween.TweenProperty(Global.JustAMomentLb,"scale",new Vector2(3.0f,3.0f),0.5f).SetTrans(Tween.TransitionType.Back);
                    _tween.TweenProperty(Global.JustAMomentLb,"scale",Vector2.One,0.5f).SetTrans(Tween.TransitionType.Back);
                }
                else
                {
                    _isFirstClick = false;
                    _timeSinceLastClick = 0f;
                }
            }

	}
    public override void _Input(InputEvent @event)
    {

        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            //ray from mouse position
            var mousePosition = GetViewport().GetMousePosition();
            var rayOrigin = Global.Camera3D.ProjectRayOrigin(mousePosition);
            var rayDirection = Global.Camera3D.ProjectRayNormal(mousePosition);
            
            // check rigidbody3d draged
            PhysicsRayQueryParameters3D _params = PhysicsRayQueryParameters3D.Create(rayOrigin, rayOrigin + rayDirection * 1000);
            PhysicsDirectSpaceState3D _spaceState = GetWorld3D().DirectSpaceState;
            var result = _spaceState.IntersectRay(_params);
            
            if (result.Count > 0)
            {
                _draggedObject = (Node3D)result["collider"];
            }
        }
        
        // ปล่อยวัตถุเมื่อคลิกเมาส์ถูกปล่อย
        if (@event is InputEventMouseButton releaseEvent && !@event.IsPressed() && releaseEvent.ButtonIndex == MouseButton.Left)
        {
            _draggedObject = null;
        }

     	
    }
}
