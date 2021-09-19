//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="UriExtensions.cs" company="PicklesDoc">
//  Copyright 2011 Jeffrey Cameron
//  Copyright 2012-present PicklesDoc team and community contributors
//
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.IO.Abstractions;

namespace PicklesDoc.Pickles.Extensions
{
    public static class UriExtensions
    {
        private const string _fileSchema = "file://";
        private static readonly string _directorySeparator = Path.DirectorySeparatorChar.ToString();

        public static Uri ToUri(this IDirectoryInfo instance)
        {
            return instance.FullName.ToFolderUri();
        }

        public static Uri ToFileUriCombined(this IDirectoryInfo instance, string file, IFileSystem fileSystem)
        {
            string path = fileSystem.Path.Combine(instance.FullName, file);

            return path.ToFileUri();
        }

        public static Uri GetFileUri(this IDirectoryInfo instance, string file)
        {
            return Path.Combine(instance.FullName, file).ToFileUri();
        }

        public static Uri ToUri(this IFileSystemInfo instance)
        {
            var di = instance as IDirectoryInfo;

            if (di != null)
            {
                return ToUri(di);
            }

            return ToFileUri(instance.FullName);
        }

        public static Uri ToUri(this IFileInfo instance)
        {
            return ToFileUri(instance.FullName);
        }

        public static Uri ToFileUri(this string instance)
        {
            instance = AddFileSchema(instance);
            return new Uri(instance);
        }

        private static string AddFileSchema(string instance)
        {
            if (!instance.StartsWith(_fileSchema))
                instance = _fileSchema + instance;
            return instance;
        }

        public static Uri ToFolderUri(this string instance)
        {
            instance = AddFileSchema(instance);

            //Win-specific folder path
            if (instance.EndsWith("\\"))
                return new Uri(instance);

            if (!instance.EndsWith(_directorySeparator))
                instance = instance + _directorySeparator;

            return new Uri(instance);
        }

        public static string GetUriForTargetRelativeToMe(this Uri me, IFileSystemInfo target, string newExtension)
        {
            return target.FullName != me.LocalPath
                ? me.MakeRelativeUri(target.ToUri()).ToString().Replace(target.Extension, newExtension)
                : "#";
        }
    }
}
