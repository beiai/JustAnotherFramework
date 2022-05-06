using YooAsset;

namespace GameFramework.Module.UIForm
{
    /// <summary>
    /// 界面辅助器接口。
    /// </summary>
    public interface IUIFormHelper
    {
        /// <summary>
        /// 加载界面资源
        /// </summary>
        /// <param name="serialId">界面 ID。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroup">界面组名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadUIFormAsset(int serialId, string uiFormAssetName, UIGroup uiGroup, bool pauseCoveredUIForm,
            object userData);

        /// <summary>
        /// 获取界面资源
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns></returns>
        UIFormInstanceObject GetUIFormAsset(string uiFormAssetName);

        /// <summary>
        /// 实例化界面。
        /// </summary>
        /// <param name="uiFormAssetName"></param>
        /// <param name="handle"></param>
        /// <returns>实例化后的界面。</returns>
        object AddUIFormAsset(string uiFormAssetName, AssetOperationHandle handle);

        /// <summary>
        /// 创建界面。
        /// </summary>
        /// <param name="uiFormInstance">界面实例。</param>
        /// <param name="uiGroup">界面所属的界面组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面。</returns>
        IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData);

        /// <summary>
        /// 释放界面。
        /// </summary>
        /// <param name="uiFormInstance">要释放的界面实例。</param>
        void ReleaseUIForm(object uiFormInstance);
    }
}