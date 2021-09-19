﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FileSystemBasedFeatureParserTests.cs" company="PicklesDoc">
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
using System.IO.Abstractions.TestingHelpers;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class FileSystemBasedFeatureParserTests : BaseFixture
    {
        [Test]
        public void Parse_InvalidFeatureFile_ThrowsFeatureParseExceptionWithFilename()
        {
            var filePath =FileSystem.Path.Combine("temp","featurefile.feature");
            FileSystem.AddFile(filePath, new MockFileData("Invalid feature file"));
            var fileInfo=FileSystem.FileInfo.FromFileName(filePath);
            var parser = new FileSystemBasedFeatureParser(new FeatureParser(Configuration), FileSystem);

            Check.ThatCode(() => parser.Parse(filePath)).Throws<FeatureParseException>()
                .WithMessage(@"There was an error parsing the feature file here: "+ fileInfo.FullName +
                             Environment.NewLine + @"Errormessage was: 'Unable to parse feature'");
        }
        [Test]
        public void Parse_FeatureFile_Sets_Uri_from_file_path()
        {
            var featureText = @"
              Feature: Feature parser uri populating
                Scenario: External uri is presented
                  Given a feature with external uri
                  When I parse the feature and pass uri
                  Then parsed feature has this uri set in Uri property
            ";
            var featureFilePath =FileSystem.Path.Combine("temp","featurefile.feature");
            FileSystem.AddFile(featureFilePath, new MockFileData(featureText));
            var parser = new FileSystemBasedFeatureParser(new FeatureParser(Configuration), FileSystem);
            var feature = parser.Parse(featureFilePath);
            Check.That(feature.Uri).Equals(FileSystem.Path.GetFullPath(featureFilePath).ToFileUri());
            Check.That(feature.Root).Equals(FileSystem.Directory.GetCurrentDirectory().ToFolderUri());
        }

    }
}