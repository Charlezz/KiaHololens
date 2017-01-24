using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ImageUtil
{


	public static Texture2D LoadImageFromFile (string filePath)
	{

		Texture2D tex = null;
		byte[] fileData;

		if (File.Exists (filePath)) {
			fileData = File.ReadAllBytes (filePath);
			tex = new Texture2D (2, 2);
			tex.LoadImage (fileData); //..this will auto-resize the texture dimensions.
            fileData = null;
		} else {
			Debug.Log ("file does not exist");
		}
		return tex;
	}

	//takes too long time
	public static Texture2D ResizeTexture (Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D result = new Texture2D (targetWidth, targetHeight, source.format, true);
		Color[] rpixels = result.GetPixels (0);
		float incX = (1.0f / (float)targetWidth);
		float incY = (1.0f / (float)targetHeight);
		for (int px = 0; px < rpixels.Length; px++) {
			rpixels [px] = source.GetPixelBilinear (incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor (px / targetWidth)));
		}
		result.SetPixels (rpixels, 0);
		result.Apply ();

		return result;
	}

	//same as above T_T
	public static Texture2D ResizeTexture2 (Texture2D source, Vector2 size)
	{
		//*** Get All the source pixels
		Color[] aSourceColor = source.GetPixels (0);
		Vector2 vSourceSize = new Vector2 (source.width, source.height);
		         
		//*** Calculate New Size
		float xWidth = size.x;
		float xHeight = size.y;
		         
		//*** Make New
		Texture2D oNewTex = new Texture2D ((int)xWidth, (int)xHeight, TextureFormat.RGBA32, false);
		         
		//*** Make destination array
		int xLength = (int)xWidth * (int)xHeight;
		Color[] aColor = new Color[xLength];
		         
		Vector2 vPixelSize = new Vector2 (vSourceSize.x / xWidth, vSourceSize.y / xHeight);
		         
		//*** Loop through destination pixels and process
		Vector2 vCenter = new Vector2 ();
		for (int ii = 0; ii < xLength; ii++) {       
			//*** Figure out x&y
			float xX = (float)ii % xWidth;
			float xY = Mathf.Floor ((float)ii / xWidth);
			             
			//*** Calculate Center
			vCenter.x = (xX / xWidth) * vSourceSize.x;
			vCenter.y = (xY / xHeight) * vSourceSize.y;
			 
			//*** Average
			//*** Calculate grid around point
			int xXFrom = (int)Mathf.Max (Mathf.Floor (vCenter.x - (vPixelSize.x * 0.5f)), 0);
			int xXTo = (int)Mathf.Min (Mathf.Ceil (vCenter.x + (vPixelSize.x * 0.5f)), vSourceSize.x);
			int xYFrom = (int)Mathf.Max (Mathf.Floor (vCenter.y - (vPixelSize.y * 0.5f)), 0);
			int xYTo = (int)Mathf.Min (Mathf.Ceil (vCenter.y + (vPixelSize.y * 0.5f)), vSourceSize.y);
			             
			//*** Loop and accumulate
			Color oColorTemp = new Color ();
			float xGridCount = 0;
			for (int iy = xYFrom; iy < xYTo; iy++) {
				for (int ix = xXFrom; ix < xXTo; ix++) {
					                     
					//*** Get Color
					oColorTemp += aSourceColor [(int)(((float)iy * vSourceSize.x) + ix)];
					                     
					//*** Sum
					xGridCount++;
				}
			}
			             
			//*** Average Color
			aColor [ii] = oColorTemp / (float)xGridCount;
		}
		         
		//*** Set Pixels
		oNewTex.SetPixels (aColor);
		oNewTex.Apply ();
		         
		//*** Return
		return oNewTex;
	}

	public static Vector2 GetCenterCropedSize (float imageWidth, float imageHeight, float imageViewWidth, float imageViewHeight)
	{
		float viewAspectRatio = imageViewWidth / imageViewHeight;
		float imageAspectRatio = imageWidth / imageHeight;

		if (imageAspectRatio > viewAspectRatio) {
			return new Vector2 (imageWidth * imageViewWidth / imageWidth, imageViewHeight);
		} else {
			return new Vector2 (imageViewWidth, imageHeight * imageViewHeight / imageHeight);
		}
	}

	public static Vector2 GetCenterFittingSize (float imageWidth, float imageHeight, float imageViewWidth, float imageViewHeight)
	{
		float imageAspectRatio = imageWidth / imageHeight;
		float viewAspectRatio = imageViewWidth / imageViewHeight;

		if (imageAspectRatio > viewAspectRatio) {
			return new Vector2 (imageViewWidth, imageHeight * imageViewWidth / imageWidth);
		} else {
			return new Vector2 (imageWidth * imageViewHeight / imageHeight, imageViewHeight);
		}
	}

	public static float GetProportionalExpression (float a, float b, float a2)
	{
		return a2 * b / a;
	}

}