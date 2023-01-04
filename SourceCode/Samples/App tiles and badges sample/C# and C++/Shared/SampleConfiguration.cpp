﻿//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

#include "pch.h"
#include "MainPage.xaml.h"
#include "SampleConfiguration.h"

using namespace SDKSample;

using namespace Windows::UI::ViewManagement;

Platform::Array<Scenario>^ MainPage::scenariosInner = ref new Platform::Array<Scenario>
{
    { "Send tile notification with text", "SDKSample.Tiles.SendTextTile" },
    { "Send tile notification with local images", "SDKSample.Tiles.SendLocalImageTile" },
    { "Send tile notification with web images", "SDKSample.Tiles.SendWebImageTile" },
    { "Send badge notification", "SDKSample.Tiles.SendBadge" },
    { "Send push notifications from a Windows Azure Mobile Service", "SDKSample.Tiles.UsePushNotifications" },
    { "Enable notification queue and tags", "SDKSample.Tiles.EnableNotificationQueue" },
    { "Use notification expiration", "SDKSample.Tiles.NotificationExpiration" },
    { "Image protocols and baseUri", "SDKSample.Tiles.ImageProtocols" },
    { "Globalization, localization, scale, and accessibility", "SDKSample.Tiles.Globalization" },
    { "Content deduplication", "SDKSample.Tiles.ContentDeduplication" }
};


