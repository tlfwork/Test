using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YooAsset
{
    public sealed class UnloadUnusedAssetsOperation : AsyncOperationBase
    {
        private enum ESteps
        {
            None,
            UnloadUnused,
            Done,
        }

        private readonly ResourceManager _resManager;

        private ESteps _steps = ESteps.None;

        internal UnloadUnusedAssetsOperation(ResourceManager resourceManager)
        {
            _resManager = resourceManager;
        }

        internal override void InternalOnStart()
        {
            _steps = ESteps.UnloadUnused;
        }

        internal override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)

                return;

            if (_steps == ESteps.UnloadUnused)
            {
                Dictionary<string, LoadBundleFileOperation> loaderDic = _resManager._loaderDic;

                List<LoadBundleFileOperation> removeList = new List<LoadBundleFileOperation>(loaderDic.Count);

                // 注意：优先销毁资源提供者
                foreach (LoadBundleFileOperation loader in loaderDic.Values)
                {
                    loader.TryDestroyProviders();
                }

                // 获取销毁列表
                foreach (LoadBundleFileOperation loader in loaderDic.Values)
                {
                    if (loader.CanDestroyLoader())
                    {
                        removeList.Add(loader);
                    }
                }

                // 销毁文件加载器
                foreach (LoadBundleFileOperation loader in removeList)
                {
                    string bundleName = loader.BundleFileInfo.Bundle.BundleName;

                    loader.DestroyLoader();

                    _resManager._loaderDic.Remove(bundleName);
                }

                // 注意：释放未被引用的Asset资源
                Resources.UnloadUnusedAssets();

                _steps = ESteps.Done;

                Status = EOperationStatus.Succeed;
            }
        }

        internal override void InternalWaitForAsyncComplete()
        {
            while (true)
            {
                if (ExecuteWhileDone())
                {
                    _steps = ESteps.Done;

                    break;
                }
            }
        }
    }
}