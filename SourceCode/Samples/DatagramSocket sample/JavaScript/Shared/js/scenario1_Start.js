﻿//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved

(function () {
    "use strict";
    
    // Array containing all available local HostName endpoints
    var localHostItems = [];
    
    var page = WinJS.UI.Pages.define("/html/scenario1_Start.html", {
        ready: function (element, options) {
            document.getElementById("buttonStartListener").addEventListener("click", startListener, false);
            document.getElementById("serviceNameAccept").addEventListener("change", socketsSample.getValues, false);
            document.getElementById("bindToAny").addEventListener("change", bindToAnyChanged, false);
            document.getElementById("bindToAddress").addEventListener("change", bindToAnyChanged, false);
            document.getElementById("bindToAdapter").addEventListener("change", bindToAnyChanged, false);

            socketsSample.hostNameConnect = "localhost";
            socketsSample.setValues();
            populateAdapterList();
        }
    });

    function bindToAnyChanged() {
        var bindToAny = document.getElementById("bindToAny");
        var adapterList = document.getElementById("adapterList");
        
        adapterList.disabled = bindToAny.checked;
    }

    function startListener() {
        var serviceName = document.getElementById("serviceNameAccept").value;
        var bindToAny = document.getElementById("bindToAny").checked;
        var bindToAddress = document.getElementById("bindToAddress").checked;
        var bindToAdapter = document.getElementById("bindToAdapter").checked;
        var inboundBufferSize = document.getElementById("inboundBufferSize").value;
        var adapterListSelectedIndex = document.getElementById("adapterList").selectedIndex;

        if (serviceName === "") {
            WinJS.log && WinJS.log("Please provide a service name.", "sample", "error");
            return;
        }
            
        if (socketsSample.listener) {
            WinJS.log && WinJS.log("Already have a listener; call close to close the listener.", "sample", "error");
            return;
        }

        var selectedLocalHost = localHostItems[adapterListSelectedIndex];

        socketsSample.closing = false;
        socketsSample.bindingToService = true;
        socketsSample.listener = new Windows.Networking.Sockets.DatagramSocket();
        socketsSample.listener.addEventListener("messagereceived", onServerMessageReceived);
        WinJS.log && WinJS.log("Server: listener creation started.", "sample", "status");

        if (inboundBufferSize.trim() !== "") {

            if (isNaN(parseInt(inboundBufferSize))) {
                onError("Please provide a positive numeric Inbound buffer size.");
                return;
            }

            try {
                // Running both client and server on localhost will most likely not reach the buffer limit.
                socketsSample.listener.control.inboundBufferSizeInBytes = parseInt(inboundBufferSize);
            }
            catch (e) {
                onError("Please provide a valid Inbound buffer size.");
                return;
            }
        }

        if (bindToAny) {
            // Don't limit traffic to an address or an adapter.
            socketsSample.listener.bindServiceNameAsync(serviceName).done(function () {
                WinJS.log && WinJS.log("Listening", "sample", "status");
                socketsSample.bindingToService = false;
            }, onError);
        } else if (bindToAddress) {
            // The user selected an address. For demo purposes, we ensure that connect will be using the same address.
            socketsSample.hostNameConnect = selectedLocalHost.canonicalName;

            // Try to bind to a specific address.
            socketsSample.listener.bindEndpointAsync(selectedLocalHost, serviceName).done(function () {
                WinJS.log && WinJS.log("Listening on address " + selectedLocalHost.canonicalName, "sample", "status");
                socketsSample.bindingToService = false;
            }, onError);
        } else if (bindToAdapter) {
            // The user selected an address. For demo purposes, we ensure that connect will be using the same address.
            socketsSample.hostNameConnect = selectedLocalHost.canonicalName;

            // Try to limit traffic to the selected adapter. 
            // This option will be overriden by interfaces with weak-host or forwarding modes enabled.
            socketsSample.listener.bindServiceNameAsync(
                serviceName,
                selectedLocalHost.ipInformation.networkAdapter).done(function () {
                    WinJS.log && WinJS.log(
                        "Listening on adapter " + selectedLocalHost.ipInformation.networkAdapter.networkAdapterId,
                        "sample",
                        "status");
                    socketsSample.bindingToService = false;
                }, onError);
        }
    }

    function onServerMessageReceived(eventArgument) {
        if (socketsSample.listenerOutputStream) {
            echoMessage(socketsSample.listenerOutputStream, eventArgument);
            return;
        }
            
        socketsSample.listener.getOutputStreamAsync(
            eventArgument.remoteAddress,
            eventArgument.remotePort).done(function (outputStream) {

            if (!socketsSample.listenerOutputStream) {
                socketsSample.listenerOutputStream = outputStream;
                socketsSample.listenerPeerAddress = eventArgument.remoteAddress;
                socketsSample.listenerPeerPort = eventArgument.remotePort;
            }

            echoMessage(socketsSample.listenerOutputStream, eventArgument);
        });
    }

    function echoMessage(outputStream, eventArgument) {
        if (socketsSample.listenerPeerAddress !== eventArgument.remoteAddress ||
            socketsSample.listenerPeerPort !== eventArgument.remotePort) {
            WinJS.log && WinJS.log(
                "Got datagram from " + eventArgument.remoteAddress + ":" + eventArgument.remotePort +
                ", but already 'connected' to " + socketsSample.listenerPeerAddress + ":" +
                socketsSample.listenerPeerPort,
                "sample",
                "status");
            return;
        }

        outputStream.writeAsync(eventArgument.getDataReader().detachBuffer()).done(function () {
            // Do nothing - client will print out a message when data is received.
        });
    }

    function onError(reason) {
        // Clean up a listener if we failed to bind to a port.
        if (socketsSample.bindingToService) {
            socketsSample.listener = null;
            socketsSample.bindingToService = false;
        }
        
        // When we close a socket, outstanding async operations will be canceled and the
        // error callbacks called.  There's no point in displaying those errors.
        if (!socketsSample.closing) {
            WinJS.log && WinJS.log(reason, "sample", "error");
        }
    }

    function populateAdapterList() {
        // Clear both the localHostItems list and the select element.
        localHostItems.length = 0;
        var adapterList = document.getElementById("adapterList");
        while (adapterList.options.length > 0) {
            adapterList.options.remove(0);
        }

        // Populate the local address and adapter pairs collection.
        var localHostNames = Windows.Networking.Connectivity.NetworkInformation.getHostNames();
        for (var i = 0; i < localHostNames.length; i++) {
            if (localHostNames[i].ipInformation !== null) {
                localHostItems.push(localHostNames[i]);
                var option = document.createElement("option");
                option.textContent = "Address: " + localHostNames[i].displayName +
                    " Adapter: " + localHostNames[i].ipInformation.networkAdapter.networkAdapterId;
                option.value.textContent = localHostItems.length - 1;
                adapterList.appendChild(option);
            }
        }
    }
})();
