using UnityEngine;

public class ActorPathManager 
{
	#region Static Fields

    private static readonly object _lock = new object();

    private static ActorPathManager instance;

    #endregion

    #region Constructors and Destructors

    private ActorPathManager()
    {
    }

    #endregion

    #region Public Methods and Operators

    public static ActorPathManager GetInstance()
    {
        if (instance == null)
        {
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = new ActorPathManager();
                }
            }
        }
        return instance;
    }
	
	private Vector3[] firstPathLeftNodes = new Vector3[3]{new Vector3(295,547,0), new Vector3(295, 504, 0), new Vector3(295,461,0)};
	private Vector3[] firstPathRightNodes = new Vector3[3]{new Vector3(665,547,0), new Vector3(665, 504, 0), new Vector3(665,461,0)};
	private Vector3[] secondPathLeftNodes = new Vector3[3]{new Vector3(295,253,0), new Vector3(295, 296, 0), new Vector3(295,339,0)};
	private Vector3[] secondPathRightNodes = new Vector3[3]{new Vector3(665,253,0), new Vector3(665, 296, 0), new Vector3(665,339,0)};
	
	public ActorPath GenerateNewPath(Vector3 startPos, Vector3 endPos)
	{
		Vector3[] pathNodes;
		if(endPos.x <= 665 && startPos.x < endPos.x || endPos.x >= 295 && startPos.x > endPos.x)
			pathNodes = new Vector3[3];
		else
			pathNodes = new Vector3[4];
		pathNodes[0] = startPos;
		
		ActorPathType pathType;
		if(startPos.y > 400)
			pathType = ActorPathType.FirstPath;
		else if(startPos.y < 400)
			pathType = ActorPathType.SecondPath;
		else
		{
			pathType = (ActorPathType)Random.Range(0,2);
		}
		
		if(pathType == ActorPathType.FirstPath)
		{
			if(startPos.x < endPos.x)
			{
				pathNodes[1] = this.firstPathLeftNodes[Random.Range(0, 3)];
				pathNodes[2] = this.firstPathRightNodes[Random.Range(0,3)];
			}
			else
			{
				pathNodes[1] = this.firstPathRightNodes[Random.Range(0, 3)];
				pathNodes[2] = this.firstPathLeftNodes[Random.Range(0, 3)];
			}
		}
		else
		{
			if(startPos.x < endPos.x)
			{
				pathNodes[1] = this.secondPathLeftNodes[Random.Range(0, 3)];
				pathNodes[2] = this.secondPathRightNodes[Random.Range(0,3)];
			}
			else
			{
				pathNodes[1] = this.secondPathRightNodes[Random.Range(0, 3)];
				pathNodes[2] = this.secondPathLeftNodes[Random.Range(0, 3)];
			}
		}
		pathNodes[pathNodes.Length-1] = endPos;
		return new ActorPath(pathNodes);
	}
	
}
#endregion