using UnityEditor;

namespace CoreUtility {
    public static class FileHelper {
        /// <param name="coreFolder">Started path to research </param>
        /// <param name="folderName">The name of seraching folder</param>
        /// <returns>Return the full path of the folder</returns>
        public static string FindFolder(string coreFolder, string folderName) {
            var subFolders = AssetDatabase.GetSubFolders(coreFolder);

            foreach (var subFolder in subFolders) {
                if (subFolder.Contains(folderName)) 
                    return subFolder;
                
                var foundFolder = FindFolder(subFolder, folderName);
                if (!string.IsNullOrEmpty(foundFolder)) 
                    return foundFolder;
            }

            return string.Empty;
        }
    }
}