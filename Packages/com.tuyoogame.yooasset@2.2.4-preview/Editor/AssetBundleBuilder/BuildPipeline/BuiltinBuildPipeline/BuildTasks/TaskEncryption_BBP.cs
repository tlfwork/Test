
namespace YooAsset.Editor
{
    public class TaskEncryption_BBP : TaskEncryption, IBuildTask
    {
        void IBuildTask.Run(BuildContext context)
        {
            BuildParametersContext buildParameters = context.GetContextObject<BuildParametersContext>();

            BuildMapContext buildMapContext = context.GetContextObject<BuildMapContext>();

            EBuildMode buildMode = buildParameters.Parameters.BuildMode;

            if (buildMode == EBuildMode.ForceRebuild || buildMode == EBuildMode.IncrementalBuild)
            {
                EncryptingBundleFiles(buildParameters, buildMapContext);
            }
        }
    }
}