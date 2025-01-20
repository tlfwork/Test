#if UNITY_2019_4_OR_NEWER
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace YooAsset.Editor
{
    internal class BuiltinBuildPipelineViewer : BuildPipelineViewerBase
    {
        public BuiltinBuildPipelineViewer(string packageName, BuildTarget buildTarget, VisualElement parent)
            : base(packageName, EBuildPipeline.BuiltinBuildPipeline, buildTarget, parent)
        {
        }

        /// <summary>
        /// 执行构建
        /// </summary>
        protected override void ExecuteBuild()
        {
            var buildMode = AssetBundleBuilderSetting.GetPackageBuildMode(PackageName, BuildPipeline);
            var fileNameStyle = AssetBundleBuilderSetting.GetPackageFileNameStyle(PackageName, BuildPipeline);
            var buildinFileCopyOption = AssetBundleBuilderSetting.GetPackageBuildinFileCopyOption(PackageName, BuildPipeline);
            var buildinFileCopyParams = AssetBundleBuilderSetting.GetPackageBuildinFileCopyParams(PackageName, BuildPipeline);
            var compressOption = AssetBundleBuilderSetting.GetPackageCompressOption(PackageName, BuildPipeline);

            BuiltinBuildParameters buildParameters = new BuiltinBuildParameters();
            buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();//D:/unitylearn/Test/Bundles
            buildParameters.BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();//D:/unitylearn/Test/Assets/StreamingAssets/yoo/
            buildParameters.BuildPipeline = BuildPipeline.ToString();//BuiltinBuildPipeline
            buildParameters.BuildTarget = BuildTarget;//StandaloneWindows64
            buildParameters.BuildMode = buildMode;//ForceRebuild
            buildParameters.PackageName = PackageName;//FirstPackage
            buildParameters.PackageVersion = GetPackageVersion();//v1
            buildParameters.EnableSharePackRule = true;
            buildParameters.VerifyBuildingResult = true;
            buildParameters.FileNameStyle = fileNameStyle;//HashName
            buildParameters.BuildinFileCopyOption = buildinFileCopyOption;//None
            buildParameters.BuildinFileCopyParams = buildinFileCopyParams;//""
            buildParameters.EncryptionServices = CreateEncryptionInstance();//null
            buildParameters.CompressOption = compressOption;//LZ4

            BuiltinBuildPipeline pipeline = new BuiltinBuildPipeline();

            BuildResult buildResult = pipeline.Run(buildParameters, true);

            if (buildResult.Success)
                EditorUtility.RevealInFinder(buildResult.OutputPackageDirectory);
        }

        protected override List<Enum> GetSupportBuildModes()
        {
            List<Enum> buildModeList = new List<Enum>();
            buildModeList.Add(EBuildMode.ForceRebuild);
            buildModeList.Add(EBuildMode.IncrementalBuild);
            buildModeList.Add(EBuildMode.DryRunBuild);
            buildModeList.Add(EBuildMode.SimulateBuild);
            return buildModeList;
        }
    }
}
#endif