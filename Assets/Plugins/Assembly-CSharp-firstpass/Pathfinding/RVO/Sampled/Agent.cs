using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Pathfinding.RVO.Sampled
{
	public class Agent : IAgent
	{
		public struct VO
		{
			public Vector2 origin;

			private Vector2 line1;

			private Vector2 line2;

			private Vector2 dir1;

			private Vector2 dir2;

			private Vector2 cutoffLine;

			private Vector2 cutoffDir;

			private float sqrCutoffDistance;

			private bool leftSide;

			private bool colliding;

			private static bool Left(Vector2 a, Vector2 dir, Vector2 p)
			{
				return dir.x * (p.y - a.y) - (p.x - a.x) * dir.y <= 0f;
			}

			private static float Det(Vector2 a, Vector2 dir, Vector2 p)
			{
				return (p.x - a.x) * dir.y - dir.x * (p.y - a.y);
			}

			public Vector2 Sample(Vector2 p, out float weight)
			{
				if (colliding)
				{
					float num = Det(line1, dir1, p);
					if (num >= 0f)
					{
						weight = num * 0.5f;
						return new Vector2(0f - dir1.y, dir1.x) * weight * GlobalIncompressibility;
					}
					weight = 0f;
					return new Vector2(0f, 0f);
				}
				float num2 = Det(line1, dir1, p);
				float num3 = Det(line2, dir2, p);
				if (num2 >= 0f && num3 >= 0f)
				{
					float num4 = Det(cutoffLine, cutoffDir, p);
					if (num4 <= 0f)
					{
						weight = 0f;
						return Vector2.zero;
					}
					if (leftSide)
					{
						if (num4 < num2)
						{
							weight = num4 * 0.5f;
							return new Vector2(0f - cutoffDir.y, cutoffDir.x) * weight;
						}
						weight = num2 * 0.5f;
						return new Vector2(0f - dir1.y, dir1.x) * weight;
					}
					if (num3 < num2)
					{
						weight = num3 * 0.5f;
						return new Vector2(0f - cutoffDir.y, cutoffDir.x) * weight;
					}
					weight = num3 * 0.5f;
					return new Vector2(0f - dir2.y, dir2.x) * weight;
				}
				weight = 0f;
				return new Vector2(0f, 0f);
			}
		}

		private Vector3 smoothPos;

		public float radius;

		public float height;

		public float maxSpeed;

		public float neighbourDist;

		public float agentTimeHorizon;

		public float obstacleTimeHorizon;

		public float weight;

		public bool locked;

		private RVOLayer layer;

		private RVOLayer collidesWith;

		public int maxNeighbours;

		public Vector3 position;

		public Vector3 desiredVelocity;

		public Vector3 prevSmoothPos;

		internal Agent next;

		private Vector3 velocity;

		private Vector3 newVelocity;

		public Simulator simulator;

		public List<Agent> neighbours = new List<Agent>();

		public List<float> neighbourDists = new List<float>();

		private List<ObstacleVertex> obstaclesBuffered = new List<ObstacleVertex>();

		private List<ObstacleVertex> obstacles = new List<ObstacleVertex>();

		private List<float> obstacleDists = new List<float>();

		public static Stopwatch watch1 = new Stopwatch();

		public static Stopwatch watch2 = new Stopwatch();

		public static float DesiredVelocityWeight = 0.1f;

		public static float DesiredVelocityScale = 0.1f;

		public static float GlobalIncompressibility = 30f;

		public Vector3 Position { get; set; }

		public Vector3 InterpolatedPosition
		{
			get
			{
				return smoothPos;
			}
		}

		public Vector3 DesiredVelocity { get; set; }

		public RVOLayer Layer { get; set; }

		public RVOLayer CollidesWith { get; set; }

		public bool Locked { get; set; }

		public float Radius { get; set; }

		public float Height { get; set; }

		public float MaxSpeed { get; set; }

		public float NeighbourDist { get; set; }

		public float AgentTimeHorizon { get; set; }

		public float ObstacleTimeHorizon { get; set; }

		public Vector3 Velocity { get; set; }

		public bool DebugDraw { get; set; }

		public int MaxNeighbours { get; set; }

		public List<ObstacleVertex> NeighbourObstacles
		{
			get
			{
				return null;
			}
		}

		public Agent(Vector3 pos)
		{
			MaxSpeed = 2f;
			NeighbourDist = 15f;
			AgentTimeHorizon = 2f;
			ObstacleTimeHorizon = 2f;
			Height = 5f;
			Radius = 5f;
			MaxNeighbours = 10;
			Locked = false;
			position = pos;
			Position = position;
			prevSmoothPos = position;
			smoothPos = position;
			CollidesWith = (RVOLayer)(-1);
		}

		public void Teleport(Vector3 pos)
		{
			Position = pos;
			smoothPos = pos;
			prevSmoothPos = pos;
		}

		public void BufferSwitch()
		{
			radius = Radius;
			height = Height;
			maxSpeed = MaxSpeed;
			neighbourDist = NeighbourDist;
			agentTimeHorizon = AgentTimeHorizon;
			obstacleTimeHorizon = ObstacleTimeHorizon;
			maxNeighbours = MaxNeighbours;
			desiredVelocity = DesiredVelocity;
			locked = Locked;
			collidesWith = CollidesWith;
			layer = Layer;
			Velocity = velocity;
			List<ObstacleVertex> list = obstaclesBuffered;
			obstaclesBuffered = obstacles;
			obstacles = list;
		}

		public void Update()
		{
			velocity = newVelocity;
			prevSmoothPos = smoothPos;
			position = Position;
			position += velocity * simulator.DeltaTime;
			Position = position;
		}

		public void Interpolate(float t)
		{
			if (t == 1f)
			{
				smoothPos = Position;
			}
			else
			{
				smoothPos = prevSmoothPos + (Position - prevSmoothPos) * t;
			}
		}

		public void CalculateNeighbours()
		{
			neighbours.Clear();
			neighbourDists.Clear();
			if (!locked)
			{
				float num;
				if (MaxNeighbours > 0)
				{
					num = neighbourDist * neighbourDist;
					simulator.Quadtree.Query(new Vector2(position.x, position.z), neighbourDist, this);
				}
				obstacles.Clear();
				obstacleDists.Clear();
				num = obstacleTimeHorizon * maxSpeed + radius;
				num *= num;
			}
		}

		private float Sqr(float x)
		{
			return x * x;
		}

		public float InsertAgentNeighbour(Agent agent, float rangeSq)
		{
			if (this == agent)
			{
				return rangeSq;
			}
			if ((agent.layer & collidesWith) == 0)
			{
				return rangeSq;
			}
			float num = Sqr(agent.position.x - position.x) + Sqr(agent.position.z - position.z);
			if (num < rangeSq)
			{
				if (neighbours.Count < maxNeighbours)
				{
					neighbours.Add(agent);
					neighbourDists.Add(num);
				}
				int num2 = neighbours.Count - 1;
				if (num < neighbourDists[num2])
				{
					while (num2 != 0 && num < neighbourDists[num2 - 1])
					{
						neighbours[num2] = neighbours[num2 - 1];
						neighbourDists[num2] = neighbourDists[num2 - 1];
						num2--;
					}
					neighbours[num2] = agent;
					neighbourDists[num2] = num;
				}
				if (neighbours.Count == maxNeighbours)
				{
					rangeSq = neighbourDists[neighbourDists.Count - 1];
				}
			}
			return rangeSq;
		}

		public void InsertObstacleNeighbour(ObstacleVertex ob1, float rangeSq)
		{
			ObstacleVertex obstacleVertex = ob1.next;
			float num = AstarMath.DistancePointSegmentStrict(ob1.position, obstacleVertex.position, Position);
			if (num < rangeSq)
			{
				obstacles.Add(ob1);
				obstacleDists.Add(num);
				int num2 = obstacles.Count - 1;
				while (num2 != 0 && num < obstacleDists[num2 - 1])
				{
					obstacles[num2] = obstacles[num2 - 1];
					obstacleDists[num2] = obstacleDists[num2 - 1];
					num2--;
				}
				obstacles[num2] = ob1;
				obstacleDists[num2] = num;
			}
		}

		private static Vector3 To3D(Vector2 p)
		{
			return new Vector3(p.x, 0f, p.y);
		}

		private static void DrawCircle(Vector2 _p, float radius, Color col)
		{
			DrawCircle(_p, radius, 0f, (float)Math.PI * 2f, col);
		}

		private static void DrawCircle(Vector2 _p, float radius, float a0, float a1, Color col)
		{
			Vector3 vector = To3D(_p);
			while (a0 > a1)
			{
				a0 -= (float)Math.PI * 2f;
			}
			Vector3 vector2 = new Vector3(Mathf.Cos(a0) * radius, 0f, Mathf.Sin(a0) * radius);
			for (int i = 0; (float)i <= 40f; i++)
			{
				Vector3 vector3 = new Vector3(Mathf.Cos(Mathf.Lerp(a0, a1, (float)i / 40f)) * radius, 0f, Mathf.Sin(Mathf.Lerp(a0, a1, (float)i / 40f)) * radius);
				UnityEngine.Debug.DrawLine(vector + vector2, vector + vector3, col);
				vector2 = vector3;
			}
		}

		private static void DrawVO(Vector2 circleCenter, float radius, Vector2 origin)
		{
			float num = Mathf.Atan2((origin - circleCenter).y, (origin - circleCenter).x);
			float num2 = radius / (origin - circleCenter).magnitude;
			float num3 = ((!(num2 <= 1f)) ? 0f : Mathf.Abs(Mathf.Acos(num2)));
			DrawCircle(circleCenter, radius, num - num3, num + num3, Color.black);
			Vector2 p = new Vector2(Mathf.Cos(num - num3), Mathf.Sin(num - num3)) * radius;
			Vector2 p2 = new Vector2(Mathf.Cos(num + num3), Mathf.Sin(num + num3)) * radius;
			Vector2 p3 = -new Vector2(0f - p.y, p.x);
			Vector2 p4 = new Vector2(0f - p2.y, p2.x);
			p += circleCenter;
			p2 += circleCenter;
			UnityEngine.Debug.DrawRay(To3D(p), To3D(p3).normalized * 100f, Color.black);
			UnityEngine.Debug.DrawRay(To3D(p2), To3D(p4).normalized * 100f, Color.black);
		}

		private static void DrawCross(Vector2 p, float size = 1f)
		{
			DrawCross(p, Color.white, size);
		}

		private static void DrawCross(Vector2 p, Color col, float size = 1f)
		{
			size *= 0.5f;
			UnityEngine.Debug.DrawLine(new Vector3(p.x, 0f, p.y) - Vector3.right * size, new Vector3(p.x, 0f, p.y) + Vector3.right * size, col);
			UnityEngine.Debug.DrawLine(new Vector3(p.x, 0f, p.y) - Vector3.forward * size, new Vector3(p.x, 0f, p.y) + Vector3.forward * size, col);
		}

		internal void CalculateVelocity(Simulator.WorkerContext context)
		{
			if (locked)
			{
				newVelocity = Vector2.zero;
				return;
			}
			if (context.vos.Length < neighbours.Count)
			{
				context.vos = new VO[Mathf.Max(context.vos.Length * 2, neighbours.Count)];
			}
			Vector2 vector = new Vector2(position.x, position.z);
			VO[] vos = context.vos;
			int num = 0;
			Vector2 vector2 = new Vector2(velocity.x, velocity.z);
			float inverseDt = 1f / agentTimeHorizon;
			for (int i = 0; i < neighbours.Count; i++)
			{
				Agent agent = neighbours[i];
				if (agent != this)
				{
					float num2 = Math.Min(position.y + height, agent.position.y + agent.height);
					float num3 = Math.Max(position.y, agent.position.y);
					if (!(num2 - num3 < 0f))
					{
						Vector2 vector3 = new Vector2(agent.Velocity.x, agent.velocity.z);
						float num4 = radius + agent.radius;
						Vector2 center = new Vector2(agent.position.x, agent.position.z) - vector;
						Vector2 sideChooser = vector2 - vector3;
					//	vos[num] = new VO(center, (vector2 + vector3) * 0.5f, num4, sideChooser, inverseDt);
						num++;
					}
				}
			}
			if (DebugDraw)
			{
			}
			float score = float.PositiveInfinity;
			Vector2 zero = Vector2.zero;
			float cutoff = new Vector2(velocity.x, velocity.z).magnitude * simulator.qualityCutoff;
			zero = Trace(vos, num, new Vector2(desiredVelocity.x, desiredVelocity.z), cutoff, out score);
			Vector2 zero2 = Vector2.zero;
			float score2;
			Vector2 vector4 = Trace(vos, num, zero2, cutoff, out score2);
			if (score2 < score)
			{
				zero = vector4;
				score = score2;
			}
			if (DebugDraw)
			{
				DrawCross(zero + vector, 1f);
			}
			newVelocity = To3D(Vector2.ClampMagnitude(zero, maxSpeed));
		}

		private static Color Rainbow(float v)
		{
			Color result = new Color(v, 0f, 0f);
			if (result.r > 1f)
			{
				result.g = result.r - 1f;
				result.r = 1f;
			}
			if (result.g > 1f)
			{
				result.b = result.g - 1f;
				result.g = 1f;
			}
			return result;
		}

		private Vector2 Trace(VO[] vos, int voCount, Vector2 p, float cutoff, out float score)
		{
			score = 0f;
			float stepScale = simulator.stepScale;
			for (int i = 0; i < 50; i++)
			{
				float num = 1f - (float)i / 50f;
				num *= stepScale;
				Vector2 zero = Vector2.zero;
				float num2 = 0f;
				for (int j = 0; j < voCount; j++)
				{
					float num3;
					Vector2 vector = vos[j].Sample(p, out num3);
					zero += vector;
					if (num3 > num2)
					{
						num2 = num3;
					}
				}
				Vector2 vector2 = new Vector2(desiredVelocity.x, desiredVelocity.z) - p;
				float val = vector2.magnitude * DesiredVelocityWeight;
				zero += vector2 * DesiredVelocityScale;
				num2 = (score = Math.Max(num2, val));
				float sqrMagnitude = zero.sqrMagnitude;
				if (sqrMagnitude > 0f)
				{
					zero *= num2 / Mathf.Sqrt(sqrMagnitude);
				}
				zero *= num;
				p += zero;
				if (score < cutoff)
				{
					break;
				}
			}
			return p;
		}

		public static bool IntersectionFactor(Vector2 start1, Vector2 dir1, Vector2 start2, Vector2 dir2, out float factor)
		{
			float num = dir2.y * dir1.x - dir2.x * dir1.y;
			if (num == 0f)
			{
				factor = 0f;
				return false;
			}
			float num2 = dir2.x * (start1.y - start2.y) - dir2.y * (start1.x - start2.x);
			factor = num2 / num;
			return true;
		}
	}
}
