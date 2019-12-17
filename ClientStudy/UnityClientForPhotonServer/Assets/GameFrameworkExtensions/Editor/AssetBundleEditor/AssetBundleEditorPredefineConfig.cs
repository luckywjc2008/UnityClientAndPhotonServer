using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// AssetBundle 编辑器扩展，支持按配置自动生成AssetBundleCollection.xml
    /// </summary>
    public class AssetBundleEditorPredefineConfig : EditorWindow
    {

        public const string savePath = "Assets/GameMain/Configs/AssetBundleEditorPredefineConfig.asset";

        private AssetBundleBuildConfig _config;
        private ReorderableList _list;
        private Vector2 _scrollPosition = Vector2.zero;
        private static AssetBundleEditorPredefineConfig window;

        [MenuItem("Game Framework/AssetBundle Tools/AssetBundle Editor PredefineConfig", false, 36)]
        static void Open()
        {
            window = GetWindow<AssetBundleEditorPredefineConfig>(true, "AssetBundleEditor PredefineConfig", true);
            window.minSize = new Vector2(1320f, 420f);
        }

        void OnGUI()
        {
            if (_config == null)
            {
                InitConfig();
            }

            if (_list == null)
            {
                InitFilterListDrawer();
            }

            //tool bar
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (GUILayout.Button("Add", EditorStyles.toolbarButton))
                {
                    Add();
                }
                if (GUILayout.Button("Save", EditorStyles.toolbarButton))
                {
                    Save();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(10);

                OnListElementLabelGUI(window.position);

            }
            GUILayout.EndHorizontal();

            //context
            GUILayout.BeginVertical();
            {
                GUILayout.Space(30);

                //Filter item list
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
                {
                    _list.DoLayoutList();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            //set dirty
            if (GUI.changed)
                EditorUtility.SetDirty(_config);

        }

        void InitConfig()
        {
            _config = LoadAssetAtPath<AssetBundleBuildConfig>(savePath);
            if (_config == null)
            {
                _config = ScriptableObject.CreateInstance<AssetBundleBuildConfig>();
            }
        }

        static T LoadAssetAtPath<T>(string path) where T : Object
        {
#if UNITY_5
            return AssetDatabase.LoadAssetAtPath<T>(path);
#else
			return (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
#endif
        }

        void InitFilterListDrawer()
        {
            _list = new ReorderableList(_config.filters, typeof(AssetBundleFilter));
            _list.drawElementCallback = OnListElementGUI;
            _list.drawHeaderCallback = OnListHeaderGUI;
            _list.draggable = true;
            _list.elementHeight = 22;
            _list.onAddCallback = (list) => Add();
        }

        void Add()
        {
            string path = SelectFolder();
            if (!string.IsNullOrEmpty(path))
            {
                var filter = new AssetBundleFilter();
                filter.path = path;
                _config.filters.Add(filter);
            }
        }

        void OnListElementGUI(Rect rect, int index, bool isactive, bool isfocused)
        {
            const float GAP = 5;

            AssetBundleFilter filter = _config.filters[index];
            rect.y++;

            Rect r = rect;
            r.width = 16;
            r.height = 18;
            filter.valid = EditorGUI.Toggle(r, filter.valid);

            r.xMin = r.xMax + GAP;
            r.xMax = r.xMax + GAP + 420;
            float assetBundleNameLength = r.width;
            filter.assetBundleName = EditorGUI.TextField(r, filter.assetBundleName);

            r.xMin = r.xMax + GAP;
            r.xMax = r.xMin + GAP + 80;
            filter.assetBundleVariant = EditorGUI.TextField(r, filter.assetBundleVariant);

            r.xMin = r.xMax + GAP;
            r.xMax = r.xMin + GAP + 80;
            filter.assetBundleGroup = EditorGUI.TextField(r, filter.assetBundleGroup);

            r.xMin = r.xMax + GAP;
            r.width = assetBundleNameLength;
            GUI.enabled = false;
            filter.path = EditorGUI.TextField(r, filter.path);
            GUI.enabled = true;

            r.xMin = r.xMax + GAP;
            r.width = 50;
            if (GUI.Button(r, "Select"))
            {
                var path = SelectFolder();
                if (path != null)
                    filter.path = path;
            }

            r.xMin = r.xMax + GAP;
            r.xMax = rect.xMax;
            filter.filter = EditorGUI.TextField(r, filter.filter);
        }

        string SelectFolder()
        {
            string dataPath = Application.dataPath;
            string selectedPath = EditorUtility.OpenFolderPanel("Path", dataPath, "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                if (selectedPath.StartsWith(dataPath))
                {
                    return "Assets/" + selectedPath.Substring(dataPath.Length + 1);
                }
                else
                {
                    ShowNotification(new GUIContent("不能在Assets目录之外!"));
                }
            }
            return null;
        }

        void OnListHeaderGUI(Rect rect)
        {
            EditorGUI.LabelField(rect, "Asset Filter");
        }

        void OnListElementLabelGUI(Rect rect)
        {
            const float GAP = 5;
            GUI.enabled = false;

            Rect r = new Rect(0,20,rect.width,rect.height);
            r.width = 40;
            r.height = 18;
            EditorGUI.TextField(r, "Active");

            r.xMin = r.xMax + GAP;
            r.xMax = r.xMax + GAP + 420;
            float assetBundleNameLength = r.width;
            EditorGUI.TextField(r, "AssetBundleName");

            r.xMin = r.xMax + GAP;
            r.xMax = r.xMin + GAP + 120;
            EditorGUI.TextField(r, "AssetBundleVariant");

            r.xMin = r.xMax + GAP;
            r.xMax = r.xMin + GAP + 120;
            EditorGUI.TextField(r, "AssetBundleGroup");

            r.xMin = r.xMax + GAP;
            r.width = assetBundleNameLength;
            EditorGUI.TextField(r, "AssetDirectory");


            r.xMin = r.xMax + GAP;
            r.xMax = rect.xMax;
            EditorGUI.TextField(r, "AssetFilter");
            GUI.enabled = true;
        }

        void Save()
        {
            if (LoadAssetAtPath<AssetBundleBuildConfig>(savePath) == null)
            {
                AssetDatabase.CreateAsset(_config, savePath);
            }
            else
            {
                EditorUtility.SetDirty(_config);
            }
        }
    }
}
