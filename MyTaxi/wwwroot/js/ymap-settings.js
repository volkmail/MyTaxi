ymaps.ready(function() {
  var myMap = new ymaps.Map("map", {
    center: [48.4861, 135.0797],
    zoom: 12,
    controls: ["zoomControl"],
    behaviors: ["drag"]
  });

  var suggestViews = new Array();
  suggestViews[0] = new ymaps.SuggestView("suggest1");
  suggestViews[1] = new ymaps.SuggestView("suggest2");

  var all_block_inputs = document.getElementsByClassName("order-body__form-inputs"),
      last_inputs = all_block_inputs[all_block_inputs.length - 1].cloneNode(false),
      clone_inputs = all_block_inputs[all_block_inputs - 2].cloneNode(true),
      current_inputs = all_block_inputs[all_block_inputs - 3].cloneNode(false);
});