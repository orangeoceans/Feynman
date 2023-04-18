using Godot;
using System;
using System.Collections.Generic;

public partial class DiagramManager : Node2D
{
	[Export]
	public int spacing = 50;
	public int border = 100;
	
	private PackedScene vertexScene = (PackedScene)GD.Load("res://src/scenes/Vertex.tscn");
	private PackedScene edgeScene = (PackedScene)GD.Load("res://src/scenes/Edge.tscn");
	private Vertex[,] vertexArray;
	private Vertex activeVertex = null;
	private Edge activeEdge = null;
	private List<Edge> edgeList = new List<Edge>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int areaWidth = (int)ProjectSettings.GetSetting("display/window/size/viewport_width") - border*2;
		int areaHeight = (int)ProjectSettings.GetSetting("display/window/size/viewport_height") - border*2;
		
		int numRows = (int)(areaWidth/spacing) + 1;
		int numCols = (int)(areaHeight/spacing) + 1;
		
		int remainderX = areaWidth - spacing*(numRows-1);
		int remainderY = areaHeight - spacing*(numCols-1);
		
		vertexArray = new Vertex[numRows, numCols];
		for (int row = 0; row < numRows; row+=1) {
			for (int col = 0; col < numCols; col+=1) {
				Vertex vertex = (Vertex)vertexScene.Instantiate();
				vertex.Position = new Vector2(row*spacing + border + remainderX/2, col*spacing + border + remainderY/2);
				vertex.ZIndex = 0;
				vertex.EdgeToggled += OnEdgeToggled;
				AddChild(vertex);
				vertexArray[row, col] = vertex;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (activeEdge != null) {
			activeEdge.endPoint = GetViewport().GetMousePosition();
		}
	}
	
	private void OnEdgeToggled(Vertex vertex)
	{
		GD.Print("toggle");
		if (activeEdge == null) {
			activeVertex = vertex;
			activeEdge = (Edge)edgeScene.Instantiate();
			activeEdge.ZIndex = -100;
			activeEdge.startPoint = vertex.Position;
			activeEdge.endPoint = GetViewport().GetMousePosition();
			AddChild(activeEdge);
		} else {
			// Compare vectors robust to floating point precision
			// If the two vertices are NOT the same
			if (activeVertex.Position != vertex.Position) {
				activeEdge.endPoint = vertex.Position;
				activeEdge.placed = true;
				activeEdge.UpdatePlacement();
				edgeList.Add(activeEdge);
			} else {
				GD.Print("Edge destroyed.");
				activeEdge.QueueFree();
			}
			activeEdge = null;
			activeVertex = null;
		}
	}
}
