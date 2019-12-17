using UnityEngine;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Resource;
using System.IO;
using System.Xml;
using UnityEditor;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// AssetBundle 复制和转移
    /// </summary>
    public class AssetBundleTransfer : EditorWindow
    {
        private const string ConfigurationName = "GameMain/Configs/AssetBundleBuilder.xml";

        private static Dictionary<string, string> somethingNeedTransferAfterAssetBundleTransferDic;

        private static int InternalResourceVersion
        {
            get;
            set;
        }

        private static string OutputDirectory
        {
            get;
            set;
        }

        private static bool IsValidOutputDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(OutputDirectory))
                {
                    return false;
                }

                if (!Directory.Exists(OutputDirectory))
                {
                    return false;
                }

                return true;
            }
        }

        private ResourceMode displayResourceMode = ResourceMode.Package;

        private BuildTarget displayPlatform = BuildTarget.StandaloneWindows;

        [MenuItem("Game Framework/AssetBundle Tools/AssetBundle Transfer", false, 34)]
        private static void Open()
        {
            LoadBuilderConfig();
            AssetBundleTransfer window = GetWindow<AssetBundleTransfer>(true, "AssetBundle Transfer", true);
            window.minSize = window.maxSize = new Vector2(530f, 250f);
        }

        private static bool LoadBuilderConfig()
        {
            string configurationName = Utility.Path.GetCombinePath(Application.dataPath, ConfigurationName);
            if (!File.Exists(configurationName))
            {
                return false;
            }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(configurationName);
                XmlNode xmlRoot = xmlDocument.SelectSingleNode("UnityGameFramework");
                XmlNode xmlEditor = xmlRoot.SelectSingleNode("AssetBundleBuilder");
                XmlNode xmlSettings = xmlEditor.SelectSingleNode("Settings");

                XmlNodeList xmlNodeList = null;
                XmlNode xmlNode = null;

                xmlNodeList = xmlSettings.ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    switch (xmlNode.Name)
                    {
                        case "InternalResourceVersion":
                            InternalResourceVersion = int.Parse(xmlNode.InnerText);
                            break;
                        case "OutputDirectory":
                            OutputDirectory = xmlNode.InnerText;
                            break;
                    }
                }
            }
            catch
            {
                File.Delete(configurationName);
                return false;
            }

            return true;
        }

        public static void TransferAssetBundlesToStreamingAssets(string outputPath, ResourceMode resourceMode, BuildTarget platform, string resourceVersion)
        {
            OutputDirectory = outputPath;

            if (!IsValidOutputDirectory)
            {
                Debug.LogError("OutputDirectory is not valid!");
                return;
            }

            string srcPath = Utility.Path.GetCombinePath(OutputDirectory, resourceMode == ResourceMode.Updatable?"Packed":resourceMode.ToString(), resourceVersion, UnityUtility.EditorPath.GetBuildTargetName(platform));
            if (!Directory.Exists(srcPath))
            {
                Debug.LogError(srcPath + " not exist!");
                return;
            }

            ClearStreamingAssetFilesByPlatform(platform);

            string targetPath = Utility.Path.GetCombinePath(Application.streamingAssetsPath, UnityUtility.EditorPath.GetBuildTargetName(platform));
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            else
            {
                Debug.LogError(targetPath + " clear failure!");
                return;
            }

            Debug.Log("Begin copy assetbundle files from [" + srcPath + "] to [" + targetPath + "]");

            CopyDirectory(srcPath, targetPath);

            somethingNeedTransferAfterAssetBundleTransferDic = new Dictionary<string, string>();
            if (platform == BuildTarget.StandaloneWindows || platform == BuildTarget.StandaloneWindows64)
            {
                somethingNeedTransferAfterAssetBundleTransferDic.Add("GameMain/Resources/Configs/BuildInfo.txt", "");
                somethingNeedTransferAfterAssetBundleTransferDic.Add("GameMain/Lua", "Lua");
                somethingNeedTransferAfterAssetBundleTransferDic.Add("GameMain/LuaTables", "Lua");
                somethingNeedTransferAfterAssetBundleTransferDic.Add("LuaFramework/ToLua/Lua", "Lua");
            }
            else if(platform == BuildTarget.Android || platform == BuildTarget.iOS)
            {
                somethingNeedTransferAfterAssetBundleTransferDic.Add("GameMain/Logos/login.mp4", "videos");
            }

            foreach (KeyValuePair<string, string> kvp in somethingNeedTransferAfterAssetBundleTransferDic)
            {
                string path = Utility.Path.GetCombinePath(Application.dataPath, kvp.Key);
                if (File.GetAttributes(path).CompareTo(FileAttributes.Directory) == 0)
                {
                    //如果是文件夹
                    if (Directory.Exists(path))
                    {
                        CopyDirectory(path, Utility.Path.GetCombinePath(targetPath, kvp.Value), true, "", "*.lua");
                    }
                    else
                    {
                        Debug.Log("directory [" + path + "] not exsit");
                    }
                }
                else
                {
                    if (File.Exists(path))
                    {
                        string desPath = Utility.Path.GetCombinePath(targetPath, kvp.Value, System.IO.Path.GetFileName(path));
                        FileTool.CreatFilePath(desPath);
                        File.Copy(path, desPath, true);

                    }
                    else
                    {
                        Debug.Log("file [" + path + "] not exsit");
                    }
                }
            }

            AssetDatabase.Refresh();

            Debug.Log("End copy assetbundle files successfully!");
        }

        /// <summary>
        /// 清空StreamingAssets/目标平台路径
        /// </summary>
        /// <param name="platform">指定平台</param>
        private static void ClearStreamingAssetFilesByPlatform(BuildTarget platform)
        {
            string targetClearPath = Utility.Path.GetCombinePath(Application.streamingAssetsPath, UnityUtility.EditorPath.GetBuildTargetName(platform));

            if (Directory.Exists(targetClearPath))
            {
                Directory.Delete(targetClearPath, true);
            }
            Debug.Log(targetClearPath + " clear success!");
        }

        /// <summary>
        /// 拷贝目录文件
        /// </summary>
        /// <param name="sourceDir">源目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overwrite">是否覆盖</param>
        /// <param name="strAppendExt">文件追加的扩展名</param>
        /// <param name="searchPattern">文件通用匹配符</param>
        public static void CopyDirectory(string sourceDir, string targetDir, bool overwrite = true, string strAppendExt = "", string searchPattern = "*")
        {
            string[] files = Directory.GetFiles(sourceDir, searchPattern, SearchOption.AllDirectories);
            int len = sourceDir.Length;

            if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\')
            {
                --len;
            }

            for (int i = 0; i < files.Length; i++)
            {
                string str = files[i].Remove(0, len);
                string dest = targetDir + "/" + str;
                if (strAppendExt != "") dest += strAppendExt;
                string dir = System.IO.Path.GetDirectoryName(dest);
                Directory.CreateDirectory(dir);
                File.Copy(files[i], dest, overwrite);
            }
        }

        /// <summary>  
        /// 文件夹的复制  
        /// </summary>  
        /// <param sourceDir="string">要复制的原路径</param>  
        /// <param targetDir="string">要复制的目的路径</param>  
        public static void DirectoryCopy(string sourceDirPath, string targetDirPath, bool overwrite = true, string searchPattern = "*")
        {
            DirectoryInfo sourceDirInfo = new DirectoryInfo(sourceDirPath);
            try
            {
                if (!sourceDirInfo.Exists)//判断所指的文件或文件夹是否存在  
                {
                    return;
                }
                if (!Directory.Exists(targetDirPath))
                {
                    Directory.CreateDirectory(targetDirPath);
                }
                // 获取文件夹中所有文件和文件夹  
                FileSystemInfo[] sourceFiles = sourceDirInfo.GetFileSystemInfos(searchPattern);
                // 对单个FileSystemInfo进行判断,如果是文件夹则进行递归操作  
                foreach (FileSystemInfo sourceFileSys in sourceFiles)
                {
                    FileInfo file = sourceFileSys as FileInfo;
                    if (file != null)   // 如果是文件的话，进行文件的复制操作  
                    {
                        file.CopyTo(targetDirPath + "/" + file.Name, overwrite);          // 将文件复制到指定的路径中  
                    }
                    else
                    {
                        DirectoryCopy(sourceFileSys.FullName, System.IO.Path.Combine(targetDirPath, sourceFileSys.Name));
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("文件复制异常：" + ex.Message);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Transfer Params", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("AssetBundle SrcOutputPath", GUILayout.Width(160f));
                    if (IsValidOutputDirectory)
                    {
                        GUILayout.TextArea(string.Format("{0}", OutputDirectory));
                    }
                    else
                    {
                        GUILayout.Label("Please choose your assetbundle output directory first!");
                    }

                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("AssetBundle TargetPath", GUILayout.Width(160f));
                    GUILayout.TextArea(Application.streamingAssetsPath);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Internal Resource Version", GUILayout.Width(160f));
                    InternalResourceVersion = EditorGUILayout.IntField(InternalResourceVersion);
                }
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Resource Version", GUILayout.Width(160f));
                    GUILayout.Label(string.Format("{0}_{1}", "1.0".Replace('.', '_'), InternalResourceVersion.ToString()));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    displayResourceMode = (ResourceMode)EditorGUILayout.EnumPopup("Resource Mode", displayResourceMode);
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2f);

                EditorGUILayout.BeginHorizontal();
                {
                    displayPlatform = (BuildTarget)EditorGUILayout.EnumPopup("Platform", displayPlatform);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();


            GUILayout.Space(50f);

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginDisabledGroup(!IsValidOutputDirectory || displayResourceMode == ResourceMode.Unspecified);
                    {
                        if (GUILayout.Button("Start Transfer AssetBundles"))
                        {
                            TransferAssetBundlesToStreamingAssets(OutputDirectory, displayResourceMode, displayPlatform, string.Format("{0}_{1}", "1.0".Replace('.', '_'), InternalResourceVersion.ToString()));
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

        }


    }
}
