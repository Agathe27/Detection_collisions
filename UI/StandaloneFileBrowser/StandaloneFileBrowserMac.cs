#if UNITY_STANDALONE_OSX

using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace SFB
{
    public class StandaloneFileBrowserMac : IStandaloneFileBrowser
    {
        private static Action<string[]> _openFileCb;
        private static Action<string[]> _openFolderCb;
        private static Action<string> _saveFileCb;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void AsyncCallback(string path);

        [AOT.MonoPInvokeCallback(typeof(AsyncCallback))]
        private static void openFileCb(string result)
        {
            _openFileCb?.Invoke(result.Split((char)28));
        }

        [AOT.MonoPInvokeCallback(typeof(AsyncCallback))]
        private static void openFolderCb(string result)
        {
            _openFolderCb?.Invoke(result.Split((char)28));
        }

        [AOT.MonoPInvokeCallback(typeof(AsyncCallback))]
        private static void saveFileCb(string result)
        {
            _saveFileCb?.Invoke(result);
        }

        // Uncomment these lines if you have the actual native library
        //[DllImport("StandaloneFileBrowser")]
        //private static extern IntPtr DialogOpenFilePanel(string title, string directory, string extension, bool multiselect);
        //[DllImport("StandaloneFileBrowser")]
        //private static extern void DialogOpenFilePanelAsync(string title, string directory, string extension, bool multiselect, AsyncCallback callback);
        //[DllImport("StandaloneFileBrowser")]
        //private static extern IntPtr DialogOpenFolderPanel(string title, string directory, bool multiselect);
        //[DllImport("StandaloneFileBrowser")]
        //private static extern void DialogOpenFolderPanelAsync(string title, string directory, bool multiselect, AsyncCallback callback);
        //[DllImport("StandaloneFileBrowser")]
        //private static extern IntPtr DialogSaveFilePanel(string title, string directory, string defaultName, string extension);
        //[DllImport("StandaloneFileBrowser")]
        //private static extern void DialogSaveFilePanelAsync(string title, string directory, string defaultName, string extension, AsyncCallback callback);

        public string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiselect)
        {
            // If native method is available, use it
            // Otherwise, use EditorUtility for a quick fix
#if UNITY_EDITOR
            string path = EditorUtility.OpenFilePanel(title, directory, GetFilterFromFileExtensionList(extensions));
            return string.IsNullOrEmpty(path) ? new string[0] : new string[] { path };
#else
            var ptr = DialogOpenFilePanel(
                title,
                directory,
                GetFilterFromFileExtensionList(extensions),
                multiselect);
            if (ptr == IntPtr.Zero) return new string[0];
            var paths = Marshal.PtrToStringAnsi(ptr);
            return paths.Split((char)28);
#endif
        }

        public void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, Action<string[]> cb)
        {
            _openFileCb = cb;
#if UNITY_EDITOR
            string path = EditorUtility.OpenFilePanel(title, directory, GetFilterFromFileExtensionList(extensions));
            _openFileCb?.Invoke(string.IsNullOrEmpty(path) ? new string[0] : new string[] { path });
#else
            DialogOpenFilePanelAsync(
                title,
                directory,
                GetFilterFromFileExtensionList(extensions),
                multiselect,
                openFileCb);
#endif
        }

        public string[] OpenFolderPanel(string title, string directory, bool multiselect)
        {
            // If native method is available, use it
            // Otherwise, use EditorUtility for a quick fix
#if UNITY_EDITOR
            string path = EditorUtility.OpenFolderPanel(title, directory, "");
            return string.IsNullOrEmpty(path) ? new string[0] : new string[] { path };
#else
            var ptr = DialogOpenFolderPanel(
                title,
                directory,
                multiselect);
            if (ptr == IntPtr.Zero) return new string[0];
            var paths = Marshal.PtrToStringAnsi(ptr);
            return paths.Split((char)28);
#endif
        }

        public void OpenFolderPanelAsync(string title, string directory, bool multiselect, Action<string[]> cb)
        {
            _openFolderCb = cb;
#if UNITY_EDITOR
            string path = EditorUtility.OpenFolderPanel(title, directory, "");
            _openFolderCb?.Invoke(string.IsNullOrEmpty(path) ? new string[0] : new string[] { path });
#else
            DialogOpenFolderPanelAsync(
                title,
                directory,
                multiselect,
                openFolderCb);
#endif
        }

        public string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions)
        {
#if UNITY_EDITOR
            return EditorUtility.SaveFilePanel(title, directory, defaultName, GetFilterFromFileExtensionList(extensions));
#else
            var ptr = DialogSaveFilePanel(
                title,
                directory,
                defaultName,
                GetFilterFromFileExtensionList(extensions));
            return Marshal.PtrToStringAnsi(ptr);
#endif
        }

        public void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> cb)
        {
            _saveFileCb = cb;
#if UNITY_EDITOR
            string path = EditorUtility.SaveFilePanel(title, directory, defaultName, GetFilterFromFileExtensionList(extensions));
            _saveFileCb?.Invoke(path);
#else
            DialogSaveFilePanelAsync(
                title,
                directory,
                defaultName,
                GetFilterFromFileExtensionList(extensions),
                saveFileCb);
#endif
        }

        private static string GetFilterFromFileExtensionList(ExtensionFilter[] extensions)
        {
            if (extensions == null)
            {
                return "";
            }

            var filterString = "";
            foreach (var filter in extensions)
            {
                filterString += filter.Name + ";";

                foreach (var ext in filter.Extensions)
                {
                    filterString += ext + ",";
                }

                filterString = filterString.Remove(filterString.Length - 1);
                filterString += "|";
            }
            filterString = filterString.Remove(filterString.Length - 1);
            return filterString;
        }
    }
}

#endif