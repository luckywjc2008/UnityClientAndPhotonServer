using UnityEngine;
using System.Collections;
using GameFramework;
using System.Collections.Generic;
using System.IO;
using System;

using System_IO_Path = System.IO.Path;

namespace UnityGameFramework.Runtime
{
    public static partial class UnityUtility
    {
        public static class Path
        {
            private static string _persistentDataPath;

            #region Java获取persistentDataPath以解决Unity获取路径为空的问题

            private static string[] _persistentDataPaths;

            private static bool IsDirectoryWritable(string path)
            {
                try
                {
                    if (!Directory.Exists(path)) return false;
                    string file = System_IO_Path.Combine(path, System_IO_Path.GetRandomFileName());
                    using (FileStream fs = File.Create(file, 1)) { }
                    File.Delete(file);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            private static string GetPersistentDataPath(params string[] components)
            {
                try
                {
                    string path = System_IO_Path.DirectorySeparatorChar + string.Join("" + System_IO_Path.DirectorySeparatorChar, components);
                    if (!Directory.GetParent(path).Exists) return null;
                    if (!Directory.Exists(path))
                    {
                        Log.Info("creating directory: " + path);
                        Directory.CreateDirectory(path);
                    }
                    if (!IsDirectoryWritable(path))
                    {
                        Log.Warning("persistent data path not writable: " + path);
                        return null;
                    }
                    return path;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    return null;
                }
            }

            private static string persistentDataPathInternal
            {
#if UNITY_ANDROID
        get
        {
            if (Application.isEditor || !Application.isPlaying) return Application.persistentDataPath;
            string path = null;
            if (string.IsNullOrEmpty(path)) path = GetPersistentDataPath("storage", "emulated", "0", "Android", "data", Application.bundleIdentifier, "files");
            if (string.IsNullOrEmpty(path)) path = GetPersistentDataPath("data", "data", Application.bundleIdentifier, "files");
            return path;
        }
#else
                get { return Application.persistentDataPath; }
#endif
            }

            private static string persistentDataPathExternal
            {
#if UNITY_ANDROID
        get
        {
            if (Application.isEditor || !Application.isPlaying) return null;
            string path = null;
            if (string.IsNullOrEmpty(path)) path = GetPersistentDataPath("storage", "sdcard0", "Android", "data", Application.bundleIdentifier, "files");
            if (string.IsNullOrEmpty(path)) path = GetPersistentDataPath("storage", "sdcard1", "Android", "data", Application.bundleIdentifier, "files");
            if (string.IsNullOrEmpty(path)) path = GetPersistentDataPath("mnt", "sdcard", "Android", "data", Application.bundleIdentifier, "files");
            return path;
        }
#else
                get { return null; }
#endif
            }

            private static string[] persistentDataPaths
            {
                get
                {
                    if (_persistentDataPaths == null)
                    {
                        List<string> paths = new List<string>();
                        if (!string.IsNullOrEmpty(Application.persistentDataPath) && !paths.Contains(Application.persistentDataPath)) paths.Add(Application.persistentDataPath);
                        if (!string.IsNullOrEmpty(persistentDataPathExternal)) paths.Add(persistentDataPathExternal);
                        if (!string.IsNullOrEmpty(persistentDataPathInternal)) paths.Add(persistentDataPathInternal);
                        _persistentDataPaths = paths.ToArray();
                    }
                    return _persistentDataPaths;
                }
            }

            // returns best persistent data path
            private static string persistentDataPath
            {
                get { return persistentDataPaths.Length > 0 ? persistentDataPaths[0] : null; }
            }

            private static string GetPersistentFile(string relativePath)
            {
                if (string.IsNullOrEmpty(relativePath)) return null;
                foreach (string path in persistentDataPaths)
                {
                    if (FileExists(path, relativePath)) return System_IO_Path.Combine(path, relativePath);
                }
                return null;
            }

            private static bool SaveData(string relativePath, byte[] data)
            {
                string path = GetPersistentFile(relativePath);
                if (string.IsNullOrEmpty(path))
                {
                    return SaveData(relativePath, data, 0);
                }
                else
                {
                    try
                    {
                        File.WriteAllBytes(path, data);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Log.Warning("couldn't save data to: " + path);
                        Debug.LogException(ex);
                        // try to delete file again
                        if (File.Exists(path)) File.Delete(path);
                        return SaveData(relativePath, data, 0);
                    }
                }
            }

            private static bool SaveData(string relativePath, byte[] data, int pathIndex)
            {
                if (pathIndex < persistentDataPaths.Length)
                {
                    string path = System_IO_Path.Combine(persistentDataPaths[pathIndex], relativePath);
                    try
                    {
                        string dir = System_IO_Path.GetDirectoryName(path);
                        if (!Directory.Exists(dir))
                        {
                            Log.Info("creating directory: " + dir);
                            Directory.CreateDirectory(dir);
                        }
                        File.WriteAllBytes(path, data);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Log.Warning("couldn't save data to: " + path);
                        Debug.LogException(ex);
                        if (File.Exists(path)) File.Delete(path);       // try to delete file again
                        return SaveData(relativePath, data, pathIndex + 1); // try next persistent path
                    }
                }
                else
                {
                    Log.Warning("couldn't save data to any persistent data path");
                    return false;
                }
            }

            private static bool FileExists(string path, string relativePath)
            {
                return Directory.Exists(path) && File.Exists(System_IO_Path.Combine(path, relativePath));
            }
            #endregion

            /// <summary>
            /// Unity3D各平台高级的持久化根路径
            /// </summary>
            public static string PersistentDataPath
            {
                get
                {
#if UNITY_ANDROID
        _persistentDataPath = persistentDataPath;
#elif UNITY_IOS
       _persistentDataPath = Application.persistentDataPath;
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        _persistentDataPath = Application.dataPath;
#else
                    _persistentDataPath = Application.persistentDataPath;
#endif
                    return _persistentDataPath;
                }
            }

           
        }

    }

}
