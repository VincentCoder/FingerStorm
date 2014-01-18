using UnityEngine;

public class ActorPath
{
	private Vector3[] pathNodes;
	public int CurrentIndex {get;set;}
	
	public ActorPath(Vector3[] nodes)
	{
		this.pathNodes = nodes;
		this.CurrentIndex = 1;
	}
	
	public bool HasNext()
	{
		return this.CurrentIndex < this.NodesCount() - 1;
	}
	
	public Vector3 NextNode()
	{
		this.CurrentIndex ++;
		return this.pathNodes[this.CurrentIndex]; 
	}
	
	public int NodesCount()
	{
		return this.pathNodes.Length;
	}
	
	public Vector3 CurrentNode()
	{
		return this.pathNodes[this.CurrentIndex];
	}
}
