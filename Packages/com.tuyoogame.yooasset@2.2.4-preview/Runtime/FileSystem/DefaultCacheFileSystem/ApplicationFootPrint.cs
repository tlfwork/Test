using System.IO;
using UnityEngine;

namespace YooAsset
{
    /// <summary>
    /// 应用程序水印
    /// </summary>
    internal class ApplicationFootPrint
    {
        private readonly DefaultCacheFileSystem _fileSystem;

        private string _footPrint;

        public ApplicationFootPrint(DefaultCacheFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// 读取应用程序水印
        /// </summary>
        public void Load(string packageName)
        {
            string footPrintFilePath = _fileSystem.GetSandboxAppFootPrintFilePath(); //D:/unitylearn/Test/yoo/FirstPackage/ManifestFiles/ApplicationFootPrint.bytes

            if (File.Exists(footPrintFilePath))
            {
                _footPrint = FileUtility.ReadAllText(footPrintFilePath);
            }
            else
            {
                Coverage(packageName);
            }
        }

        /// <summary>
        /// 检测水印是否发生变化
        /// </summary>
        public bool IsDirty()
        {
#if UNITY_EDITOR
            return _footPrint != Application.version;
#else
		    return _footPrint != Application.buildGUID;
#endif
        }

        /// <summary>
        /// 覆盖掉水印
        /// </summary>
        public void Coverage(string packageName)
        {
#if UNITY_EDITOR
            _footPrint = Application.version; //0.1
#else
			_footPrint = Application.buildGUID;
#endif
            string footPrintFilePath = _fileSystem.GetSandboxAppFootPrintFilePath(); //D:/unitylearn/Test/yoo/FirstPackage/ManifestFiles/ApplicationFootPrint.bytes

            FileUtility.WriteAllText(footPrintFilePath, _footPrint);

            YooLogger.Log($"Save application foot print : {_footPrint}");
        }
    }
}