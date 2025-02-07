using Godot;

public partial class Global : Node
{
    public static float SpeedX { get; set; } = 5;
    public static float PowerValue { get; set; } = 30;
    public static float VectorX { get; set; } = 0;
    public static int ScoreCurrent { get; set; } = 0;
    public static int RollsCurrent { get; set; } = 1;
    public static Camera3D Camera3D;
    public static int SceneLevel {get; set;} = 0;
    public static bool SceneEnter {get; set;} = false;

    Button _homeButton;
    Button _languageButton;

    public static Label JustAMomentLb;
    public static Button GenSweepBt;
    public static RigidBody3D SweepPinRigid;

    string[] _scenePaths = new string[] {
    "res://Scene_Script/PinRigid.tscn",
    "res://Scene_Script/PinRigidA.tscn",
    "res://Scene_Script/PinRigidB.tscn",
    };
    PackedScene _pinScene;
    public static RigidBody3D PinRigid;
    RandomNumberGenerator _rng = new RandomNumberGenerator();
    MeshInstance3D _pinAlbedo;

    Label _rollLabel;
    public static Label RollValue;
    Label _scoreLabel;
    public static Label ScoreValue;

    VBoxContainer _menuBox;
    VBoxContainer _scoreBox;
    VBoxContainer _powerBox;
    HSlider _powerBar;
	Label _powerLabel;

    public override void _Ready()
    {
        _homeButton = GetNode<Button>("VBoxMenu/MainBt");
        _homeButton.Pressed += GoHome;
        _languageButton = GetNode<Button>("LanguageBt");
        _languageButton.Toggled += ChangeLanguage;
        GeneratePinFunc();
        GenSweepBt = GetNode<Button>("VBoxMenu/GenSweepBt");
        GenSweepBt.Pressed += GenSweepFunc;

        _rollLabel = GetNode<Label>("ScoreBox/RowRoll/RollLabel");
        RollValue = GetNode<Label>("ScoreBox/RowRoll/RollValue");
		_scoreLabel = GetNode<Label>("ScoreBox/RowScore/ScoreLabel");
        ScoreValue = GetNode<Label>("ScoreBox/RowScore/ScoreValue");
        JustAMomentLb = GetNode<Label>("JustAMoment");

        _menuBox = GetNode<VBoxContainer>("VBoxMenu");
        _scoreBox = GetNode<VBoxContainer>("ScoreBox");
        _powerBox = GetNode<VBoxContainer>("PowerBox");

        _powerBar = GetNode<HSlider>("PowerBox/PowerBar");
		_powerLabel = GetNode<Label>("PowerBox/PowerLabel");

        TweenPower();
    }
    public override void _Process(double delta)
    {
        _menuBox.Visible = (GetTree()?.CurrentScene != null && GetTree().CurrentScene.Name == "MainMenu"? false : true);
        _scoreBox.Visible = (GetTree()?.CurrentScene != null && GetTree().CurrentScene.Name == "MainMenu"? false : true);
        _powerBox.Visible = (GetTree()?.CurrentScene != null && GetTree().CurrentScene.Name == "MainMenu"? false : true);
        
        PowerValue = (float)_powerBar.Value;
		_powerLabel.Text = Tr("POWER")+" : "+PowerValue;
        _rollLabel.Text = Tr("ROLL")+" ";
        RollValue.Text = RollsCurrent.ToString(); 
        _scoreLabel.Text = Tr("SCORE")+" ";
        ScoreValue.Text = ScoreCurrent.ToString();
        JustAMomentLb.Text = Tr("JAMOMENT");
        _homeButton.Text = Tr("MAIN");
        _languageButton.Text = Tr("EN/TH");
        GenSweepBt.Text = Tr("SWEEPGEN");

        if(SceneEnter)TweenPower();

    }
    public void GoHome()
	{
		GetTree().ChangeSceneToFile("res://Scene_Script/MainMenu.tscn");
	}
    public void ChangeLanguage(bool _toggleOn)
    {
        TranslationServer.SetLocale((_toggleOn?"th":"en"));
    }
    public void GeneratePinFunc()
	{
         _pinScene = GD.Load<PackedScene>(_scenePaths[_rng.RandiRange(0,_scenePaths.Length-1)]);
		for (int i = -2; i <= 2; i++)
		{
			for (int j = -15; j <= -13; j++)
			{
				RigidBody3D PinRigid = _pinScene.Instantiate<RigidBody3D>();
				PinRigid.Position = new Vector3(i, 2, j);
                _pinAlbedo = PinRigid.GetNode<MeshInstance3D>("MeshInstance3D");
                if (_pinAlbedo.GetActiveMaterial(0) is StandardMaterial3D pinMat)
                {
                    pinMat.AlbedoColor = new Color(_rng.RandfRange(0.2f,1), _rng.RandfRange(0.2f,1), _rng.RandfRange(0.2f,1));
                }
				PinRigid.AddToGroup("PinGroup");
				AddChild(PinRigid);
			}
		}
	}
    public void SweepPinFunc()
    {
        SweepPinRigid = GD.Load<PackedScene>("res://Scene_Script/SweepPin.tscn").Instantiate<RigidBody3D>();
        SweepPinRigid.Position = new Vector3(0,1,-8);
        SweepPinRigid.AddToGroup("SweepPin");
        AddChild(SweepPinRigid);
        SweepPinRigid.ApplyImpulse(new Vector3(0, 0, -50));
    }
    public void GenSweepFunc()
    {
        SweepPinFunc();
        GetTree().CreateTimer(1).Timeout += () => GeneratePinFunc();
        
    }
    public static void IfBallGrop()
    {
        RollsCurrent += 1;
        JustAMomentLb.Visible = false;
    }
    public void TweenPower()
    {
        Tween _tween = GetTree().CreateTween();
		_tween.TweenProperty(_powerBar,"value",66,0.5f).SetTrans(Tween.TransitionType.Quad);
		_tween.TweenProperty(_powerBar,"value",33,0.5f).SetTrans(Tween.TransitionType.Quad);
        SceneEnter = false;
    }
}
