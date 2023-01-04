﻿//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

//
// UpdateAsync.xaml.h
// Declaration of the UpdateAsync class
//

#pragma once

#include "pch.h"
#include "Scenario8_UpdateAsync.g.h"
#include "MainPage.xaml.h"

namespace SDKSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [Windows::Foundation::Metadata::WebHostHidden]
    public ref class UpdateAsync sealed
    {
    public:
        UpdateAsync();

    protected:
        virtual void OnNavigatedTo(Windows::UI::Xaml::Navigation::NavigationEventArgs^ e) override;

    private:
        MainPage^ rootPage;
        void UpdateDefaultLogo_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
    };
}
