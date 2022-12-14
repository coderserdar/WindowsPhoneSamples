//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved

(function () {
    "use strict";
    var page = WinJS.UI.Pages.define("/html/scenario1_Files.html", {
        ready: function (element, options) {
            document.getElementById("filesLocalIncrement").addEventListener("click", filesLocalIncrement, false);
            document.getElementById("filesRoamingIncrement").addEventListener("click", filesRoamingIncrement, false);
            document.getElementById("filesTemporaryIncrement").addEventListener("click", filesTemporaryIncrement, false);
            filesDisplayOutput();
        }
    });

    var localFolder = Windows.Storage.ApplicationData.current.localFolder;
    var roamingFolder = Windows.Storage.ApplicationData.current.roamingFolder;
    var temporaryFolder = Windows.Storage.ApplicationData.current.temporaryFolder;
    var filename = "sampleFile.txt";
    var localCounter = 0;
    var roamingCounter = 0;
    var temporaryCounter = 0;

    // Guidance for Local, Roaming, and Temporary files.
    //
    // Files are ideal for storing large data-sets, databases, or data that is
    // in a common file-format.
    //
    // Files can exist in either the Local, Roaming, or Temporary folders.
    //
    // Roaming files will be synchronized across machines on which the user has
    // singed in with a connected account.  Roaming of files is not instant; the
    // system weighs several factors when determining when to send the data.  Usage
    // of roaming data should be kept below the quota (available via the 
    // RoamingStorageQuota property), or else roaming of data will be suspended.
    // Files cannot be roamed while an application is writing to them, so be sure
    // to close your application's file objects when they are no longer needed.
    //
    // Local files are not synchronized and remain on the machine on which they
    // were originally written.
    //
    // Temporary files are subject to deletion when not in use.  The system 
    // considers factors such as available disk capacity and the age of a file when
    // determining when or whether to delete a temporary file.

    function filesLocalIncrement() {
        localCounter = localCounter + 1;

        localFolder.createFileAsync(filename, Windows.Storage.CreationCollisionOption.replaceExisting)
            .then(function (file) {
                return Windows.Storage.FileIO.writeTextAsync(file, localCounter);
            }).done(function () {
                filesReadLocalCounter();
            });
    }

    function filesReadLocalCounter() {
        localFolder.getFileAsync(filename)
            .then(function (file) {
                return Windows.Storage.FileIO.readTextAsync(file);
            }).done(function (text) {
                localCounter = parseInt(text);
                document.getElementById("filesLocalOutput").innerText = "Local Counter: " + localCounter;
            }, function () {
                // getFileAsync or readTextAsync failed.
                document.getElementById("filesLocalOutput").innerText = "Local Counter: <not found>";
            });
    }

    function filesRoamingIncrement() {
        roamingCounter = roamingCounter + 1;

        roamingFolder.createFileAsync(filename, Windows.Storage.CreationCollisionOption.replaceExisting)
            .then(function (file) {
                return Windows.Storage.FileIO.writeTextAsync(file, roamingCounter);
            }).done(function () {
                filesReadRoamingCounter();
            });
    }

    function filesReadRoamingCounter() {
        roamingFolder.getFileAsync(filename)
            .then(function (file) {
                return Windows.Storage.FileIO.readTextAsync(file);
            }).done(function (text) {
                roamingCounter = parseInt(text);
                document.getElementById("filesRoamingOutput").innerText = "Roaming Counter: " + roamingCounter;
            }, function () {
                // getFileAsync or readTextAsync failed.
                document.getElementById("filesRoamingOutput").innerText = "Roaming Counter: <not found>";
            });
    }

    function filesTemporaryIncrement() {
        temporaryCounter = temporaryCounter + 1;

        temporaryFolder.createFileAsync(filename, Windows.Storage.CreationCollisionOption.replaceExisting)
            .then(function (file) {
                return Windows.Storage.FileIO.writeTextAsync(file, temporaryCounter);
            }).done(function () {
                filesReadTemporaryCounter();
            });
    }

    function filesReadTemporaryCounter() {
        temporaryFolder.getFileAsync(filename)
            .then(function (file) {
                return Windows.Storage.FileIO.readTextAsync(file);
            }).done(function (text) {
                temporaryCounter = parseInt(text);
                document.getElementById("filesTemporaryOutput").innerText = "Temporary Counter: " + temporaryCounter;
            }, function () {
                // getFileAsync or readTextAsync failed.
                document.getElementById("filesTemporaryOutput").innerText = "Temporary Counter: <not found>";
            });
    }

    function filesDisplayOutput() {
        filesReadLocalCounter();
        filesReadRoamingCounter();
        filesReadTemporaryCounter();
    }
})();
