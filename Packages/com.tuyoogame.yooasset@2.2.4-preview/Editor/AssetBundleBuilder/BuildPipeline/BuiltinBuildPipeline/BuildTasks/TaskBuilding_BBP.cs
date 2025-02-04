﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YooAsset.Editor
{
    public class TaskBuilding_BBP : IBuildTask
    {
        public class BuildResultContext : IContextObject
        {
            public AssetBundleManifest UnityManifest;
        }

        void IBuildTask.Run(BuildContext context)
        {
            BuildParametersContext buildParametersContext = context.GetContextObject<BuildParametersContext>();

            BuildMapContext buildMapContext = context.GetContextObject<BuildMapContext>();

            BuiltinBuildParameters builtinBuildParameters = buildParametersContext.Parameters as BuiltinBuildParameters;

            // 模拟构建模式下跳过引擎构建
            EBuildMode buildMode = buildParametersContext.Parameters.BuildMode;

            if (buildMode == EBuildMode.SimulateBuild)

                return;

            // 开始构建
            string pipelineOutputDirectory = buildParametersContext.GetPipelineOutputDirectory();//D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/OutputCache

            BuildAssetBundleOptions buildOptions = builtinBuildParameters.GetBundleBuildOptions();

            AssetBundleManifest unityManifest = BuildPipeline.BuildAssetBundles(pipelineOutputDirectory, buildMapContext.GetPipelineBuilds(), buildOptions, buildParametersContext.Parameters.BuildTarget);
            
            if (unityManifest == null)
            {
                string message = BuildLogger.GetErrorMessage(ErrorCode.UnityEngineBuildFailed, "UnityEngine build failed !");

                throw new Exception(message);
            }

            if (buildMode == EBuildMode.ForceRebuild || buildMode == EBuildMode.IncrementalBuild)
            {
                //D:/unitylearn/Test/Bundles/StandaloneWindows64/FirstPackage/OutputCache/OutputCache
                string unityOutputManifestFilePath = $"{pipelineOutputDirectory}/{YooAssetSettings.OutputFolderName}";

                if (!System.IO.File.Exists(unityOutputManifestFilePath))
                {
                    string message = BuildLogger.GetErrorMessage(ErrorCode.UnityEngineBuildFatal, $"Not found output {nameof(AssetBundleManifest)} file : {unityOutputManifestFilePath}");
                    
                    throw new Exception(message);
                }
            }

            BuildLogger.Log("UnityEngine build success !");

            BuildResultContext buildResultContext = new BuildResultContext();

            buildResultContext.UnityManifest = unityManifest;

            context.SetContextObject(buildResultContext);
        }
    }
}