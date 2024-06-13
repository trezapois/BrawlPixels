using Godot;
using System;

public partial class VolumeSlider : HSlider
{
	[Export]
	public string bus_name;

	private int bus_index;

	public override void _Ready()
	{
		bus_index = AudioServer.GetBusIndex(bus_name);
		this.Connect("value_changed", new Callable(this, nameof(OnValueChanged)));
	}

	private void OnValueChanged(float value)
	{
		// Manual conversion from linear to decibel using System.Math
		float db = 20.0f * (float)Math.Log10(Math.Max(value, 0.0001f));
		AudioServer.SetBusVolumeDb(bus_index, db);
	}
}
