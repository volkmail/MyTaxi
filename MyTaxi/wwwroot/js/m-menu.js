$(function() {
  var link = $(".m-menu-link-btn");
  var close = $(".m-menu__close-menu");
  var menu = $(".m-menu");
  link.on("click", function(event) {
    event.preventDefault();
    menu.toggleClass("m-menu_active");
  });
  close.on("click", function(event) {
    event.preventDefault();
    menu.toggleClass("m-menu_active");
  });
});
