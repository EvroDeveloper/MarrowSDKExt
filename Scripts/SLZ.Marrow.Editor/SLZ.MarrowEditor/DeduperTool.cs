using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace SLZ.MarrowEditor
{
    public class DeduperTool
    {
        private const string GENERATED_DEDUPE_GROUP_NAME = "Duplicate Asset Isolation";
        private const string DEDUPE_GROUP_NAME = "Deduped";
        public enum StripOptions
        {
            None,
            Models,
            PartialAnimations
        }

        public static AddressableAssetGroup Dedupe(string buildPath, string loadPath, StripOptions stripOptions = StripOptions.PartialAnimations)
        {
            AddressableAssetGroup dedupeGroup = null;
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            AddressableAssetSettings settings = AddressablesManager.Settings;
            dedupeGroup = settings.FindGroup(DEDUPE_GROUP_NAME);
            if (dedupeGroup != null)
            {
                settings.RemoveGroup(dedupeGroup);
            }

            dedupeGroup = settings.FindGroup(GENERATED_DEDUPE_GROUP_NAME);
            if (dedupeGroup != null)
            {
                settings.RemoveGroup(dedupeGroup);
            }

            var dedupeRule = new UnityEditor.AddressableAssets.Build.AnalyzeRules.CheckBundleDupeDependencies();
            dedupeRule.FixIssues(settings);
            dedupeGroup = settings.FindGroup(GENERATED_DEDUPE_GROUP_NAME);
            if (dedupeGroup != null)
            {
                BundledAssetGroupSchema schema = dedupeGroup.GetSchema<BundledAssetGroupSchema>();
                schema.BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackSeparately;
                schema.IncludeAddressInCatalog = false;
                AddressablesManager.SetCustomBuildPath(schema, buildPath);
                AddressablesManager.SetCustomLoadPath(schema, loadPath);
                var updateSchema = dedupeGroup.GetSchema<ContentUpdateGroupSchema>();
                updateSchema.StaticContent = false;
                EditorUtility.SetDirty(schema);
                AssetDatabase.SaveAssetIfDirty(schema);
                EditorUtility.SetDirty(updateSchema);
                AssetDatabase.SaveAssetIfDirty(updateSchema);
                dedupeGroup.Name = DEDUPE_GROUP_NAME;
                dedupeGroup.name = DEDUPE_GROUP_NAME;
                EditorUtility.SetDirty(dedupeGroup);
                AssetDatabase.SaveAssetIfDirty(dedupeGroup);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(dedupeGroup));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                var dedupeSchemas = dedupeGroup.Schemas.ToList();
                settings.groups.Remove(dedupeGroup);
                settings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupRemoved, dedupeGroup, true, true);
                RegenerateGUID(dedupeGroup, out var metaGuid);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                dedupeGroup = settings.FindGroup(DEDUPE_GROUP_NAME);
                AddressablesManager.GroupGuidField.SetValue(dedupeGroup, metaGuid);
                EditorUtility.SetDirty(dedupeGroup);
                AssetDatabase.SaveAssetIfDirty(dedupeGroup);
                dedupeGroup = settings.FindGroup(DEDUPE_GROUP_NAME);
                foreach (var dedupeSchema in dedupeSchemas)
                {
                    dedupeGroup.Schemas.Remove(dedupeSchema);
                    settings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupSchemaRemoved, dedupeSchema, true, true);
                }

                foreach (var dedupeSchema in dedupeSchemas)
                {
                    var loadedSchema = RegenerateGUID(dedupeSchema, out _, true);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    dedupeGroup.AddSchema(loadedSchema, true);
                }

                dedupeGroup.SetDirty(AddressableAssetSettings.ModificationEvent.GroupAdded, dedupeGroup, true, true);
                EditorUtility.SetDirty(dedupeGroup);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                T RegenerateGUID<T>(T assetObject, out string newGuid, bool reimport = false)
                    where T : UnityEngine.Object
                {
                    string assetPath = AssetDatabase.GetAssetPath(assetObject);
                    var md5 = System.Security.Cryptography.MD5.Create();
                    var hash = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(assetPath));
                    string metaGuid = new Guid(hash).ToString().Replace("-", "");
                    newGuid = metaGuid;
                    string metaPath = AssetDatabase.GetTextMetaFilePathFromAssetPath(assetPath);
                    string yamlContent = System.IO.File.ReadAllText(metaPath);
                    string replacedContent = Regex.Replace(yamlContent, @"guid:\s*(\w+)", "guid: " + metaGuid);
                    System.IO.File.WriteAllText(metaPath, replacedContent);
                    if (reimport)
                        AssetDatabase.ImportAsset(assetPath);
                    return AssetDatabase.LoadAssetAtPath<T>(assetPath);
                }

                EditorUtility.SetDirty(dedupeGroup);
                List<string> addedAddresses = new List<string>();
                foreach (var entry in dedupeGroup.entries)
                {
                    if (entry.MainAsset != null)
                    {
                        string name = entry.MainAsset.name;
                        if (entry.MainAsset is Shader)
                        {
                            string path = AssetDatabase.GetAssetPath(entry.MainAsset);
                            if (!string.IsNullOrEmpty(path))
                            {
                                name = System.IO.Path.GetFileNameWithoutExtension(path);
                            }
                            else
                            {
                                name = entry.MainAsset.name.Substring(name.LastIndexOf("/") + 1, name.Length - name.LastIndexOf("/") - 1);
                            }
                        }

                        string address = entry.MainAsset.GetType().Name + "/" + name;
                        if (addedAddresses.Contains(address))
                        {
                            address += "--" + entry.guid.Substring(0, 8);
                        }

                        entry.SetAddress(address);
                        addedAddresses.Add(address);
                    }
                    else
                    {
                        Debug.LogError("Deduper: BAD entry in dedupe group! " + entry.address);
                        entry.SetAddress("NULL/DELETEME");
                    }
                }

                Debug.Log("Deduper: Created Dedupe Group with " + dedupeGroup.entries.Count + " assets: " + string.Format("{0:hh\\:mm\\:ss}", timer.Elapsed));
                if (stripOptions != StripOptions.None)
                {
                    List<AddressableAssetEntry> strippers = new List<AddressableAssetEntry>();
                    foreach (var entry in dedupeGroup.entries)
                    {
                        bool strip = stripOptions.Equals(StripOptions.Models) && (entry.AssetPath.EndsWith(".fbx") || entry.AssetPath.EndsWith(".obj"));
                        if (stripOptions.Equals(StripOptions.PartialAnimations) && entry.AssetPath.Contains("@") && (entry.AssetPath.EndsWith(".fbx") || entry.AssetPath.EndsWith(".obj")))
                        {
                            strip = true;
                        }

                        if (strip)
                        {
                            strippers.Add(entry);
                        }
                    }

                    foreach (var entry in strippers)
                    {
                        dedupeGroup.RemoveAssetEntry(entry);
                    }

                    string strippedAssets = "";
                    foreach (var entry in strippers)
                    {
                        string extension = System.IO.Path.GetExtension(entry.AssetPath);
                        strippedAssets += "[" + entry.MainAssetType.Name + (!string.IsNullOrEmpty(extension) ? "/" + extension.Replace(".", "") : "") + "] " + entry.MainAsset.name + " (" + entry.AssetPath + ") \n";
                    }

                    Debug.Log("Deduper: Stripped " + strippers.Count + " Assets: \n" + strippedAssets);
                    if (strippers.Count > 0)
                        Debug.Log("Deduper: Stripped Dedupe Group now has " + dedupeGroup.entries.Count + " assets");
                }

                List<AddressableAssetEntry> strippers2 = new List<AddressableAssetEntry>();
                foreach (var entry in dedupeGroup.entries)
                {
                    bool strip = false;
                    if (entry.MainAsset.name.ToLower().StartsWith("litmas") || ((entry.MainAsset.name.ToLower().StartsWith("mas_m") || entry.MainAsset.name.ToLower().StartsWith("defaultdetail")) && entry.MainAssetType == typeof(Texture2D)))
                    {
                        strip = true;
                    }

                    if (strip)
                    {
                        strippers2.Add(entry);
                    }
                }

                foreach (var entry in strippers2)
                {
                    dedupeGroup.RemoveAssetEntry(entry);
                }

                string strippedAssets2 = "";
                foreach (var entry in strippers2)
                {
                    string extension = System.IO.Path.GetExtension(entry.AssetPath);
                    strippedAssets2 += "[" + entry.MainAssetType.Name + (!string.IsNullOrEmpty(extension) ? "/" + extension.Replace(".", "") : "") + "] " + entry.MainAsset.name + " (" + entry.AssetPath + ") \n";
                }

                Debug.Log("Deduper: Stripped " + strippers2.Count + " Assets: \n" + strippedAssets2);
                if (strippers2.Count > 0)
                    Debug.Log("Deduper: Stripped Dedupe Group now has " + dedupeGroup.entries.Count + " assets");

            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Deduper: Finished dedupe! Took " + string.Format("{0:hh\\:mm\\:ss}", timer.Elapsed));
            return dedupeGroup;
        }
    }
}
