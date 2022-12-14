(function() {
  "use strict";
  var sampleTitle = "Orientation Sensor";
  var scenarios = [{
    url: "/html/scenario1_DataEvents.html",
    title: "Data Events"
}, {
    url: "/html/scenario2_Polling.html",
    title: "Polling"
}, {
    url: "/html/scenario3_Calibration.html",
    title: "Calibration"
}];
  WinJS.Namespace.define("SdkSample", {
    sampleTitle: sampleTitle,
    scenarios: new WinJS.Binding.List(scenarios)
});
})();