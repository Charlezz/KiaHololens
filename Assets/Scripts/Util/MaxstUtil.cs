using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class MaxstUtil : MonoBehaviour
{

	public static IEnumerator loadImageFromFile (string path, GameObject imageObject, System.Action complete)
	{
		WWW img_load = new WWW ("file://" + path);

		yield return img_load;

		Texture2D texture = (Texture2D)img_load.texture;
		Image img = imageObject.GetComponentInChildren<Image> ();
		img.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), img.GetComponent<RectTransform> ().pivot);
   
		if (complete != null) {
			complete ();
		}
	}

	public static IEnumerator loadRawImageFromFile (string path, GameObject rawImageObject, System.Action complete)
	{
		WWW img_load = new WWW ("file://" + path);

		yield return img_load;

		//var texture = img_load.texture;
		Texture2D imgTexture = new Texture2D (512, 512, TextureFormat.ETC2_RGB, false);  
		RawImage img = rawImageObject.GetComponentInChildren<RawImage> ();
		img_load.LoadImageIntoTexture (imgTexture);
		img.texture = imgTexture;
        
		if (complete != null) {
			complete ();
		}
	}

	public static IEnumerator loadRawImageFromWWW (string path, GameObject rawImageObject, System.Action complete)
	{
		WWW img_load = new WWW (path);

		yield return img_load;

        Texture2D imgTexture = img_load.textureNonReadable;
        RawImage img = rawImageObject.GetComponentInChildren<RawImage>();

        img.texture = imgTexture;

        img_load.Dispose();

		if (complete != null) {
			complete ();
		}
	}

	public static IEnumerator loadFastRawImageFromFileIEnumerator (string path, GameObject rawImageObject, System.Action complete)
	{
		Texture2D imgTexture = new Texture2D (2, 2);
		try {
			byte[] binaryImageData = File.ReadAllBytes (path);
			imgTexture.LoadImage (binaryImageData);
            binaryImageData = null;
		} catch {

		}

		RawImage img = rawImageObject.GetComponentInChildren<RawImage> ();
		img.texture = imgTexture;

		yield return null;

		if (complete != null) {
			complete ();
		}
	}

	public static void loadFastRawImageFromFile (string path, GameObject rawImageObject, System.Action complete)
	{
		//Texture2D imgTexture = new Texture2D(512, 512, TextureFormat.ETC2_RGB, false);  
		Texture2D imgTexture = new Texture2D (2, 2);
		try {
			byte[] binaryImageData = File.ReadAllBytes (path);
			imgTexture.LoadImage (binaryImageData);
            binaryImageData = null;
		} catch {
            
		}
       
		RawImage img = rawImageObject.GetComponentInChildren<RawImage> ();
		img.texture = imgTexture;
        
		//TextureResourceController.Instance.pushTexture( imgTexture );
        
		if (complete != null) {
			complete ();
		}
	}

	public static IEnumerator loadFastRawImageFromFileWithSizeIEnumerator (string path, GameObject rawImageObject, System.Action<int, int> complete)
	{
		Texture2D imgTexture = new Texture2D (2, 2);
		try {
			byte[] binaryImageData = File.ReadAllBytes (path);
			imgTexture.LoadImage (binaryImageData);
            binaryImageData = null;
		} catch {

		}
		if (rawImageObject != null) {
			RawImage img = rawImageObject.GetComponentInChildren<RawImage> ();
			img.texture = imgTexture;
		}

		yield return null;
		if (complete != null) {
			complete (imgTexture.width, imgTexture.height);
		}
	}

    public static IEnumerator loadFastRawImageFromFileWithSizeIEnumeratorSecond( string path, GameObject rawImageObject, System.Action<int, int> complete )
    {
        WWW img_load = new WWW("file://" + path);

        yield return img_load;

        //var texture = img_load.texture;
        Texture2D imgTexture = img_load.textureNonReadable;
        RawImage img = rawImageObject.GetComponentInChildren<RawImage>();

        img.texture = imgTexture;

        img_load.Dispose();

        if( complete != null )
        {
            complete(imgTexture.width, imgTexture.height);
        }
    }


	public static void loadFastRawImageFromFileWithSize (string path, GameObject rawImageObject, System.Action<int, int> complete)
	{
		Texture2D imgTexture = new Texture2D (2, 2);
		try {
			byte[] binaryImageData = File.ReadAllBytes (path);
			imgTexture.LoadImage (binaryImageData);
            binaryImageData = null;
		} catch {

		}
		if (rawImageObject != null) {
			RawImage img = rawImageObject.GetComponentInChildren<RawImage> ();
			img.texture = imgTexture;
		}

		if (complete != null) {
			complete (imgTexture.width, imgTexture.height);
		}
	}




	public const int TYPE_WIDTH = 0;
	public const int TYPE_HEIGHT = 1;

	public static float GetPixelFromInch (int type, float inch)
	{
		switch (type) {
		case TYPE_WIDTH:
			float wScale = (float)Screen.width / (float)Screen.currentResolution.width;
			return inch * Screen.dpi / (wScale) * (1920f / Screen.currentResolution.width);
		case TYPE_HEIGHT:
			float hScale = (float)Screen.height / (float)Screen.currentResolution.height;
			return inch * Screen.dpi / (hScale) * (1920f / Screen.currentResolution.width);
		default:
			return inch;
		}
	}
}
