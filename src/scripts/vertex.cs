using Godot;
using System;

public partial class vertex : Node2D {
	bool dragging = false;
	bool drawingLine = false;
	PackedScene edgeScene = (PackedScene)GD.Load("res://src/scenes/edge.tscn");
	Node2D edgeInstance = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		Vector2 mousePosition = GetViewport().GetMousePosition();
		if (this.dragging) {
			this.Position = mousePosition;
		}
		if (this.edgeInstance != null) {
			float lineLength = this.Position.DistanceTo(mousePosition);
			float lineAngle = (this.Position - mousePosition).Angle();
			this.edgeInstance.Scale = new Vector2(lineLength, 5);
			this.edgeInstance.Rotation = lineAngle;
			this.edgeInstance.Position = (mousePosition-this.Position)/2;
			GD.Print(this.Position);
		}
	}
	
	public void _On_Area2D_Input_Event(Node viewport, InputEvent inputEvent, int shapeIdx) {
		if (inputEvent is InputEventMouseButton eventMouseButton) {
			if (eventMouseButton.ButtonIndex == MouseButton.Left && eventMouseButton.Pressed) {
				this.dragging = !this.dragging;
			}
			if (eventMouseButton.ButtonIndex == MouseButton.Right && eventMouseButton.Pressed) {
				if (this.edgeInstance == null) {
					this.edgeInstance = (Node2D)edgeScene.Instantiate();
					AddChild(this.edgeInstance);
				} else {
					this.edgeInstance.QueueFree();
					this.edgeInstance = null;
				}
			}
		}
		else if (inputEvent is InputEventScreenTouch eventScreenTouch) {
			if (eventScreenTouch.Pressed && eventScreenTouch.Index == 0) {
				this.Position = eventScreenTouch.Position;
			}
		}
	}
}
