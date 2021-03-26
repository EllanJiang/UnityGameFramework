//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.ResourceTools
{
    internal sealed partial class ResourceEditor : EditorWindow
    {
        private sealed class ResourceFolder
        {
            private static Texture s_CachedIcon = null;

            private readonly List<ResourceFolder> m_Folders;
            private readonly List<ResourceItem> m_Items;

            public ResourceFolder(string name, ResourceFolder folder)
            {
                m_Folders = new List<ResourceFolder>();
                m_Items = new List<ResourceItem>();

                Name = name;
                Folder = folder;
            }

            public string Name
            {
                get;
                private set;
            }

            public ResourceFolder Folder
            {
                get;
                private set;
            }

            public string FromRootPath
            {
                get
                {
                    return Folder == null ? string.Empty : (Folder.Folder == null ? Name : Utility.Text.Format("{0}/{1}", Folder.FromRootPath, Name));
                }
            }

            public int Depth
            {
                get
                {
                    return Folder != null ? Folder.Depth + 1 : 0;
                }
            }

            public static Texture Icon
            {
                get
                {
                    if (s_CachedIcon == null)
                    {
                        s_CachedIcon = AssetDatabase.GetCachedIcon("Assets");
                    }

                    return s_CachedIcon;
                }
            }

            public void Clear()
            {
                m_Folders.Clear();
                m_Items.Clear();
            }

            public ResourceFolder[] GetFolders()
            {
                return m_Folders.ToArray();
            }

            public ResourceFolder GetFolder(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("Resource folder name is invalid.");
                }

                foreach (ResourceFolder folder in m_Folders)
                {
                    if (folder.Name == name)
                    {
                        return folder;
                    }
                }

                return null;
            }

            public ResourceFolder AddFolder(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("Resource folder name is invalid.");
                }

                ResourceFolder folder = GetFolder(name);
                if (folder != null)
                {
                    throw new GameFrameworkException("Resource folder is already exist.");
                }

                folder = new ResourceFolder(name, this);
                m_Folders.Add(folder);

                return folder;
            }

            public ResourceItem[] GetItems()
            {
                return m_Items.ToArray();
            }

            public ResourceItem GetItem(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("Resource item name is invalid.");
                }

                foreach (ResourceItem item in m_Items)
                {
                    if (item.Name == name)
                    {
                        return item;
                    }
                }

                return null;
            }

            public void AddItem(string name, Resource resource)
            {
                ResourceItem item = GetItem(name);
                if (item != null)
                {
                    throw new GameFrameworkException("Resource item is already exist.");
                }

                item = new ResourceItem(name, resource, this);
                m_Items.Add(item);
                m_Items.Sort(ResourceItemComparer);
            }

            private int ResourceItemComparer(ResourceItem a, ResourceItem b)
            {
                return a.Name.CompareTo(b.Name);
            }
        }
    }
}
