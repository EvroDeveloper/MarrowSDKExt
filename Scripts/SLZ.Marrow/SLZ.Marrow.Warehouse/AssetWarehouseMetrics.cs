 
 
using Unity.Profiling;
using UnityEngine;
 
 
#if UNITY_EDITOR
using Unity.Profiling.Editor;

#endif
namespace SLZ.Marrow.Warehouse
{
    public static class AssetWarehouseMetrics
    {
        public static readonly ProfilerCategory AssetWarehouseCategory = ProfilerCategory.Scripts;
        public const string LoadedScannableCountName = "Loaded Scannables";
        public static readonly ProfilerCounterValue<int> LoadedScannableCount = new ProfilerCounterValue<int>(AssetWarehouseCategory, LoadedScannableCountName, ProfilerMarkerDataUnit.Count);
        public const string LoadedPalletCountName = "Loaded Pallets";
        public static readonly ProfilerCounterValue<int> LoadedPalletCount = new ProfilerCounterValue<int>(AssetWarehouseCategory, LoadedPalletCountName, ProfilerMarkerDataUnit.Count);
        public const string LoadedCrateCountName = "Loaded Crates";
        public static readonly ProfilerCounterValue<int> LoadedCrateCount = new ProfilerCounterValue<int>(AssetWarehouseCategory, LoadedCrateCountName, ProfilerMarkerDataUnit.Count);
        public const string LoadedDataCardCountName = "Loaded DataCards";
        public static readonly ProfilerCounterValue<int> LoadedDataCardCount = new ProfilerCounterValue<int>(AssetWarehouseCategory, LoadedDataCardCountName, ProfilerMarkerDataUnit.Count);
        public const string LoadedMarrowAssetsCountName = "Loaded Marrow Assets";
        public static readonly ProfilerCounterValue<int> LoadedMarrowAssetsCount = new ProfilerCounterValue<int>(AssetWarehouseCategory, LoadedMarrowAssetsCountName, ProfilerMarkerDataUnit.Count);
        public static void Reset()
        {
            LoadedScannableCount.Value = 0;
            LoadedPalletCount.Value = 0;
            LoadedCrateCount.Value = 0;
            LoadedDataCardCount.Value = 0;
            LoadedMarrowAssetsCount.Value = 0;
        }

#if UNITY_EDITOR
        [System.Serializable]
        [ProfilerModuleMetadata("Asset Warehouse")]
        public class AssetWarehouseProfileModule : ProfilerModule
        {
            static readonly ProfilerCounterDescriptor[] counters = new ProfilerCounterDescriptor[]
            {
                new ProfilerCounterDescriptor(AssetWarehouseMetrics.LoadedScannableCountName, AssetWarehouseMetrics.AssetWarehouseCategory),
                new ProfilerCounterDescriptor(AssetWarehouseMetrics.LoadedPalletCountName, AssetWarehouseMetrics.AssetWarehouseCategory),
                new ProfilerCounterDescriptor(AssetWarehouseMetrics.LoadedCrateCountName, AssetWarehouseMetrics.AssetWarehouseCategory),
                new ProfilerCounterDescriptor(AssetWarehouseMetrics.LoadedDataCardCountName, AssetWarehouseMetrics.AssetWarehouseCategory),
                new ProfilerCounterDescriptor(AssetWarehouseMetrics.LoadedMarrowAssetsCountName, AssetWarehouseMetrics.AssetWarehouseCategory),
            };
            static readonly string[] autoEnabledCategoryNames = new string[]
            {
                ProfilerCategory.Scripts.Name
            };
            public AssetWarehouseProfileModule() : base(counters, autoEnabledCategoryNames: autoEnabledCategoryNames)
            {
            }
        }
#endif
    }
}