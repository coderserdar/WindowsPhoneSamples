//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved

(function () {
    "use strict";
    var page = WinJS.UI.Pages.define("/html/oAuthGoogle.html", {
        ready: function (element, options) {
            document.getElementById("oAuthGoogleLaunch").addEventListener("click", launchGoogleDesktopAuth, false);
            // Continuation handlers are specific to Windows Phone.
            if (options && options.activationKind === Windows.ApplicationModel.Activation.ActivationKind.webAuthenticationBrokerContinuation) {

                continueWebAuthentication(options.activatedEventArgs);
            }
        }
    });

    function isValidUriString(uriString) {
        var uri = null;
        try {
            uri = new Windows.Foundation.Uri(uriString);
        }
        catch (err) {
        }
        return uri !== null;
    }

    var authzInProgress = false;
    
    function launchGoogleDesktopAuth() {
        var googleURL = "https://accounts.google.com/o/oauth2/auth?";

        var clientID = document.getElementById("GoogleClientID").value;
        if (clientID === null || clientID === "") {
            WinJS.log("Please enter a ClientID for Google", "Web Authentication SDK Sample", "error");            
            return;
        }

        var callbackURL = document.getElementById("GoogleCallbackURL").value;
        if (!isValidUriString(callbackURL)) {
            WinJS.log("Enter a valid Callback URL for Google", "Web Authentication SDK Sample", "error");
            return;
        }

        if (authzInProgress) {
            document.getElementById("GoogleDebugArea").value += "\r\nAuthorization already in Progress ...";
            return;
        }

        googleURL += "client_id=" + clientID + "&redirect_uri=" + callbackURL + "&response_type=code&scope=http://picasaweb.google.com/data";

        document.getElementById("GoogleDebugArea").value += "Navigating to: " + googleURL + "\r\n";
        var startURI = new Windows.Foundation.Uri(googleURL);
        var endURI = new Windows.Foundation.Uri("https://accounts.google.com/o/oauth2/approval?");

        authzInProgress = true;
        if (Windows.Security.Authentication.Web.WebAuthenticationBroker.authenticateAndContinue) {
            Windows.Security.Authentication.Web.WebAuthenticationBroker.authenticateAndContinue(startURI, endURI, null, Windows.Security.Authentication.Web.WebAuthenticationOptions.none);
        }
        else {
            Windows.Security.Authentication.Web.WebAuthenticationBroker.authenticateAsync(
                Windows.Security.Authentication.Web.WebAuthenticationOptions.useTitle, startURI, endURI)
                .done(function (result) {
                    document.getElementById("GoogleReturnedToken").value = result.responseData;
                    document.getElementById("GoogleDebugArea").value += "Status returned by WebAuth broker: " + result.responseStatus + "\r\n";
                    if (result.responseStatus === Windows.Security.Authentication.Web.WebAuthenticationStatus.errorHttp) {
                        document.getElementById("GoogleDebugArea").value += "Error returned: " + result.responseErrorDetail + "\r\n";
                    }
                    authzInProgress = false;
                }, function (err) {
                    WinJS.log("Error returned by WebAuth broker: " + err, "Web Authentication SDK Sample", "error");
                    document.getElementById("GoogleDebugArea").value += " Error Message: " + err.message + "\r\n";
                    authzInProgress = false;
                });
        }
    }

    function continueWebAuthentication(args) {
        var result = args[0].webAuthenticationResult;

        if (result.responseStatus === Windows.Security.Authentication.Web.WebAuthenticationStatus.success) {
            document.getElementById("GoogleReturnedToken").value = result.responseData;
        }
        else if (result.responseStatus === Windows.Security.Authentication.Web.WebAuthenticationStatus.errorHttp) {
            document.getElementById("GoogleDebugArea").value += "Error returned: " + result.responseErrorDetail + "\r\n";
        }
        else {
            document.getElementById("GoogleDebugArea").value += "Status returned by WebAuth broker: " + result.responseStatus + "\r\n";
        }
        authzInProgress = false;

    }
})();
