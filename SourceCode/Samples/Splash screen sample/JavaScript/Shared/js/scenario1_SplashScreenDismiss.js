﻿//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved

(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/html/scenario1_SplashScreenDismiss.html", {
        ready: function (element, options) {
            // Display whether the splash screen dismissal event was successfully received.
            document.getElementById("dismissalOutput").innerText = SdkSample.dismissed ? "Received the splash screen dismissal event." :
            "The application didn't receive the splash screen dismissal event.";
        }
    });
})();
