using Pathfinding;
using UnityEngine;

public class LocalSpaceRichAI : RichAI
{
	public LocalSpaceGraph graph;

	public override void UpdatePath()
	{
		canSearchPath = true;
		waitingForPathCalc = false;
		Path currentPath = seeker.GetCurrentPath();
		if (currentPath != null && !seeker.IsDone())
		{
			currentPath.Error();
			currentPath.Claim(this);
			currentPath.Release(this);
		}
		waitingForPathCalc = true;
		lastRepath = Time.time;
		Matrix4x4 matrix = graph.GetMatrix();
		seeker.StartPath(matrix.MultiplyPoint3x4(tr.position), matrix.MultiplyPoint3x4(target.position));
	}

	protected override Vector3 UpdateTarget(RichFunnel fn)
	{
		Matrix4x4 matrix = graph.GetMatrix();
		Matrix4x4 inverse = matrix.inverse;
		Debug.DrawRay(matrix.MultiplyPoint3x4(tr.position), Vector3.up * 2f, Color.red);
		Debug.DrawRay(inverse.MultiplyPoint3x4(tr.position), Vector3.up * 2f, Color.green);
		buffer.Clear();
		Vector3 position = tr.position;
		bool requiresRepath;
		position = inverse.MultiplyPoint3x4(fn.Update(matrix.MultiplyPoint3x4(position), buffer, 2, out lastCorner, out requiresRepath));
		Debug.DrawRay(position, Vector3.up * 3f, Color.black);
		for (int i = 0; i < buffer.Count; i++)
		{
			buffer[i] = inverse.MultiplyPoint3x4(buffer[i]);
			Debug.DrawRay(buffer[i], Vector3.up * 3f, Color.yellow);
		}
		if (requiresRepath && !waitingForPathCalc)
		{
			UpdatePath();
		}
		return position;
	}
}
