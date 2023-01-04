using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FlipTileDemo.Resources;

namespace FlipTileDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        ShellTile primaryTile = ShellTile.ActiveTiles.FirstOrDefault();

protected override void OnNavigatedTo(NavigationEventArgs e)
{
    FlipTileData newTile = new FlipTileData()
    {
        Title = "Demo Tile",
        Count = 5,
        BackgroundImage = new Uri("/Assets/Tiles/FlowersSQ.png", UriKind.Relative),
        SmallBackgroundImage = new Uri("/Assets/Tiles/FlowersSQ.png", UriKind.Relative),
        WideBackgroundImage = new Uri("/Assets/Tiles/FlowersWide.png", UriKind.Relative),
        BackTitle = "The Back",
        BackContent = "Hello from the back",
        WideBackContent = "Hello from the back in wide screen",
        BackBackgroundImage = new Uri("/Assets/Tiles/NormalBack.png", UriKind.Relative),
        WideBackBackgroundImage = new Uri("/Assets/Tiles/WideBack.png", UriKind.Relative)
    };

    primaryTile.Update(newTile);
    base.OnNavigatedTo(e);
}
    }
}