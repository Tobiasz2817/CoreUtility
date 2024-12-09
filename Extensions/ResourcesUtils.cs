using UnityEngine;
#if UNITY_RENDER_PIPELINE_URP
using UnityEngine.Rendering;
#endif

namespace CoreUtility.Extensions {
    public static class ResourcesUtils {
#if UNITY_RENDER_PIPELINE_URP
        /// <summary>
        /// Load volume profile from given path.
        /// </summary>
        /// <param name="path">Path from where volume profile should be loaded.</param>

        public static void LoadVolumeProfile(this Volume volume, string path) {
            var profile = Resources.Load<VolumeProfile>(path);
            volume.profile = profile;
        }
#endif
    }
}