//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StepDefinitions.cs" company="PicklesDoc">
//  Copyright 2017 Dmitry Grekov
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using PicklesDoc.Pickles.DocumentationBuilders.Cucumber;
using PicklesDoc.Pickles.Test;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles;
using System.IO;
using System.IO.Abstractions;
using PicklesDoc.Pickles.DirectoryCrawler;
using Autofac;
using NFluent;

namespace Pickles.DocumentationBuilders.Cucumber.UnitTests.AutomationLayer
{
    [Binding]
    [Scope(Tag = "cucumber")]
    public sealed class StepDefinitions : BaseFixture /* God object antipattern */
    {
        private Tree nodes;

        [Given(@"I have this feature description placed in a folder '(.*)' in a file '(.*)'")]
        public void GivenIHaveThisFeatureDescriptionPlacedInAFolder(string featureFolder, string featureFile, string multilineText)
        {
            var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName(featureFolder);
            directoryInfo.Create();
            var fileName = Path.Combine(featureFolder,featureFile);
            using var writer = FileSystem.FileInfo.FromFileName(fileName).CreateText();
            {
                writer.Write(multilineText);
                writer.Close();
            }

            var parser = new FileSystemBasedFeatureParser(new FeatureParser(Configuration),FileSystem);

            var feature = parser.Parse(fileName);

            this.nodes = new Tree(new FeatureNode(this.FileSystem.DirectoryInfo.FromDirectoryName(@"output"), string.Empty, feature));
        }
        [Given("I have this feature description")]
        public void IHaveThisFeatureDescription(string featureDescription)
        {
            FeatureParser parser = new FeatureParser(Configuration);

            var feature = parser.Parse(new StringReader(featureDescription));

            this.nodes = new Tree(new FeatureNode(this.FileSystem.DirectoryInfo.FromDirectoryName(@"output"), string.Empty, feature));
        }

        [When(@"I generate the documentation")]
        public void WhenIGenerateTheJsonDocumentation()
        {
            var configuration = this.Configuration;
            configuration.OutputFolder = this.FileSystem.GetOrCreateDirectory("output");
            var jsonDocumentationBuilder = this.Container.Resolve<CucumberDocumentationBuilder>();

            jsonDocumentationBuilder.Build(this.nodes);
        }

        [Then("the JSON file should contain")]
        public void ThenTheResultShouldBe(string expectedResult)
        {
            var actualResult = this.FileSystem.File.ReadAllText(@"output/cucumberResult.json");

            Check.That(actualResult).Contains(expectedResult);
        }
    }
}
