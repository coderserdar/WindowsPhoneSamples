(function() {
  "use strict";
  var sampleTitle = "Simple Orientation Sensor";
  var scenarios = [{
    url: "/html/scenario1_DataEvents.html",
    title: "Data Events"
}, {
    url: "/html/scenario2_Polling.html",
    title: "Polling"
}];
  WinJS.Namespace.define("SdkSample", {
    sampleTitle: sampleTitle,
    scenarios: new WinJS.Binding.List(scenarios)
});
})();