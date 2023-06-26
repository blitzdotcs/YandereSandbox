namespace Pathfinding
{
	public struct PathThreadInfo
	{
		public int threadIndex;

		public AstarPath astar;

		public PathHandler runData;

		private object _lock;

		public object Lock
		{
			get
			{
				return _lock;
			}
		}

		public PathThreadInfo(int index, AstarPath astar, PathHandler runData)
		{
			threadIndex = index;
			this.astar = astar;
			this.runData = runData;
			_lock = new object();
		}
	}
}
