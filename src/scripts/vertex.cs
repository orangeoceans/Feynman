using Godot;
using System;
using System.Collections.Generic;

public partial class Vertex : Node2D {
	bool dragging = false;
	bool drawingLine = false;
	
	[Export]
	public float appearAtMouseDistance = 200f;
	
	[Signal]
	public delegate void EdgeToggledEventHandler(Vertex vertex);
	
	private List<Vertex> connectedVertices;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		connectedVertices = new List<Vertex>();
		connectedVertices.Add(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		Vector2 mousePosition = GetViewport().GetMousePosition();
		float mouseDistance = this.Position.DistanceTo(mousePosition);
		if (mouseDistance <= appearAtMouseDistance) {
			float scale = (appearAtMouseDistance - mouseDistance)/appearAtMouseDistance;
			this.Scale = new Vector2(scale, scale);
		} else if (this.Scale.X != 0) {
			this.Scale = new Vector2(0, 0);
		}
		// To enable dragging vertices, uncomment
		// if (this.dragging) {
		//	this.Position = mousePosition;
		// }
	}
	
	public bool ConnectToVertex(Vertex vertex) {
		foreach (Vertex connectedVertex in connectedVertices) {
			if (connectedVertex.Position == vertex.Position) {
				return false;
			}
		}
		connectedVertices.Add(vertex);
		vertex.ConnectToVertex(this);
		return true;
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
