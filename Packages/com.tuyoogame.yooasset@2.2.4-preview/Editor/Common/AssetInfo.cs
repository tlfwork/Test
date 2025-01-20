using System;
using System.Collections;
using System.Collections.Generic;

namespace YooAsset.Editor
{
    [Serializable]
    public class AssetInfo
    {
        private string _fileExtension = null;

        /// <summary>
        /// 资源路径
        /// </summary>
        public string AssetPath; //Assets/Res/Model/Mat/100.mat

        /// <summary>
        /// 资源GUID
        /// </summary>
        public string AssetGUID; //406ff9b5c08a68a4f8796ea0a6e2d4c3

        /// <summary>
        /// 资源类型
        /// </summary>
        public System.Type AssetType; //UnityEngine.Material

        /// <summary>
        /// 文件格式
        /// </summary>
        public string FileExtension //.mat
        {
            get
            {
                if (string.IsNullOrEmpty(_fileExtension))

                    _fileExtension = System.IO.Path.GetExtension(AssetPath);

                return _fileExtension;
            }
        }

        public AssetInfo(string assetPath)
        {
            AssetPath = assetPath;

            AssetGUID = UnityEditor.AssetDatabase.AssetPathToGUID(AssetPath);

            AssetType = UnityEditor.AssetDatabase.GetMainAssetTypeAtPath(AssetPath);

            // 注意：如果资源文件损坏或者实例化关联脚本丢失，获取的资源类型会无效！
            if (AssetType == null)
            {
                throw new Exception($"Found invalid asset : {AssetPath}");
            }
        }

        /// <summary>
        /// 是否为着色器资源
        /// </summary>
        public bool IsShaderAsset()
        {
            if (AssetType == typeof(UnityEngine.Shader) || AssetType == typeof(UnityEngine.ShaderVariantCollection))

                return true;
            else
                return false;
        }
    }
}