//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System.Collections.Generic;
using Windows.UI.Xaml.Controls;using System;

namespace SDKTemplate
{
    public partial class MainPage : Page
    {
        // Change the string below to reflect the name of your sample.
        // This is used on the main page as the title of the sample.
        public const string FEATURE_NAME = "Number Formatting Sample";

        // Change the array below to reflect the name of your scenarios.
        // This will be used to populate the list of scenarios on the main page with
        // which the user will choose the specific scenario that they are interested in.
        // These should be in the form: "Navigating to a web page".
        // The code in MainPage will take care of turning this into: "1) Navigating to a web page"
        List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario() { Title = "Percent and Permille Formatting", ClassType = typeof(NumberFormatting.PercentPermilleFormatting) },
            new Scenario() { Title = "Decimal Formatting", ClassType = typeof(NumberFormatting.DecimalFormatting) },
            new Scenario() { Title = "Currency Formatting", ClassType = typeof(NumberFormatting.CurrencyFormatting) },
            new Scenario() { Title = "Number Parsing", ClassType = typeof(NumberFormatting.NumberParsing) },
            new Scenario() { Title = "Rounding and Padding", ClassType = typeof(NumberFormatting.RoundingAndPadding) },
            new Scenario() { Title = "Numeral System Translation", ClassType = typeof(NumberFormatting.NumeralSystemTranslation) },
            new Scenario() { Title = "Formatting/Translation using Unicode Extensions", ClassType = typeof(NumberFormatting.UsingUnicodeExtensions) }
        };
    }

    public class Scenario
    {
        public string Title { get; set; }

        public Type ClassType { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
