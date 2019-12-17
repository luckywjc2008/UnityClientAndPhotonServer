using System.Collections.Generic;
using System;
using GameFramework;
using UnityEngine;

/// <summary>
/// 配置管理器，只读
/// </summary>
public static class ConfigManager 
{
    public const string c_editorConfigDirectoryName = "GameFrameworkExtensions/Editor/Configs";
    public const string c_gameConfigDirectoryName = "GameMain/Resources/Configs";
    public const string c_expandName    = "json";

    /// <summary>
    /// 配置缓存
    /// </summary>
    static Dictionary<string, Dictionary<string, SingleField>> s_configCache = new Dictionary<string,Dictionary<string, SingleField>>();

    public static bool GetIsExistConfig(string ConfigName)
    {
        string dataJson = IOTool.ReadStringByResource(GetGameConfigFileRelPath(ConfigName));

        if (dataJson == "")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static Dictionary<string, SingleField> GetData(string ConfigName)
    {
        if (s_configCache.ContainsKey(ConfigName))
        {
            return s_configCache[ConfigName];
        }

        string dataJson = IOTool.ReadStringByResource(GetGameConfigFileRelPath(ConfigName));

        if (dataJson == "")
        {
            throw new Exception("ConfigManager.GetData() not find " + ConfigName);
        }
        else
        {
            Dictionary<string, SingleField> config = JsonTool.Json2Dictionary<SingleField>(dataJson);

            s_configCache.Add(ConfigName, config);
            return config;
        }
    }

    public static void CleanCache()
    {
        s_configCache.Clear();
    }

    /// <summary>
    /// 根据文件名 获取游戏配置的相对路径
    /// </summary>
    /// <param name="ConfigName">游戏配置文件名</param>
    /// <returns>相对Assets目录的路径</returns>
    public static string GetGameConfigFileRelPath(string ConfigName)
    {
        return Utility.Path.GetCombinePath("Configs", ConfigName + "." + c_expandName);
    }

    /// <summary>
    /// 根据文件名 获取游戏配置的全路径
    /// </summary>
    /// <param name="ConfigName">游戏配置文件名</param>
    /// <returns>全路径</returns>
    public static string GetGameConfigFileFullPath(string ConfigName)
    {
        return Utility.Path.GetCombinePath(Application.dataPath,c_gameConfigDirectoryName, ConfigName + "." + c_expandName);
    }

    /// <summary>
    /// 根据文件名 获取编辑器配置的相对路径
    /// </summary>
    /// <param name="ConfigName">编辑器配置文件名</param>
    /// <returns>相对Assets目录的路径</returns>
    public static string GetEditorConfigFileRelPath(string ConfigName)
    {
        return Utility.Path.GetCombinePath(c_editorConfigDirectoryName, ConfigName + "." + c_expandName);
    }

    /// <summary>
    /// 根据文件名 获取编辑器配置文件的全路径
    /// </summary>
    /// <param name="ConfigName">编辑器配置文件名</param>
    /// <returns>全路径</returns>
    public static string GetEditorConfigFileFullPath(string ConfigName)
    {
        return Utility.Path.GetCombinePath(Application.dataPath, c_editorConfigDirectoryName, ConfigName + "." + c_expandName);
    }
}
