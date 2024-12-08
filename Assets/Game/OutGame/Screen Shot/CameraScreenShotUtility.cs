using OdinSerializer;
using System;
using UnityEngine;

namespace Confront.Utility
{
    public static class CameraScreenShotUtility
    {
        /// <summary>
        /// カメラからスクリーンショットを取得します。
        /// width, heightが指定されていない場合はカメラのピクセルサイズで取得します。
        /// width, heightが指定されている場合は、指定した解像度で取得します。
        /// </summary>
        /// <param name="camera">キャプチャ対象のカメラ</param>
        /// <param name="width">取得するスクリーンショットの幅（0以下の場合はカメラのpixelWidth）</param>
        /// <param name="height">取得するスクリーンショットの高さ（0以下の場合はカメラのpixelHeight）</param>
        /// <returns>PNGエンコード済みのバイト配列</returns>
        public static byte[] CaptureScreenShotJPG(this Camera camera, int quality = 75, int width = 0, int height = 0)
        {
            if (width <= 0) width = camera.pixelWidth;
            if (height <= 0) height = camera.pixelHeight;

            // 指定解像度でRenderTextureを準備
            var renderTexture = new RenderTexture(width, height, 24);
            var previous = camera.targetTexture;

            camera.targetTexture = renderTexture;
            camera.Render(); // カメラの内容をtargetTextureに書きこむ
            camera.targetTexture = previous;

            RenderTexture.active = renderTexture; // ピクセルデータ読み出し可能にする
            var screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenShot.Apply();

            RenderTexture.active = null;
            UnityEngine.Object.DestroyImmediate(renderTexture);

            return screenShot.EncodeToJPG(quality);
        }

        public static byte[] CaptureScreenShotPNG(this Camera camera, int width = 0, int height = 0)
        {
            if (width <= 0) width = camera.pixelWidth;
            if (height <= 0) height = camera.pixelHeight;

            // 指定解像度でRenderTextureを準備
            var renderTexture = new RenderTexture(width, height, 24);
            var previous = camera.targetTexture;

            camera.targetTexture = renderTexture;
            camera.Render(); // カメラの内容をtargetTextureに書きこむ
            camera.targetTexture = previous;

            RenderTexture.active = renderTexture; // ピクセルデータ読み出し可能にする
            var screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenShot.Apply();

            RenderTexture.active = null;
            UnityEngine.Object.DestroyImmediate(renderTexture);

            return screenShot.EncodeToPNG();
        }
    }
}
