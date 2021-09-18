//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FeatureParserTests.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class FeatureParserTests : BaseFixture
    {
        [Test]
        public void Parse_InvalidFeatureContent_ThrowsFeatureParseException()
        {
            var parser = new FeatureParser(Configuration);

            var reader = new System.IO.StringReader("Invalid feature file");

            Check.ThatCode(() => parser.Parse(reader)).Throws<FeatureParseException>()
                .WithMessage("Unable to parse feature");
        }

        [Test]
        public void Parse_WithUri_SetsUriInFeature()
        {
            var parser = new FeatureParser(Configuration);
            var featureText = @"
              Feature: Feature parser uri populating
                Scenario: External uri is presented
                  Given a feature with external uri
                  When I parse the feature and pass uri
                  Then parsed feature has this uri set in Uri property
            ";
            var reader = new System.IO.StringReader(featureText);
            var uri = "TestUri";
            var feature = parser.Parse(reader, uri);
            Check.That(feature.Uri).Equals(uri);
        }
    }
}
