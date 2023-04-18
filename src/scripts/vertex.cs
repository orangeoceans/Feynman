using Godot;
using System;

public partial class Vertex : Node2D {
	bool dragging = false;
	bool drawingLine = false;
	
	[Signal]
	public delegate void EdgeToggledEventHandler(Vertex vertex);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		// Vector2 mousePosition = GetViewport().GetMousePosition();
		// To enable dragging vertices, uncomment
		// if (this.dragging) {
		//	this.Position = mousePosition;
		// }
	}
	
	public void _On_Area2D_Input_Event(Node viewport, InputEvent inputEvent, int shapeIdx) {
		if (inputEvent is InputEventMouseButton eventMouseButton) {
			// To enable dragging vertices, uncomment
			// if (eventMouseButton.ButtonIndex == MouseButton.Left && eventMouseButton.Pressed) {
			//	this.dragging = !this.dragging;
			//}
			if (eventMouseButton.ButtonIndex == MouseButton.Right && eventMouseButton.Pressed) {
				EmitSignal(SignalName.EdgeToggled, this);
			}
		}
		// To enable dragging vertices, uncomment
		// else if (inputEvent is InputEventScreenTouch eventScreenTouch) {
		//	if (eventScreenTouch.Pressed && eventScreenTouch.Index == 0) {
		//		this.Position = eventScreenTouch.Position;
		//	}
		//}
	}
}
