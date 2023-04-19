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
	
	private float[] rowPositions;
	private float[] colPositions;
	private Vertex[,] vertexArray;
	
	private Vertex activeVertex = null;
	private Edge activeEdge = null;
	private List<Edge> edgeList = new List<Edge>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CreateVertexGrid();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (activeEdge != null) {
			activeEdge.endPoint = GetViewport().GetMousePosition();
		}
	}
	
	private void CreateVertexGrid()
	{
		int areaWidth = (int)ProjectSettings.GetSetting("display/window/size/viewport_width") - border*2;
		int areaHeight = (int)ProjectSettings.GetSetting("display/window/size/viewport_height") - border*2;
		
		int numRows = (int)(areaWidth/spacing) + 1;
		int numCols = (int)(areaHeight/spacing) + 1;
		
		int remainderX = areaWidth - spacing*(numRows-1);
		int remainderY = areaHeight - spacing*(numCols-1);
		
		rowPositions = new float[numRows];
		colPositions = new float[numCols];
		vertexArray = new Vertex[numRows, numCols];
		for (int row = 0; row < numRows; row+=1) {
			for (int col = 0; col < numCols; col+=1) {
				Vertex vertex = (Vertex)vertexScene.Instantiate();
				vertex.Position = new Vector2(row*spacing + border + remainderX/2, col*spacing + border + remainderY/2);
				rowPositions[row] = vertex.Position.X;
				colPositions[col] = vertex.Position.Y;
				vertex.ZIndex = 0;
				vertex.EdgeToggled += OnEdgeToggled;
				AddChild(vertex);
				vertexArray[row, col] = vertex;
			}
		}
	}
	
	private void OnEdgeToggled(Vertex vertex)
	{
		if (activeEdge == null) {
			activeVertex = vertex;
			activeEdge = (Edge)edgeScene.Instantiate();
			activeEdge.ZIndex = -100;
			activeEdge.startPoint = vertex.Position;
			activeEdge.endPoint = GetViewport().GetMousePosition();
			AddChild(activeEdge);
		} else {
			activeEdge.PlaceEdge(activeVertex, vertex);
			bool canAdd = activeVertex.ConnectToVertex(vertex);
			foreach (Edge edge in edgeList) {
				canAdd = canAdd && !edge.OverlapsWith(activeEdge);
			}
			if (canAdd) {
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
