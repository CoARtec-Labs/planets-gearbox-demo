using UnityEngine;

namespace ICG
{
    public class ICGRgbPreview : MonoBehaviour
    {
        public Renderer targetRenderer;
        public bool flipY = true; // muss zu Calibrator/QuadFitter passen

        private Texture2D _tex;
        private byte[] _buffer;
        private byte[] _flipped;
        private int _w, _h, _c;

        [Tooltip("0 = unlimited")]
        public float maxFps = 30f;
        private float _nextUpdateTime = 0f;

        void Update()
        {
            if (maxFps > 0f)
            {
                if (Time.unscaledTime < _nextUpdateTime) return;
                _nextUpdateTime = Time.unscaledTime + 1f / maxFps;
            }

            if (!ICGNative.TryGetColorFrameInfo(out int w, out int h, out int c, out double ts))
                return;

            if (w <= 0 || h <= 0 || c < 3)
                return;

            // (Re)allocate texture/buffer when dimensions change
            if (_tex == null || w != _w || h != _h)
            {
                _w = w; _h = h; _c = c;

                _tex = new Texture2D(_w, _h, TextureFormat.RGB24, false, false);
                _tex.wrapMode = TextureWrapMode.Clamp;
                _tex.filterMode = FilterMode.Bilinear;

                _buffer = new byte[_w * _h * 3];
                _flipped = new byte[_buffer.Length];

                if (targetRenderer != null && targetRenderer.sharedMaterial != null)
                {
                    // Background / möglichst früh
                    targetRenderer.sharedMaterial.renderQueue = 1000;
                    targetRenderer.sharedMaterial.SetTexture("_MainTex", _tex);
                }
            }

            int copied = ICGNative.CopyColorFrameRGB(_buffer);
            if (copied <= 0) return;

            if (flipY)
            {
                int rowBytes = _w * 3;
                for (int y = 0; y < _h; y++)
                {
                    System.Buffer.BlockCopy(_buffer, y * rowBytes, _flipped, (_h - 1 - y) * rowBytes, rowBytes);
                }
                _tex.LoadRawTextureData(_flipped);
            }
            else
            {
                _tex.LoadRawTextureData(_buffer);
            }

            _tex.Apply(false);
        }
    }
}
