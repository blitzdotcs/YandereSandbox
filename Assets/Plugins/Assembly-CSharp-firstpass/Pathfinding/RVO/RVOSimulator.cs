using Pathfinding.RVO.Sampled;
using UnityEngine;

namespace Pathfinding.RVO
{
	[AddComponentMenu("Pathfinding/Local Avoidance/RVO Simulator")]
	public class RVOSimulator : MonoBehaviour
	{
		[Tooltip("Calculate local avoidance in between frames")]
		public bool doubleBuffering = true;

		[Tooltip("Interpolate positions between simulation timesteps")]
		public bool interpolation = true;

		[Tooltip("Desired FPS for rvo simulation. It is usually not necessary to run a crowd simulation at a very high fps.\nUsually 10-30 fps is enough, but can be increased for better quality.\nThe rvo simulation will never run at a higher fps than the game")]
		public int desiredSimulatonFPS = 20;

		[Tooltip("Number of RVO worker threads. If set to None, no multithreading will be used.")]
		public ThreadCount workerThreads = ThreadCount.Two;

		[Tooltip("A higher value will result in lower quality local avoidance but faster calculations. [0...1]")]
		public float qualityCutoff = 0.05f;

		public float stepScale = 1.5f;

		[Tooltip("Higher values will raise the penalty for agent-agent intersection")]
		public float incompressibility = 30f;

		public float desiredVelocityWeight = 0.1f;

		private Simulator simulator;

		public Simulator GetSimulator()
		{
			if (simulator == null)
			{
				Awake();
			}
			return simulator;
		}

		private void Awake()
		{
			if (desiredSimulatonFPS < 1)
			{
				desiredSimulatonFPS = 1;
			}
			if (simulator == null)
			{
				int workers = AstarPath.CalculateThreadCount(workerThreads);
				simulator = new Simulator(workers, doubleBuffering);
				simulator.Interpolation = interpolation;
				simulator.DesiredDeltaTime = 1f / (float)desiredSimulatonFPS;
			}
		}

		private void Update()
		{
			if (desiredSimulatonFPS < 1)
			{
				desiredSimulatonFPS = 1;
			}
			Agent.DesiredVelocityWeight = desiredVelocityWeight;
			Agent.GlobalIncompressibility = incompressibility;
			GetSimulator().DesiredDeltaTime = 1f / (float)desiredSimulatonFPS;
			GetSimulator().Interpolation = interpolation;
			GetSimulator().stepScale = stepScale;
			GetSimulator().qualityCutoff = qualityCutoff;
			GetSimulator().Update();
		}

		private void OnDestroy()
		{
			simulator.OnDestroy();
		}
	}
}
