using System.Collections;
using System.Collections.Generic;

namespace YooAsset.Editor
{
    public class TaskCreatePackage_BBP : IBuildTask
    {
        void IBuildTask.Run(BuildContext context)
        {
            BuildParametersContext buildParameters = context.GetContextObject<BuildParametersContext>();

            BuildMapContext buildMapContext = context.GetContextObject<BuildMapContext>();

            EBuildMode buildMode = buildParameters.Parameters.BuildMode;

            if (buildMode != EBuildMode.SimulateBuild && buildMode != EBuildMode.DryRunBuild)
            {
                CreatePackageCatalog(buildParameters, buildMapContext);
            }
        }

        /// <summary>
        /// 拷贝补丁文件到补丁包目录
        /// </summary>
        private void CreatePackageCatalog(BuildParametersContext buildParametersContext, BuildMapContext buildMapContext)
        {
            string pipelineOutputDirectory = buildParametersContext.GetPipelineOutputDirectory();//D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/OutputCache

            string packageOutputDirectory = buildParametersContext.GetPackageOutputDirectory();//D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/v1

            BuildLogger.Log($"Start making patch package: {packageOutputDirectory}");

            // 拷贝UnityManifest序列化文件
            {
                string sourcePath = $"{pipelineOutputDirectory}/{YooAssetSettings.OutputFolderName}"; //D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/OutputCache/OutputCache

                string destPath = $"{packageOutputDirectory}/{YooAssetSettings.OutputFolderName}"; //D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/V1/OutputCache

                EditorTools.CopyFile(sourcePath, destPath, true);
            }

            // 拷贝UnityManifest文本文件
            {
                string sourcePath = $"{pipelineOutputDirectory}/{YooAssetSettings.OutputFolderName}.manifest"; //D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/OutputCache/OutputCache.manifes

                string destPath = $"{packageOutputDirectory}/{YooAssetSettings.OutputFolderName}.manifest"; //D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/v1/OutputCache.manifest

                EditorTools.CopyFile(sourcePath, destPath, true);
            }

            // 拷贝所有补丁文件
            int progressValue = 0;

            int fileTotalCount = buildMapContext.Collection.Count;

            foreach (BuildBundleInfo bundleInfo in buildMapContext.Collection)
            {
                EditorTools.CopyFile(bundleInfo.PackageSourceFilePath, bundleInfo.PackageDestFilePath, true);

                EditorTools.DisplayProgressBar("Copy patch file", ++progressValue, fileTotalCount);
            }
            EditorTools.ClearProgressBar();
        }
    }
}