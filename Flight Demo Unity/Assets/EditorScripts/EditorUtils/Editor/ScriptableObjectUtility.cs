using UnityEngine;
using UnityEditor;
using System.IO;

namespace Protopia.EditorClasses.BuildUtilities
{
    public static class ScriptableObjectUtility
    {
        /// <summary>
        //	Creates a scriptable object asset of type T. If path is left as null, will use currently selected folder in AssetsFolder.
        /// </summary>
        /// <param name="prependNew"> Whether the created object should start with "New". </param>
        public static void CreateAsset<T>(bool prependNew, string path = null, string name = null) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            if (path == null)
            {
                path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (path == "")
                {
                    path = "Assets";
                }
                else if (Path.GetExtension(path) != "")
                {
                    path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
                }
            }

            string fileName = (name != null ? name: typeof(T).ToString());

            //Cut out duplicate ".asset" format name.
            if (fileName.EndsWith(".asset")) 
            {
                fileName = fileName.Substring(0, fileName.Length - 6);
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + (prependNew ? "/New " : "") + fileName + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}