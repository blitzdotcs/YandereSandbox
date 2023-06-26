using System;
using UnityEngine;

[Serializable]
public class GridScript : MonoBehaviour
{
	public GameObject Tile;

	public int Row;

	public int Column;

	public int Rows;

	public int Columns;

	public int ID;

	public GridScript()
	{
		Rows = 25;
		Columns = 25;
	}

	public virtual void Start()
	{
		while (ID < Rows * Columns)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Tile, new Vector3(Row, 0f, Column), Quaternion.identity);
			gameObject.transform.parent = transform;
			Row++;
			if (Row > Rows)
			{
				Row = 1;
				Column++;
			}
			ID++;
		}
		transform.localScale = new Vector3(4f, 4f, 4f);
		transform.position = new Vector3(-52f, 0f, -52f);
	}

	public virtual void Main()
	{
	}
}
