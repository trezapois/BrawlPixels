using Godot;
using System;
using System.Collections.Generic;

public static class turn
{
	public static int swap(int initial)
	{
		if(initial % 3 == 0)
		{
			return initial - 2;
		}
		else if(initial % 3 == 1)
		{
			return initial + 2;
		}
		return initial;
	}
	
	public static List<int> swaplist(List<int> l)
	{
		var res = new List<int>();
		foreach(var i in l)
		{
			res.Add(swap(i));
		}
		return res;
	}
}
