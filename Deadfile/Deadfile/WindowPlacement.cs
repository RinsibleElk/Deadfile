using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Xml;
using System.Xml.Serialization;

namespace Deadfile
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct DeadfileRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public DeadfileRect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct DeadfilePoint
    {
        public int X;
        public int Y;

        public DeadfilePoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Windowplacement
    {
        public int length;
        public int flags;
        public int showCmd;
        public DeadfilePoint minPosition;
        public DeadfilePoint maxPosition;
        public DeadfileRect normalPosition;
    }

    public static class WindowPlacement
    {
        private static readonly Encoding Encoding = new UTF8Encoding();
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Windowplacement));

        [DllImport("user32.dll")]
        private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref Windowplacement lpwndpl);

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, out Windowplacement lpwndpl);

        private const int SwShownormal = 1;
        private const int SwShowminimized = 2;

        public static void SetPlacement(this Window window, string placementXml)
        {
            WindowPlacement.SetPlacement(new WindowInteropHelper(window).Handle, placementXml);
        }

        public static string GetPlacement(this Window window)
        {
            return WindowPlacement.GetPlacement(new WindowInteropHelper(window).Handle);
        }

        public static void SetPlacement(IntPtr windowHandle, string placementXml)
        {
            if (string.IsNullOrEmpty(placementXml))
            {
                return;
            }

            byte[] xmlBytes = Encoding.GetBytes(placementXml);

            try
            {
                Windowplacement placement;
                using (var memoryStream = new MemoryStream(xmlBytes))
                {
                    placement = (Windowplacement)Serializer.Deserialize(memoryStream);
                }

                placement.length = Marshal.SizeOf(typeof(Windowplacement));
                placement.flags = 0;
                placement.showCmd = (placement.showCmd == SwShowminimized ? SwShownormal : placement.showCmd);
                SetWindowPlacement(windowHandle, ref placement);
            }
            catch (InvalidOperationException)
            {
                // Parsing placement XML failed. Fail silently.
            }
        }

        public static string GetPlacement(IntPtr windowHandle)
        {
            Windowplacement placement;
            GetWindowPlacement(windowHandle, out placement);

            using (var memoryStream = new MemoryStream())
            {
                using (var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                {
                    Serializer.Serialize(xmlTextWriter, placement);
                    var xmlBytes = memoryStream.ToArray();
                    return Encoding.GetString(xmlBytes);
                }
            }
        }
    }
}
