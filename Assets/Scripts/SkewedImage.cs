using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkewedImage : Image
{
	[SerializeField] private float skewX;

	//protected override void OnPopulateMesh(Mesh m)
	//{
	//	base.OnPopulateMesh(m);
	//	m.
	//}

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		base.OnPopulateMesh(toFill);
		Rect rect = GetPixelAdjustedRect();
		float centerY = rect.center.y;
		float height = rect.height / 2;

		for (int i = 0; i < toFill.currentVertCount; ++i)
		{
			UIVertex vert = default;
			toFill.PopulateUIVertex(ref vert, i);
			float verticalOffset = (vert.position.y - centerY) / height;
			float skewOffset = verticalOffset * skewX;
			vert.position += Vector3.right * skewOffset;
			toFill.SetUIVertex(vert, i);
		}

		//toFill.Clear();
		//toFill.AddVert(new Vector3(rect.x - skewX, rect.y), color, new Vector2(0, 0));
		//toFill.AddVert(new Vector3(rect.x + skewX, rect.y + rect.height), color, new Vector2(0, 1));
		//toFill.AddVert(new Vector3(rect.x + rect.width + skewX, rect.y + rect.height), color, new Vector2(1, 1));
		//toFill.AddVert(new Vector3(rect.x + rect.width - skewX, rect.y), color, new Vector2(1, 0));
		//toFill.AddTriangle(0, 1, 2);
		//toFill.AddTriangle(2, 3, 0);
	}
}
