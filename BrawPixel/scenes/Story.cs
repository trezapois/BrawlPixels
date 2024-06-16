using Godot;
using System;

public partial class Story : Control
{
	private AnimationPlayer _animationPlayer;
	private Label _narratorText;
	private Button _continueTextButton;
	private LineEdit _playerNameInput;
	private Button _confirmNameButton;
	private Label _warningLabel;
	private Timer _warningTimer;
	private string _playerName = "Player";
	private int _currentTextIndex = 0;
	private bool _askingForName = false;
	private Label _nothingLabel;
	private Button _skipButton;

	private string[] _narrationTexts = new string[]
	{
		"In a realm where shadows whisper and secrets abound, a lone figure steps forward ...",
		"His name is ..... ",
		"His name is {playerName} !!!",
		"Cloaked in mystery, your presence sends ripples through the very fabric of the world.",
		"Your eyes, deep with untold stories, {playerName} is no ordinary adventurer. Legends speak of this enigmatic wanderer, driven by motives unknown.",
		"One day, {playerName} decided to embark on a journey ...",
		"And so the adventure begins ..."
	};

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("BackgroundAnimation");
		_narratorText = GetNode<Label>("NarratorText");
		_continueTextButton = GetNode<Button>("ContinueText");
		_playerNameInput = GetNode<LineEdit>("VBoxContainer/PlayerNameInput");
		_confirmNameButton = GetNode<Button>("VBoxContainer/ConfirmNameButton");
		_warningLabel = GetNode<Label>("VBoxContainer2/WarningLabel");
		_nothingLabel = GetNode<Label>("nothing");
		_skipButton = GetNode<Button>("Skip");

		GD.Print($"AnimationPlayer found: {_animationPlayer != null}");
		GD.Print($"NarratorText found: {_narratorText != null}");
		GD.Print($"ContinueText button found: {_continueTextButton != null}");
		GD.Print($"PlayerNameInput found: {_playerNameInput != null}");
		GD.Print($"ConfirmNameButton found: {_confirmNameButton != null}");
		GD.Print($"WarningLabel found: {_warningLabel != null}");
		GD.Print($"NothingLabel found: {_nothingLabel != null}");
		GD.Print($"SkipButton found: {_skipButton != null}");

		_continueTextButton.Pressed += OnContinueTextPressed;
		_confirmNameButton.Pressed += OnConfirmNamePressed;
		_playerNameInput.TextSubmitted += _on_player_name_input_text_submitted;
		_skipButton.Pressed += OnSkipButtonPressed;

		_playerNameInput.Visible = false;
		_confirmNameButton.Visible = false;
		_warningLabel.Visible = false;

		_playerNameInput.MaxLength = 24;
		_playerNameInput.TextChanged += OnTextChanged;

		_warningTimer = new Timer();
		_warningTimer.WaitTime = 3.0f;
		_warningTimer.OneShot = true;
		AddChild(_warningTimer);
		_warningTimer.Timeout += OnWarningTimeout;

		ShowNextText();

		if (_animationPlayer != null)
		{
			GD.Print("Starting BackgroundAnimation");
			_animationPlayer.Play("BackgroundAnimation");
		}
		else
		{
			GD.Print("AnimationPlayer node not found or animation name incorrect");
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Keycode == Key.Space)
		{
			if (_continueTextButton.Visible)
			{
				OnContinueTextPressed();
			}
		}
	}

	private void OnConfirmNamePressed()
	{
		_playerName = _playerNameInput.Text;
		_playerNameInput.Visible = false;
		_confirmNameButton.Visible = false;
		_narratorText.Visible = true;
		_continueTextButton.Visible = true;
		_nothingLabel.Visible = true;

		_askingForName = false;
		ShowNextText();
	}

	private void _on_player_name_input_text_submitted(string newText)
	{
		OnConfirmNamePressed();
	}

	private void OnContinueTextPressed()
	{
		if (_narratorText.Text == "And so the adventure begins ...")
		{
			GetTree().ChangeSceneToFile("res://scenes/AdventureScene.tscn");
		}
		else
		{
			ShowNextText();
		}
	}

	private void OnSkipButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/AdventureScene.tscn");
	}

	private void OnTextChanged(string newText)
	{
		if (newText.Length >= 24)
		{
			GD.Print("Warning: Maximum character limit reached!");
			_warningLabel.Text = "Maximum character limit is 24.";
			_warningLabel.Visible = true;
			_warningTimer.Start();
		}
	}

	private void OnWarningTimeout()
	{
		GD.Print("Hiding warning label");
		_warningLabel.Visible = false;
	}

	private void ShowNextText()
	{
		if (_askingForName)
		{
			return;
		}

		if (_currentTextIndex < _narrationTexts.Length)
		{
			if (_narrationTexts[_currentTextIndex].Contains("{playerName}"))
			{
				if (_playerName == "Player")
				{
					_askingForName = true;
					_narratorText.Text = "What is your name?";
					_narratorText.Visible = false;
					_playerNameInput.Visible = true;
					_confirmNameButton.Visible = true;
					_continueTextButton.Visible = false;
					_nothingLabel.Visible = false;
					return;
				}
				else
				{
					_narratorText.Text = _narrationTexts[_currentTextIndex].Replace("{playerName}", _playerName);
				}
			}
			else
			{
				_narratorText.Text = _narrationTexts[_currentTextIndex];
			}
			_currentTextIndex++;
		}
		else
		{
			_narratorText.Text = "And so the adventure begins ...";
			_continueTextButton.Pressed += OnAdventureBeginPressed;
		}
		_nothingLabel.Visible = _continueTextButton.Visible;
	}

	private void OnAdventureBeginPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/AdventureScene.tscn");
	}
}
