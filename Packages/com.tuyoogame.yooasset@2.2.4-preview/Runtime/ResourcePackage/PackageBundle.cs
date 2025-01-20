using System;
using System.Linq;

namespace YooAsset
{
    [Serializable]
    internal class PackageBundle
    {
        /// <summary>
        /// 资源包名称
        /// </summary>
        public string BundleName; //firstpackage_assets_res_model_mat.bundle

        /// <summary>
        /// Unity引擎生成的CRC
        /// </summary>
        public uint UnityCRC;    //1295706721

        /// <summary>
        /// 文件哈希值
        /// </summary>
        public string FileHash;  //41edab5f73cd63214e760fb5f18ec25c

        /// <summary>
        /// 文件校验码
        /// </summary>
        public string FileCRC;   //501c3e05

        /// <summary>
        /// 文件大小（字节数）
        /// </summary>
        public long FileSize;   //42434

        /// <summary>
        /// 文件是否加密
        /// </summary>
        public bool Encrypted;  //false

        /// <summary>
        /// 资源包的分类标签
        /// </summary>
        public string[] Tags;

        /// <summary>
        /// 依赖的资源包ID集合
        /// </summary>
        public int[] DependIDs; //通过TaskBuilding_BBP步骤中生成的AssetBundleManifest文件获得依赖列表 再通过AB 名称_下标 缓存获取

        //----------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// 所属的包裹名称
        /// </summary>
        public string PackageName { private set; get; }     // FirstPackage

        /// <summary>
        /// 所属的构建管线
        /// </summary>
        public string BuildPipeline { private set; get; }   // BuiltinBuildPipeline

        /// <summary>
        /// 资源包GUID
        /// </summary>
        public string BundleGUID
        {
            get { return FileHash; }
        }     //41edab5f73cd63214e760fb5f18ec25c

        /// <summary>
        /// 文件名称
        /// </summary>
        private string _fileName;

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_fileName))
                    throw new Exception("Should never get here !");
                return _fileName;
            }
        }       //41edab5f73cd63214e760fb5f18ec25c.bundle

        /// <summary>
        /// 文件后缀名
        /// </summary>
        private string _fileExtension;

        public string FileExtension
        {
            get
            {
                if (string.IsNullOrEmpty(_fileExtension))
                    throw new Exception("Should never get here !");
                return _fileExtension;
            }
        }  //.bundle


        public PackageBundle()
        {
        }

        /// <summary>
        /// 解析资源包
        /// </summary>
        public void ParseBundle(PackageManifest manifest)
        {
            PackageName = manifest.PackageName;
            BuildPipeline = manifest.BuildPipeline;
            _fileExtension = ManifestTools.GetRemoteBundleFileExtension(BundleName);
            _fileName = ManifestTools.GetRemoteBundleFileName(manifest.OutputNameStyle, BundleName, _fileExtension, FileHash); 
        }

        /// <summary>
        /// 是否包含Tag
        /// </summary>
        public bool HasTag(string[] tags)
        {
            if (tags == null || tags.Length == 0)
                return false;
            if (Tags == null || Tags.Length == 0)
                return false;

            foreach (var tag in tags)
            {
                if (Tags.Contains(tag))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 是否包含任意Tags
        /// </summary>
        public bool HasAnyTags()
        {
            if (Tags != null && Tags.Length > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 检测资源包文件内容是否相同
        /// </summary>
        public bool Equals(PackageBundle otherBundle)
        {
            if (FileHash == otherBundle.FileHash)
                return true;

            return false;
        }
    }
}