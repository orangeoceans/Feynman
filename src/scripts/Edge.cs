using Godot;
using System;

public partial class Edge : Node2D
{
	public Vector2 startPoint;
	public Vector2 endPoint;
	public bool placed = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		placed = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!placed) {
			UpdatePlacement();
		}
	}
	
	public void UpdatePlacement()
	{
		float lineLength = startPoint.DistanceTo(endPoint);
		float lineAngle = (startPoint - endPoint).Angle();
		this.Scale = new Vector2(lineLength, 5);
		this.Rotation = lineAngle;	
		this.Position = startPoint+(endPoint-startPoint)/2;
	}
}
