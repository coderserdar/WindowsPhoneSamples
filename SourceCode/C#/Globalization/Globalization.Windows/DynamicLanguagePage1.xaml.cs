using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Globalization
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DynamicLanguagePage : Page
    {
        public DynamicLanguagePage()
        {
            this.InitializeComponent();
        }

        private void CmboxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Changes the language strings based on language strings
            if (CmboxLanguage.SelectedIndex == 0)
            {
                // Display string  using en-US resources.
                ResourceContext ctx = new ResourceContext();
                ctx.Languages = new string[] { "en-US" };
                ResourceMap rmap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
                AppTitle.Text = rmap.GetValue("ApplicationTitleDyn", ctx).ValueAsString;
                txtDate.Text = rmap.GetValue("DateTextBlockDyn", ctx).ValueAsString;
                txtEmail.Text = rmap.GetValue("EmailIdTextblockDyn", ctx).ValueAsString;

            }
            else
            {
                // Display string  using es-MX resources.
                ResourceContext ctx = new ResourceContext();
                ctx.Languages = new string[] { "es-MX" };
                ResourceMap rmap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
                AppTitle.Text = rmap.GetValue("ApplicationTitleDyn", ctx).ValueAsString;
                txtDate.Text = rmap.GetValue("DateTextBlockDyn", ctx).ValueAsString;
                txtEmail.Text = rmap.GetValue("EmailIdTextblockDyn", ctx).ValueAsString;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

    }
}
