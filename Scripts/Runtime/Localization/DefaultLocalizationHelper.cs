//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Localization;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认本地化辅助器。
    /// </summary>
    public class DefaultLocalizationHelper : LocalizationHelperBase
    {
        private static readonly string[] ColumnSplitSeparator = new string[] { "\t" };
        private static readonly string BytesAssetExtension = ".bytes";
        private const int ColumnCount = 4;

        private ResourceComponent m_ResourceComponent = null;

        /// <summary>
        /// 获取系统语言。
        /// </summary>
        public override Language SystemLanguage
        {
            get
            {
                switch (Application.systemLanguage)
                {
                    case UnityEngine.SystemLanguage.Afrikaans: return Language.Afrikaans;
                    case UnityEngine.SystemLanguage.Arabic: return Language.Arabic;
                    case UnityEngine.SystemLanguage.Basque: return Language.Basque;
                    case UnityEngine.SystemLanguage.Belarusian: return Language.Belarusian;
                    case UnityEngine.SystemLanguage.Bulgarian: return Language.Bulgarian;
                    case UnityEngine.SystemLanguage.Catalan: return Language.Catalan;
                    case UnityEngine.SystemLanguage.Chinese: return Language.ChineseSimplified;
                    case UnityEngine.SystemLanguage.ChineseSimplified: return Language.ChineseSimplified;
                    case UnityEngine.SystemLanguage.ChineseTraditional: return Language.ChineseTraditional;
                    case UnityEngine.SystemLanguage.Czech: return Language.Czech;
                    case UnityEngine.SystemLanguage.Danish: return Language.Danish;
                    case UnityEngine.SystemLanguage.Dutch: return Language.Dutch;
                    case UnityEngine.SystemLanguage.English: return Language.English;
                    case UnityEngine.SystemLanguage.Estonian: return Language.Estonian;
                    case UnityEngine.SystemLanguage.Faroese: return Language.Faroese;
                    case UnityEngine.SystemLanguage.Finnish: return Language.Finnish;
                    case UnityEngine.SystemLanguage.French: return Language.French;
                    case UnityEngine.SystemLanguage.German: return Language.German;
                    case UnityEngine.SystemLanguage.Greek: return Language.Greek;
                    case UnityEngine.SystemLanguage.Hebrew: return Language.Hebrew;
                    case UnityEngine.SystemLanguage.Hungarian: return Language.Hungarian;
                    case UnityEngine.SystemLanguage.Icelandic: return Language.Icelandic;
                    case UnityEngine.SystemLanguage.Indonesian: return Language.Indonesian;
                    case UnityEngine.SystemLanguage.Italian: return Language.Italian;
                    case UnityEngine.SystemLanguage.Japanese: return Language.Japanese;
                    case UnityEngine.SystemLanguage.Korean: return Language.Korean;
                    case UnityEngine.SystemLanguage.Latvian: return Language.Latvian;
                    case UnityEngine.SystemLanguage.Lithuanian: return Language.Lithuanian;
                    case UnityEngine.SystemLanguage.Norwegian: return Language.Norwegian;
                    case UnityEngine.SystemLanguage.Polish: return Language.Polish;
                    case UnityEngine.SystemLanguage.Portuguese: return Language.PortuguesePortugal;
                    case UnityEngine.SystemLanguage.Romanian: return Language.Romanian;
                    case UnityEngine.SystemLanguage.Russian: return Language.Russian;
                    case UnityEngine.SystemLanguage.SerboCroatian: return Language.SerboCroatian;
                    case UnityEngine.SystemLanguage.Slovak: return Language.Slovak;
                    case UnityEngine.SystemLanguage.Slovenian: return Language.Slovenian;
                    case UnityEngine.SystemLanguage.Spanish: return Language.Spanish;
                    case UnityEngine.SystemLanguage.Swedish: return Language.Swedish;
                    case UnityEngine.SystemLanguage.Thai: return Language.Thai;
                    case UnityEngine.SystemLanguage.Turkish: return Language.Turkish;
                    case UnityEngine.SystemLanguage.Ukrainian: return Language.Ukrainian;
                    case UnityEngine.SystemLanguage.Unknown: return Language.Unspecified;
                    case UnityEngine.SystemLanguage.Vietnamese: return Language.Vietnamese;
                    default: return Language.Unspecified;
                }
            }
        }

        /// <summary>
        /// 读取字典。
        /// </summary>
        /// <param name="localizationManager">本地化管理器。</param>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="dictionaryAsset">字典资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否读取字典成功。</returns>
        public override bool ReadData(ILocalizationManager localizationManager, string dictionaryAssetName, object dictionaryAsset, object userData)
        {
            TextAsset dictionaryTextAsset = dictionaryAsset as TextAsset;
            if (dictionaryTextAsset != null)
            {
                if (dictionaryAssetName.EndsWith(BytesAssetExtension, StringComparison.Ordinal))
                {
                    return localizationManager.ParseData(dictionaryTextAsset.bytes, userData);
                }
                else
                {
                    return localizationManager.ParseData(dictionaryTextAsset.text, userData);
                }
            }

            Log.Warning("Dictionary asset '{0}' is invalid.", dictionaryAssetName);
            return false;
        }

        /// <summary>
        /// 读取字典。
        /// </summary>
        /// <param name="localizationManager">本地化管理器。</param>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="dictionaryBytes">字典二进制流。</param>
        /// <param name="startIndex">字典二进制流的起始位置。</param>
        /// <param name="length">字典二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否读取字典成功。</returns>
        public override bool ReadData(ILocalizationManager localizationManager, string dictionaryAssetName, byte[] dictionaryBytes, int startIndex, int length, object userData)
        {
            if (dictionaryAssetName.EndsWith(BytesAssetExtension, StringComparison.Ordinal))
            {
                return localizationManager.ParseData(dictionaryBytes, startIndex, length, userData);
            }
            else
            {
                return localizationManager.ParseData(Utility.Converter.GetString(dictionaryBytes, startIndex, length), userData);
            }
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="localizationManager">本地化管理器。</param>
        /// <param name="dictionaryString">要解析的字典字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public override bool ParseData(ILocalizationManager localizationManager, string dictionaryString, object userData)
        {
            try
            {
                int position = 0;
                string dictionaryLineString = null;
                while ((dictionaryLineString = dictionaryString.ReadLine(ref position)) != null)
                {
                    if (dictionaryLineString[0] == '#')
                    {
                        continue;
                    }

                    string[] splitedLine = dictionaryLineString.Split(ColumnSplitSeparator, StringSplitOptions.None);
                    if (splitedLine.Length != ColumnCount)
                    {
                        Log.Warning("Can not parse dictionary line string '{0}' which column count is invalid.", dictionaryLineString);
                        return false;
                    }

                    string dictionaryKey = splitedLine[1];
                    string dictionaryValue = splitedLine[3];
                    if (!localizationManager.AddRawString(dictionaryKey, dictionaryValue))
                    {
                        Log.Warning("Can not add raw string with dictionary key '{0}' which may be invalid or duplicate.", dictionaryKey);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse dictionary string with exception '{0}'.", exception);
                return false;
            }
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="localizationManager">本地化管理器。</param>
        /// <param name="dictionaryBytes">要解析的字典二进制流。</param>
        /// <param name="startIndex">字典二进制流的起始位置。</param>
        /// <param name="length">字典二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public override bool ParseData(ILocalizationManager localizationManager, byte[] dictionaryBytes, int startIndex, int length, object userData)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(dictionaryBytes, startIndex, length, false))
                {
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                    {
                        while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                        {
                            string dictionaryKey = binaryReader.ReadString();
                            string dictionaryValue = binaryReader.ReadString();
                            if (!localizationManager.AddRawString(dictionaryKey, dictionaryValue))
                            {
                                Log.Warning("Can not add raw string with dictionary key '{0}' which may be invalid or duplicate.", dictionaryKey);
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse dictionary bytes with exception '{0}'.", exception);
                return false;
            }
        }

        /// <summary>
        /// 释放字典资源。
        /// </summary>
        /// <param name="localizationManager">本地化管理器。</param>
        /// <param name="dictionaryAsset">要释放的字典资源。</param>
        public override void ReleaseDataAsset(ILocalizationManager localizationManager, object dictionaryAsset)
        {
            m_ResourceComponent.UnloadAsset(dictionaryAsset);
        }

        private void Start()
        {
            m_ResourceComponent = GameEntry.GetComponent<ResourceComponent>();
            if (m_ResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
                return;
            }
        }
    }
}
