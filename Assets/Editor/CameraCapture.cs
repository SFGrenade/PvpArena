using System.IO;
using UnityEngine;
using UnityEditor;
    
public class CameraCapture : MonoBehaviour
{
    protected static int fileCounter;
    
    [MenuItem("Camera/Render View")]
    static public void Capture()
    {
		Camera camera = GameObject.FindObjectOfType<Camera> ();

        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;
    
        camera.Render();
    
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;
    
        byte[] bytes = image.EncodeToPNG();
        Destroy(image);
    
        File.WriteAllBytes("E:\\Github_Projects\\!Exports\\TMP\\"+fileCounter+".png", bytes);
        fileCounter++;
    }
}
