using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace YooAsset.Editor
{
    public class TaskGetBuildMap_BBP : TaskGetBuildMap, IBuildTask
    {
        void IBuildTask.Run(BuildContext context)
        {
            BuildParametersContext buildParametersContext = context.GetContextObject<BuildParametersContext>();

            BuildMapContext buildMapContext = CreateBuildMap(buildParametersContext.Parameters);

            context.SetContextObject(buildMapContext);
        }
    }
}