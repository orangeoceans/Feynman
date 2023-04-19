using Godot;
using System;

public partial class Edge : Node2D
{
	public Vector2 startPoint;
	public Vector2 endPoint;
	public Vector2 edgeVector;
	
	public bool placed = false;
	public Vertex startVertex;
	public Vertex endVertex;
	public float slope = 0f;
	
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
	
	public void PlaceEdge(Vertex start, Vertex end)
	{
		placed = true;
		startVertex = start;
		endVertex = end;
		startPoint = start.Position;
		endPoint = end.Position;
		edgeVector = startPoint - endPoint;
		slope = CalculateSlope(startPoint, endPoint);
		UpdatePlacement();
	}
	
	public bool OverlapsWith(Edge edge)
	{
		float startSlope = CalculateSlope(startPoint, edge.startPoint);
		float endSlope   = CalculateSlope(endPoint, edge.endPoint);
		
		bool slopesMatch = Mathf.IsEqualApprox(slope, edge.slope);
		bool startsColinear = float.IsNaN(startSlope) || Mathf.IsEqualApprox(startSlope, slope);
		bool endsColinear = float.IsNaN(endSlope) || Mathf.IsEqualApprox(endSlope, slope);
		
		GD.Print($"startSlope {startSlope} endSlope {endSlope} slope {slope} edge.slope {edge.slope}");
		
		if (slopesMatch && startsColinear && endsColinear) {
			// Calculate the bounding boxes of the line segments
			float minX1 = Mathf.Min(startPoint.X, endPoint.X);
			float minY1 = Mathf.Min(startPoint.Y, endPoint.Y);
			float maxX1 = Mathf.Max(startPoint.X, endPoint.X);
			float maxY1 = Mathf.Max(startPoint.Y, endPoint.Y);

			float minX2 = Mathf.Min(edge.startPoint.X, edge.endPoint.X);
			float minY2 = Mathf.Min(edge.startPoint.Y, edge.endPoint.Y);
			float maxX2 = Mathf.Max(edge.startPoint.X, edge.endPoint.X);
			float maxY2 = Mathf.Max(edge.startPoint.Y, edge.endPoint.Y);

			// Check if the bounding boxes intersect
			if (maxX1 >= minX2 && maxX2 >= minX1 && maxY1 >= minY2 && maxY2 >= minY1)
				return true;
		}
		return false;
	}
	
	public static float CalculateSlope(Vector2 start, Vector2 end)
	{
		float m = -(end.Y - start.Y)/(end.X - start.X);
		if (float.IsInfinity(m))
			m = Math.Abs(m);
		return m;
	}
}
