/*
 *    描述:
 *          1.
 *
 *    开发人: 邓平
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LtFramework.Util;

namespace LtFramework.ResKit
{
    public class MonoRes : MonoSingleton<MonoRes>
    {
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private bool _ClearAsset = false;
        List<int> ClearAssetList = new List<int>();
        public void ClearAsset(int assetHelperHash)
        {
            _ClearAsset = true;
            ClearAssetList.Add(assetHelperHash);
        }

        void Update()
        {
            if (_ClearAsset)
            {
                _ClearAsset = false;
                foreach (int hash in ClearAssetList)
                {
                    ResManager.Instance.ClearAssetCacheEnumerator(hash);
                }
            }
        }
    }
}
