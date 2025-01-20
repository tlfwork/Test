using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace YooAsset.Editor
{
    public abstract class TaskUpdateBundleInfo
    {
        public void UpdateBundleInfo(BuildContext context)
        {
            BuildParametersContext buildParametersContext = context.GetContextObject<BuildParametersContext>();

            BuildMapContext buildMapContext = context.GetContextObject<BuildMapContext>();

            string pipelineOutputDirectory = buildParametersContext.GetPipelineOutputDirectory();//D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/OutputCache

            string packageOutputDirectory = buildParametersContext.GetPackageOutputDirectory();//D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/v1

            int outputNameStyle = (int)buildParametersContext.Parameters.FileNameStyle;//HashName 0

            // 1.检测文件名长度
            foreach (BuildBundleInfo bundleInfo in buildMapContext.Collection)
            {
                // NOTE：检测文件名长度不要超过260字符。
                string fileName = bundleInfo.BundleName;

                if (fileName.Length >= 260)
                {
                    string message = BuildLogger.GetErrorMessage(ErrorCode.CharactersOverTheLimit, $"Bundle file name character count exceeds limit : {fileName}");

                    throw new Exception(message);
                }
            }

            // 2.更新构建输出的文件路径
            foreach (BuildBundleInfo bundleInfo in buildMapContext.Collection)
            {
                //D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/OutputCache/firstpackage_assets_res_model_mat.bundle
                bundleInfo.BuildOutputFilePath = $"{pipelineOutputDirectory}/{bundleInfo.BundleName}";

                if (bundleInfo.Encrypted)

                    bundleInfo.PackageSourceFilePath = bundleInfo.EncryptedFilePath;
                else
                    bundleInfo.PackageSourceFilePath = bundleInfo.BuildOutputFilePath;
            }

            // 3.更新文件其它信息
            foreach (var bundleInfo in buildMapContext.Collection)
            {
                bundleInfo.PackageUnityHash = GetUnityHash(bundleInfo, context);

                bundleInfo.PackageUnityCRC = GetUnityCRC(bundleInfo, context);

                bundleInfo.PackageFileHash = GetBundleFileHash(bundleInfo, buildParametersContext);

                bundleInfo.PackageFileCRC = GetBundleFileCRC(bundleInfo, buildParametersContext);

                bundleInfo.PackageFileSize = GetBundleFileSize(bundleInfo, buildParametersContext);
            }

            // 4.更新补丁包输出的文件路径
            foreach (var bundleInfo in buildMapContext.Collection)
            {
                string bundleName = bundleInfo.BundleName; //firstpackage_assets_res_model_mat.bundle

                string fileHash = bundleInfo.PackageFileHash; //41edab5f73cd63214e760fb5f18ec25c

                string fileExtension = ManifestTools.GetRemoteBundleFileExtension(bundleName);//.bundle

                string fileName = ManifestTools.GetRemoteBundleFileName(outputNameStyle, bundleName, fileExtension, fileHash);//41edab5f73cd63214e760fb5f18ec25c.bundle
                
                //D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/v1/41edab5f73cd63214e760fb5f18ec25c.bundle
                bundleInfo.PackageDestFilePath = $"{packageOutputDirectory}/{fileName}";
            }
        }
        protected abstract string GetUnityHash(BuildBundleInfo bundleInfo, BuildContext context);
        protected abstract uint GetUnityCRC(BuildBundleInfo bundleInfo, BuildContext context);
        protected abstract string GetBundleFileHash(BuildBundleInfo bundleInfo, BuildParametersContext buildParametersContext);
        protected abstract string GetBundleFileCRC(BuildBundleInfo bundleInfo, BuildParametersContext buildParametersContext);
        protected abstract long GetBundleFileSize(BuildBundleInfo bundleInfo, BuildParametersContext buildParametersContext);
        protected string GetFilePathTempHash(string filePath)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(filePath);
            return HashUtility.BytesMD5(bytes);

            // 注意：在文件路径的哈希值冲突的情况下，可以使用下面的方法
            //return $"{HashUtility.BytesMD5(bytes)}-{Guid.NewGuid():N}";
        }
        protected long GetBundleTempSize(BuildBundleInfo bundleInfo)
        {
            long tempSize = 0;

            var assetPaths = bundleInfo.GetAllMainAssetPaths();
            foreach (var assetPath in assetPaths)
            {
                long size = FileUtility.GetFileSize(assetPath);
                tempSize += size;
            }

            if (tempSize == 0)
            {
                string message = BuildLogger.GetErrorMessage(ErrorCode.BundleTempSizeIsZero, $"Bundle temp size is zero, check bundle main asset list : {bundleInfo.BundleName}");
                throw new Exception(message);
            }
            return tempSize;
        }
    }
}