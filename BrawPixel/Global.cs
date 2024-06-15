using Godot;
using System.Collections.Generic;

public partial class Global : Node
{
	private Dictionary<int, bool> stageCompletionStatus = new Dictionary<int, bool>();

	public override void _Ready()
	{
		stageCompletionStatus[1] = false;
		stageCompletionStatus[2] = false;
		stageCompletionStatus[3] = false;
	}

	public bool IsStageCompleted(int stageNumber)
	{
		if (stageCompletionStatus.ContainsKey(stageNumber))
		{
			return stageCompletionStatus[stageNumber];
		}
		return false;
	}

	public void SetStageCompleted(int stageNumber, bool completed)
	{
		if (stageCompletionStatus.ContainsKey(stageNumber))
		{
			stageCompletionStatus[stageNumber] = completed;
		}
	}
}
