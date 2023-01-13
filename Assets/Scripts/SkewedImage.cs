using UnityEngine;
using UnityEngine.UI;

public class SkewedImage : Image
{
	[SerializeField] private float skewX;

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
	}
}
