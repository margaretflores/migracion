$(document).ready(function () {
    var highestCol = Math.max($('nav').height());
    $('.subMenu').height(highestCol);
    $('.todo_contenido').height(highestCol);
});