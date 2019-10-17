//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using GameFramework.Sound;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 播放声音更新事件。
    /// </summary>
    public sealed class PlaySoundUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 播放声音更新事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PlaySoundUpdateEventArgs).GetHashCode();

        /// <summary>
        /// 初始化播放声音更新事件的新实例。
        /// </summary>
        public PlaySoundUpdateEventArgs()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            PlaySoundParams = null;
            Progress = 0f;
            BindingEntity = null;
            UserData = null;
        }

        /// <summary>
        /// 获取播放声音更新事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取声音的序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取声音资源名称。
        /// </summary>
        public string SoundAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取声音组名称。
        /// </summary>
        public string SoundGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取播放声音参数。
        /// </summary>
        public PlaySoundParams PlaySoundParams
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载声音进度。
        /// </summary>
        public float Progress
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取声音绑定的实体。
        /// </summary>
        public Entity BindingEntity
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建播放声音更新事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的播放声音更新事件。</returns>
        public static PlaySoundUpdateEventArgs Create(GameFramework.Sound.PlaySoundUpdateEventArgs e)
        {
            PlaySoundInfo playSoundInfo = (PlaySoundInfo)e.UserData;
            PlaySoundUpdateEventArgs playSoundUpdateEventArgs = ReferencePool.Acquire<PlaySoundUpdateEventArgs>();
            playSoundUpdateEventArgs.SerialId = e.SerialId;
            playSoundUpdateEventArgs.SoundAssetName = e.SoundAssetName;
            playSoundUpdateEventArgs.SoundGroupName = e.SoundGroupName;
            playSoundUpdateEventArgs.PlaySoundParams = e.PlaySoundParams;
            playSoundUpdateEventArgs.Progress = e.Progress;
            playSoundUpdateEventArgs.BindingEntity = playSoundInfo.BindingEntity;
            playSoundUpdateEventArgs.UserData = playSoundInfo.UserData;
            return playSoundUpdateEventArgs;
        }

        /// <summary>
        /// 清理播放声音更新事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            PlaySoundParams = null;
            Progress = 0f;
            BindingEntity = null;
            UserData = null;
        }
    }
}
