//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved

(function () {
    "use strict";

    var streamWebSocket;
    var dataWriter;
    var dataReader;
    var data = "Hello World";
    var countOfDataSent;
    var countOfDataReceived;

    var page = WinJS.UI.Pages.define("/html/scenario2_Upload.html", {
        ready: function (element, options) {
            document.getElementById("startButton").addEventListener("click", start, false);
            document.getElementById("closeButton").addEventListener("click", closeSocket, false);
        },
        unload: function (eventArgument) {
            closeSocket();
        }
    });

    function start() {
        if (streamWebSocket) {
            WinJS.log && WinJS.log("Already started", "sample", "status");
            return;
        }

        var webSocket = new Windows.Networking.Sockets.StreamWebSocket();
        webSocket.onclosed = onClosed;

        // Validating the URI is required since it was received from an untrusted source (user
        // input). The URI is validated by calling validateAndCreateUri() that will return 'null' for
        // strings that are not valid WebSocket URIs.
        // Note that when enabling the text box users may provide URIs to machines on the intrAnet
        // or intErnet. In these cases the app requires the "Home or Work Networking" or
        // "Internet (Client)" capability respectively.
        var uri = validateAndCreateUri(document.getElementById("serverAddress").value);
        if (!uri) {
            return;
        }

        WinJS.log && WinJS.log("Connecting to: " + uri.absoluteUri, "sample", "status");

        webSocket.connectAsync(uri).done(function () {
            WinJS.log && WinJS.log("Connected", "sample", "status");

            streamWebSocket = webSocket;
            dataWriter = new Windows.Storage.Streams.DataWriter(webSocket.outputStream);
            dataReader = new Windows.Storage.Streams.DataReader(webSocket.inputStream);
            // When buffering, return as soon as any data is available.
            dataReader.inputStreamOptions = Windows.Storage.Streams.InputStreamOptions.partial;
            countOfDataSent = 0;
            countOfDataReceived = 0;

            // Continuously send data to the server
            writeOutgoing();

            // Continuously listen for a response
            readIncoming();

        }, function (error) {
            var errorStatus = Windows.Networking.Sockets.WebSocketError.getStatus(error.number);
            if (errorStatus === Windows.Web.WebErrorStatus.cannotConnect ||
                errorStatus === Windows.Web.WebErrorStatus.notFound ||
                errorStatus === Windows.Web.WebErrorStatus.requestTimeout) {
                WinJS.log && WinJS.log("Cannot connect to the server. Please make sure " +
                    "that you run the server setup script before running the sample.", "sample", "error");
            } else {
                WinJS.log && WinJS.log("Failed to connect: " + getError(error), "sample", "error");
            }
        });
    }

    function writeOutgoing() {
        try {
            var size = dataWriter.measureString(data);
            countOfDataSent += size;
            if (!document.getElementById("dataSent")) {
                return; // We switched scenarios
            }
            document.getElementById("dataSent").value = countOfDataSent;

            dataWriter.writeString(data);
            dataWriter.storeAsync().done(function () {
                // Add a 1 second delay so the user can see what's going on.
                setTimeout(writeOutgoing, 1000);
            }, writeError);
        }
        catch (error) {
            log("Sync write error: " + getError(error));
        }
    }

    function writeError(error) {
        log("Write error: " + getError(error));
    }

    function readIncoming(args) {
        // Buffer as much data as you require for your protocol.
        dataReader.loadAsync(100).done(function (sizeBytesRead) {
            countOfDataReceived += sizeBytesRead;
            if (!document.getElementById("dataReceived")) {
                return; // We switched scenarios
            }
            document.getElementById("dataReceived").value = countOfDataReceived;

            var incomingBytes = new Array(sizeBytesRead);
            dataReader.readBytes(incomingBytes);

            // Do something with the data.
            // Alternatively you can use DataReader to read out individual booleans,
            // ints, strings, etc.

            // Start another read.
            readIncoming();
        }, readError);
    }

    function readError(error) {
        log("Read Error: " + getError(error));
    }

    function onClosed(args) {
        log("Closed; Code: " + args.code + " Reason: " + args.reason);
        if (!streamWebSocket) {
            return;
        }

        closeSocketCore();
    }

    function closeSocket() {
        if (!streamWebSocket) {
            WinJS.log && WinJS.log("Not connected", "sample", "status");
            return;
        }

        WinJS.log && WinJS.log("Closing", "sample", "status");
        closeSocketCore(1000, "Closed due to user request.");
    }

    function closeSocketCore(closeCode, closeStatus) {
        if (closeCode && closeStatus) {
            streamWebSocket.close(closeCode, closeStatus);
        } else {
            streamWebSocket.close();
        }

        streamWebSocket = null;

        if (dataWriter) {
            dataWriter.close();
        }

        if (dataReader) {
            dataReader.close();
        }
    }

    function log(text) {
        var outputFiled = document.getElementById("outputField");
        if (outputFiled !== null) {
            outputFiled.innerHTML += text + "<br>";
        }
    }

})();
