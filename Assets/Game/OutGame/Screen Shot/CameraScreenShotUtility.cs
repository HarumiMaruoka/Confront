using OdinSerializer;
using System;
using UnityEngine;

namespace Confront.Utility
{
    public static class CameraScreenShotUtility
    {
        public static byte[] CaptureScreenShot(this Camera camera)
        {
            var renderTexture = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 24);
            var previous = camera.targetTexture;

            camera.targetTexture = renderTexture;
            camera.Render(); // カメラの内容をtargetTextureに書きこむ。
            camera.targetTexture = previous;

            RenderTexture.active = renderTexture; // ピクセルデータを読み取れるようにする。
            var screenShot = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
            screenShot.Apply();

            return screenShot.EncodeToPNG();
        }
    }
}