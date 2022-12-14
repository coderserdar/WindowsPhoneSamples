//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved

(function () {
    "use strict";
    var page = WinJS.UI.Pages.define("/html/scenario1_SingleFile.html", {
        ready: function (element, options) {
            element.querySelector(".open").addEventListener("click", pickSinglePhoto, false);
            
            if (options && options.activationKind === Windows.ApplicationModel.Activation.ActivationKind.pickFileContinuation) {
                continueFileOpenPicker(options.activatedEventArgs);
            }
        }
    });

    function pickSinglePhoto() {
        // Clean scenario output
        WinJS.log && WinJS.log("", "sample", "status");

        // Create the picker object and set options
        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
        openPicker.viewMode = Windows.Storage.Pickers.PickerViewMode.thumbnail;
        openPicker.suggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.picturesLibrary;
        // Users expect to have a filtered view of their folders depending on the scenario.
        // For example, when choosing a documents folder, restrict the filetypes to documents for your application.
        openPicker.fileTypeFilter.replaceAll([".png", ".jpg", ".jpeg"]);

        // Open the picker for the user to pick a file
        openPicker.pickSingleFileAndContinue();
    }
    
    // Called when app is activated from file open picker
    // eventObject contains the returned files picked by user
    function continueFileOpenPicker(eventObject) {
        var files = eventObject[0].files;
        var filePicked = files.size > 0 ? files[0] : null;
        if (filePicked !== null) {
            // Application now has read/write access to the picked file
            WinJS.log && WinJS.log("Picked photo: " + filePicked.name, "sample", "status");
        } else {
            // The picker was dismissed with no selected file
            WinJS.log && WinJS.log("Operation cancelled.", "sample", "status");
        }
    }
})();
