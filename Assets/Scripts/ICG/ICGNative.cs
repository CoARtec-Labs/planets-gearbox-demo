using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ICG
{
    internal static class ICGNative
    {
        private const string DllName = "icg_unity_bridge";

        // ---------- Public API wrappers ----------
        public static void SetSequenceDirectory(string path)
        {
            using var s = new Utf8String(path);
            UnityTracker_SetSequenceDirectory(s.Ptr);
        }

        public static int Create(string metaDirectory)
        {
            using var s = new Utf8String(metaDirectory);
            return UnityTracker_Create(s.Ptr);
        }

        public static int AddBody(string bodyName)
        {
            using var s = new Utf8String(bodyName);
            return UnityTracker_AddBody(s.Ptr);
        }

        public static int Start() => UnityTracker_Start();

        public static void Stop() => UnityTracker_Stop();

        public static void Destroy() => UnityTracker_Destroy();

        public static bool TryGetPose(string bodyName, out float[] t_xyz, out float[] q_xyzw, out double timestamp)
        {
            t_xyz = new float[3];
            q_xyzw = new float[4];
            using var s = new Utf8String(bodyName);
            int ok = UnityTracker_GetPose(s.Ptr, t_xyz, q_xyzw, out timestamp);
            return ok != 0;
        }

        public static string GetLastError()
        {
            IntPtr p = UnityTracker_GetLastError();
            return PtrToStringUtf8(p) ?? "";
        }

        public static bool TryGetColorFrameInfo(out int w, out int h, out int c, out double ts)
        {
            return UnityTracker_GetColorFrameInfo(out w, out h, out c, out ts) != 0;
        }
        public static bool TryGetColorIntrinsics(out float fx, out float fy, out float cx, out float cy, out int w, out int h)
        {
            return UnityTracker_GetColorIntrinsics(out fx, out fy, out cx, out cy, out w, out h) != 0;
        }

        public static int CopyColorFrameRGB(byte[] dst)
        {
            if (dst == null || dst.Length == 0) return 0;
            return UnityTracker_CopyColorFrameRGB(dst, dst.Length);
        }

        // ---------- Native imports ----------
        [DllImport(DllName, EntryPoint = "UnityTracker_SetSequenceDirectory")]
        private static extern void UnityTracker_SetSequenceDirectory(IntPtr sequenceDir);

        [DllImport(DllName, EntryPoint = "UnityTracker_Create")]
        private static extern int UnityTracker_Create(IntPtr metaDirectory);

        [DllImport(DllName, EntryPoint = "UnityTracker_AddBody")]
        private static extern int UnityTracker_AddBody(IntPtr bodyName);

        [DllImport(DllName, EntryPoint = "UnityTracker_Start")]
        private static extern int UnityTracker_Start();

        [DllImport(DllName, EntryPoint = "UnityTracker_Stop")]
        private static extern void UnityTracker_Stop();

        [DllImport(DllName, EntryPoint = "UnityTracker_Destroy")]
        private static extern void UnityTracker_Destroy();
        
        [DllImport(DllName, EntryPoint = "UnityTracker_GetColorFrameInfo")]
        private static extern int UnityTracker_GetColorFrameInfo(out int width, out int height, out int channels, out double timestamp);

        [DllImport(DllName, EntryPoint = "UnityTracker_GetColorIntrinsics")]
                private static extern int UnityTracker_GetColorIntrinsics(out float fx, out float fy, out float cx, out float cy, out int width, out int height);

        [DllImport(DllName, EntryPoint = "UnityTracker_CopyColorFrameRGB")]
        private static extern int UnityTracker_CopyColorFrameRGB([Out] byte[] dst, int dstSize);

        [DllImport(DllName, EntryPoint = "UnityTracker_GetLastError")]
        private static extern IntPtr UnityTracker_GetLastError();

        [DllImport(DllName, EntryPoint = "UnityTracker_GetPose")]

        private static extern int UnityTracker_GetPose(
            IntPtr bodyName,
            [Out] float[] translation_xyz,
            [Out] float[] rotation_xyzw,
            out double timestamp);

        // ---------- UTF-8 helper ----------
        private sealed class Utf8String : IDisposable
        {
            public IntPtr Ptr { get; private set; }

            public Utf8String(string s)
            {
                if (s == null) s = "";
                byte[] bytes = Encoding.UTF8.GetBytes(s + "\0");
                Ptr = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, Ptr, bytes.Length);
            }

            public void Dispose()
            {
                if (Ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(Ptr);
                    Ptr = IntPtr.Zero;
                }
            }
        }

        private static string? PtrToStringUtf8(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) return null;

            // Find null-ter
            int len = 0;
            while (Marshal.ReadByte(ptr, len) != 0) len++;

            byte[] bytes = new byte[len];
            Marshal.Copy(ptr, bytes, 0, len);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
