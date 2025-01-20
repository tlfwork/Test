using System.Collections;
using System.Collections.Generic;

namespace YooAsset.Editor
{
    public class CollectAssetInfo
    {
        /// <summary>
        /// 收集器类型
        /// </summary>
        public ECollectorType CollectorType { private set; get; }//MainAssetCollector

        /// <summary>
        /// 资源包名称
        /// </summary>
        public string BundleName { private set; get; }//firstpackage_assets_res_model_mat.bundle     seperate firstpackage_assets_res_scene_scene_one.bundle

        /// <summary>
        /// 可寻址地址
        /// </summary>
        public string Address { private set; get; } //100

        /// <summary>
        /// 资源信息
        /// </summary>
        public AssetInfo AssetInfo { private set; get; }

        /// <summary>
        /// 资源分类标签
        /// </summary>
        public List<string> AssetTags { private set; get; }

        /// <summary>
        /// 依赖的资源列表
        /// </summary>
        public List<AssetInfo> DependAssets = new List<AssetInfo>();


        public CollectAssetInfo(ECollectorType collectorType, string bundleName, string address, AssetInfo assetInfo, List<string> assetTags)
        {
            CollectorType = collectorType;
            BundleName = bundleName;
            Address = address;
            AssetInfo = assetInfo;
            AssetTags = assetTags;
        }
    }
}