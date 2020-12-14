using System;
using UnityEngine;

public static class Ln
{
    private static LnData lnData;
    private static LnData defaultLnData;
    private const string ErrPrefix = "LnErr";

    public static bool IsLoaded => Ln.lnData != null;

    public static bool DefaultIsLoaded => Ln.defaultLnData != null;

    public static void DefaultLoad(LnData defaulLnData) => Ln.defaultLnData = defaulLnData;

    public static void Load(LnData lnData) => Ln.lnData = lnData;

    public static Language GetCurrentLanguage() => Ln.lnData != null ? Ln.lnData.Lang : throw new NullReferenceException("[Ln] LnData Is Null");

    public static string Get(LnType lnType, string key) => Ln.Get(lnType.ToString().Replace("_", "/") + "/" + key);

    public static string Get(string key)
    {
        if (!Ln.IsLoaded)
        {
            Debug.LogWarning("[Ln] LnData is not loaded. key = " + key);
            return string.Empty;
        }
        string str1 = Ln.lnData.GetString(key);
        if (str1 != null && !string.IsNullOrEmpty(str1))
            return str1;
        Debug.LogWarning("[Ln] : key(" + key + ") is invalid.");
        if (!Ln.DefaultIsLoaded)
            Debug.LogWarning("[Ln] Default LnData is not loaded. key = " + key);
        string str2 = Ln.defaultLnData.GetString(key);
        if (str2 != null && !string.IsNullOrEmpty(str2))
            return str2;
        Debug.LogWarning("[Ln] : key(" + key + ") Default is invalid.");
        return Debug.isDebugBuild ? "LnErr[" + key + "]" : string.Empty;
    }

    public static string Format(string key, params object[] paramArray)
    {
        string format = Ln.Get(key);
        if (string.IsNullOrEmpty(format))
            return string.Empty;
        if (format.Contains("LnErr"))
            return format;
        string src = string.Empty;
        try
        {
            src = string.Format(format, paramArray);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[Ln] key(" + key + ") : Input string was not in a correct format.");
        }
        //if (Ln.lnData.Lang == Language.Korean && !string.IsNullOrEmpty(src))
            //src = Korean.ReplaceJosa(src);
        return src;
    }

    public static bool HasKey(string key) => Ln.lnData.GetString(key) != null;
}