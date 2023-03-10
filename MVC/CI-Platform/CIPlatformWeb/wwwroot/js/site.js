// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


    const filterBtn = document.querySelector(".filter-btn");
    const sideBar = document.querySelector("#filter-menu");

        filterBtn.addEventListener("click", () => {
        sideBar.classList.toggle("hide-aside");
        });

    const gridBtn = document.querySelector(".grid-btn");
    const listBtn = document.querySelector(".list-btn");
    const listView = document.querySelector(".list-view");
    const gridView = document.querySelector(".grid-view");
        gridBtn.addEventListener("click", () => {
        listView.classList.add("d-none");
    gridView.classList.remove("d-none")

        });

        listBtn.addEventListener("click", () => {
        gridView.classList.add("d-none");
    listView.classList.remove("d-none")

        });
    const searchBtn = document.getElementById("search-btn");
    const searchBar = document.getElementById("search-bar");


        searchBtn.addEventListener("click", () => {
        searchBar.focus();
        });


