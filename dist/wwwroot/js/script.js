var task = document.querySelectorAll(".task p");
var table1 = document.querySelector(".first-table");
var table2 = document.querySelector(".second-table");
var table3 = document.querySelector(".third-table");

task[0].addEventListener('click', function () {
    table1.style.display="block";
    table2.style.display="none";
    table3.style.display="none";

})
task[1].addEventListener('click', function () {
    table1.style.display="none";
    table2.style.display="block";
    table3.style.display="none";

})
task[2].addEventListener('click', function () {
    table1.style.display="none";
    table2.style.display="none";
    table3.style.display="block";

})




