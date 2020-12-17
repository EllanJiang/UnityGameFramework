//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using GameFramework.Sound;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 播放声音失败事件。
    /// </summary>
    public sealed class PlaySoundFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 播放声音失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PlaySoundFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化播放声音失败事件的新实例。
        /// </summary>
        public PlaySoundFailureEventArgs()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            PlaySoundParams = null;
            BindingEntity = null;
            ErrorCode = 0;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取播放声音失败事件编号。
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
        /// 获取声音绑定的实体。
        /// </summary>
        public Entity BindingEntity
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误码。
        /// </summary>
        public PlaySoundErrorCode ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
        /// 创建播放声音失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的播放声音失败事件。</returns>
        public static PlaySoundFailureEventArgs Create(GameFramework.Sound.PlaySoundFailureEventArgs e)
        {
            PlaySoundInfo playSoundInfo = (PlaySoundInfo)e.UserData;
            PlaySoundFailureEventArgs playSoundFailureEventArgs = ReferencePool.Acquire<PlaySoundFailureEventArgs>();
            playSoundFailureEventArgs.SerialId = e.SerialId;
            playSoundFailureEventArgs.SoundAssetName = e.SoundAssetName;
            playSoundFailureEventArgs.SoundGroupName = e.SoundGroupName;
            playSoundFailureEventArgs.PlaySoundParams = e.PlaySoundParams;
            playSoundFailureEventArgs.BindingEntity = playSoundInfo.BindingEntity;
            playSoundFailureEventArgs.ErrorCode = e.ErrorCode;
            playSoundFailureEventArgs.ErrorMessage = e.ErrorMessage;
            playSoundFailureEventArgs.UserData = playSoundInfo.UserData;
            ReferencePool.Release(playSoundInfo);
            return playSoundFailureEventArgs;
        }

        /// <summary>
        /// 清理播放声音失败事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            SoundAssetName = null;
            SoundGroupName = null;
            PlaySoundParams = null;
            BindingEntity = null;
            ErrorCode = 0;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
