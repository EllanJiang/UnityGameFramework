//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Config;
using GameFramework.Resource;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 配置组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Config")]
    public sealed class ConfigComponent : GameFrameworkComponent
    {
        private const int DefaultPriority = 0;

        private IConfigManager m_ConfigManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private bool m_EnableLoadConfigSuccessEvent = true;

        [SerializeField]
        private bool m_EnableLoadConfigFailureEvent = true;

        [SerializeField]
        private bool m_EnableLoadConfigUpdateEvent = false;

        [SerializeField]
        private bool m_EnableLoadConfigDependencyAssetEvent = false;

        [SerializeField]
        private string m_ConfigHelperTypeName = "UnityGameFramework.Runtime.DefaultConfigHelper";

        [SerializeField]
        private ConfigHelperBase m_CustomConfigHelper = null;

        /// <summary>
        /// 获取配置数量。
        /// </summary>
        public int ConfigCount
        {
            get
            {
                return m_ConfigManager.ConfigCount;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_ConfigManager = GameFrameworkEntry.GetModule<IConfigManager>();
            if (m_ConfigManager == null)
            {
                Log.Fatal("Config manager is invalid.");
                return;
            }

            m_ConfigManager.LoadConfigSuccess += OnLoadConfigSuccess;
            m_ConfigManager.LoadConfigFailure += OnLoadConfigFailure;
            m_ConfigManager.LoadConfigUpdate += OnLoadConfigUpdate;
            m_ConfigManager.LoadConfigDependencyAsset += OnLoadConfigDependencyAsset;
        }

        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
                m_ConfigManager.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                m_ConfigManager.SetResourceManager(GameFrameworkEntry.GetModule<IResourceManager>());
            }

            ConfigHelperBase configHelper = Helper.CreateHelper(m_ConfigHelperTypeName, m_CustomConfigHelper);
            if (configHelper == null)
            {
                Log.Error("Can not create config helper.");
                return;
            }

            configHelper.name = string.Format("Config Helper");
            Transform transform = configHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_ConfigManager.SetConfigHelper(configHelper);
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configName">配置名称。</param>
        /// <param name="configAssetName">配置资源名称。</param>
        public void LoadConfig(string configName, string configAssetName)
        {
            LoadConfig(configName, configAssetName, DefaultPriority, null);
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configName">配置名称。</param>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="priority">加载配置资源的优先级。</param>
        public void LoadConfig(string configName, string configAssetName, int priority)
        {
            LoadConfig(configName, configAssetName, priority, null);
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configName">配置名称。</param>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadConfig(string configName, string configAssetName, object userData)
        {
            LoadConfig(configName, configAssetName, DefaultPriority, userData);
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configName">配置名称。</param>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="priority">加载配置资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadConfig(string configName, string configAssetName, int priority, object userData)
        {
            if (string.IsNullOrEmpty(configName))
            {
                Log.Error("Config name is invalid.");
                return;
            }

            m_ConfigManager.LoadConfig(configAssetName, priority, new LoadConfigInfo(configName, userData));
        }

        /// <summary>
        /// 解析配置。
        /// </summary>
        /// <param name="text">要解析的配置文本。</param>
        /// <returns>是否解析配置成功。</returns>
        public bool ParseConfig(string text)
        {
            return m_ConfigManager.ParseConfig(text);
        }

        /// <summary>
        /// 解析配置。
        /// </summary>
        /// <param name="text">要解析的配置文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析配置成功。</returns>
        public bool ParseConfig(string text, object userData)
        {
            return m_ConfigManager.ParseConfig(text, userData);
        }

        /// <summary>
        /// 检查是否存在指定配置项。
        /// </summary>
        /// <param name="configName">要检查配置项的名称。</param>
        /// <returns>指定的配置项是否存在。</returns>
        public bool HasConfig(string configName)
        {
            return m_ConfigManager.HasConfig(configName);
        }

        /// <summary>
        /// 移除指定配置项。
        /// </summary>
        /// <param name="configName">要移除配置项的名称。</param>
        public void RemoveConfig(string configName)
        {
            m_ConfigManager.RemoveConfig(configName);
        }

        /// <summary>
        /// 清空所有配置项。
        /// </summary>
        public void RemoveAllConfigs()
        {
            m_ConfigManager.RemoveAllConfigs();
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string configName)
        {
            return m_ConfigManager.GetBool(configName);
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string configName, bool defaultValue)
        {
            return m_ConfigManager.GetBool(configName, defaultValue);
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string configName)
        {
            return m_ConfigManager.GetInt(configName);
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string configName, int defaultValue)
        {
            return m_ConfigManager.GetInt(configName, defaultValue);
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string configName)
        {
            return m_ConfigManager.GetFloat(configName);
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string configName, float defaultValue)
        {
            return m_ConfigManager.GetFloat(configName, defaultValue);
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string configName)
        {
            return m_ConfigManager.GetString(configName);
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string configName, string defaultValue)
        {
            return m_ConfigManager.GetString(configName, defaultValue);
        }

        private void OnLoadConfigSuccess(object sender, GameFramework.Config.LoadConfigSuccessEventArgs e)
        {
            if (m_EnableLoadConfigSuccessEvent)
            {
                m_EventComponent.Fire(this, ReferencePool.Acquire<LoadConfigSuccessEventArgs>().Fill(e));
            }
        }

        private void OnLoadConfigFailure(object sender, GameFramework.Config.LoadConfigFailureEventArgs e)
        {
            Log.Warning("Load config failure, asset name '{0}', error message '{1}'.", e.ConfigAssetName, e.ErrorMessage);
            if (m_EnableLoadConfigFailureEvent)
            {
                m_EventComponent.Fire(this, ReferencePool.Acquire<LoadConfigFailureEventArgs>().Fill(e));
            }
        }

        private void OnLoadConfigUpdate(object sender, GameFramework.Config.LoadConfigUpdateEventArgs e)
        {
            if (m_EnableLoadConfigUpdateEvent)
            {
                m_EventComponent.Fire(this, ReferencePool.Acquire<LoadConfigUpdateEventArgs>().Fill(e));
            }
        }

        private void OnLoadConfigDependencyAsset(object sender, GameFramework.Config.LoadConfigDependencyAssetEventArgs e)
        {
            if (m_EnableLoadConfigDependencyAssetEvent)
            {
                m_EventComponent.Fire(this, ReferencePool.Acquire<LoadConfigDependencyAssetEventArgs>().Fill(e));
            }
        }
    }
}
