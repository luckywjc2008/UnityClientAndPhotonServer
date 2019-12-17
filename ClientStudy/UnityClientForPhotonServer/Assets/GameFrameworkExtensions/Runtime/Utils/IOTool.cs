using System.Text;
using System.IO;
using System;
using UnityEngine;

public class IOTool 
{
    public static string ReadStringByResource(string path)
    {
        path = FileTool.RemoveExpandName(path);
        TextAsset text = (TextAsset)Resources.Load(path);

        if (text == null)
        {
            return "";
        }
        else
        {
            return text.text;
        }
    }

    public static string ReadStringByFile(string path)
    {
        StringBuilder line = new StringBuilder();
        try
        {
            if (!File.Exists(path))
            {
                Debug.Log("Path not exists ! : " + path);
                return "";
            }

            StreamReader sr = File.OpenText(path);
            line.Append(sr.ReadToEnd());

            sr.Close();
            sr.Dispose();
        }
        catch (Exception e)
        {
            Debug.Log("Load text fail ! message:" + e.Message);
        }

        return line.ToString();
    }

    public static void WriteStringByFile(string path, string content)
    {
        byte[] dataByte = Encoding.GetEncoding("UTF-8").GetBytes(content);

        CreateFile(path, dataByte);
    }

    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            Debug.Log("File:[" + path + "] dont exists");
        }
    }

    public static void CreateFile(string path, byte[] byt)
    {
        try
        {
            FileTool.CreatFilePath(path);
            File.WriteAllBytes(path, byt);
        }
        catch (Exception e)
        {
            Debug.LogError("File Create Fail! \n" + e.Message);
        }
    }
}
