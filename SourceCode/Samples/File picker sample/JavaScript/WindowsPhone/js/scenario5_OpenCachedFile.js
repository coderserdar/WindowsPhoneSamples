//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved

(function () {
    "use strict";

    var fileToken;

    var page = WinJS.UI.Pages.define("/html/scenario5_OpenCachedFile.html", {
        ready: function (element, options) {
            element.querySelector(".pickFileButton").addEventListener("click", pickFile, false);
            element.querySelector(".outputFileButton").addEventListener("click", outputFile, false);

            if (options && options.activationKind === Windows.ApplicationModel.Activation.ActivationKind.pickFileContinuation) {
                continueFileOpenPicker(options.activatedEventArgs);
            }
        }
    });

    function pickFile() {
        // Clean scenario output
        WinJS.log && WinJS.log("", "sample", "status");

        // Create the picker object and set options
        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
        // Users expect to have a filtered view of their folders depending on the scenario.
        // For example, when choosing a documents folder, restrict the filetypes to documents for your application.
        openPicker.fileTypeFilter.replaceAll([".txt"]);

        // Open the picker for the user to pick a file
        openPicker.pickSingleFileAndContinue();
    }

    // Called when app is activated from file open picker
    // eventObject contains the returned files picked by user
    function continueFileOpenPicker(eventObject) {
        var files = eventObject[0].files;
        var filePicked = files.size > 0 ? files[0] : null;

        if (filePicked) {
            fileToken = Windows.Storage.AccessCache.StorageApplicationPermissions.futureAccessList.add(filePicked);
            outputFileButton.disabled = false;
            printFileAsync(filePicked);
        } else {
            // The picker was dismissed with no selected file
            WinJS.log && WinJS.log("Operation cancelled.", "sample", "status");
        }
    }

    function outputFile() {
        if (fileToken) {
            // Windows will call the server app to update the local version of the file
            Windows.Storage.AccessCache.StorageApplicationPermissions.futureAccessList.getFileAsync(fileToken).then(function (file) {
                printFileAsync(file);
            },
            function (error) {
                WinJS.log && WinJS.log(error, "sample", "error");
            });
        }
    }

    function printFileAsync(file) {
        Windows.Storage.FileIO.readTextAsync(file).then(function (fileContent) {
            WinJS.log && WinJS.log("Recieved File: " + file.name + "\n" + "File Content: " + fileContent, "sample", "status");
        });
    }
})();
