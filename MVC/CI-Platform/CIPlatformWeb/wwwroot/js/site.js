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

//all filters code

$(document).ready(function () {
    $("#country-filter").change(function () {
        var country = $(this).val();
        $.ajax({
            url: "/Users/Home/GetCitiesByCountry",
            type: "GET",
            dataType: "json",
            data: { country: country },
            success: function (result) {

                var cityDropdown = $("#city-filter");
                cityDropdown.empty();

                $.each(result, function (i, city) {
                    $("#city-filter").append($('<option></option>').val(city.name).html(city.name));
                    //alert(city.name);
                });
            }
        });
    });
});

//country filter
const countryFilter = () => {
    const country = document.getElementById("country-filter");

    let countryName = document.getElementById("country-filter").value.toLowerCase();
    let missions = document.getElementsByClassName("grid-mission");
    //alert(missions.Count());
    //alert(countryName);

    for (var i = 0; i <= missions.length; i++) {
        var missionCountry = missions[i].getElementsByClassName("country-name")[0];
        //alert(missionCountry.value);
        if (missionCountry.value.toLowerCase().indexOf(countryName) > -1) {
            missions[i].style.display = "";

        }
        else {
            missions[i].style.display = "none";

            //tempCount++;
        }
    }

}

//Theme filter
const themeFilter = () => {
    const country = document.getElementById("theme-filter");

    let themeName = document.getElementById("theme-filter").value.toLowerCase();
    let missions = document.getElementsByClassName("grid-mission");
    //alert(missions.Count());
    //alert(themeName);

    for (var i = 0; i <= missions.length; i++) {
        var missionTheme = missions[i].getElementsByClassName("theme-name")[0];
        // alert(missionTheme.innerHTML);
        if (missionTheme.innerHTML.toLowerCase().indexOf(themeName) > -1) {
            missions[i].style.display = "";

        }
        else {
            missions[i].style.display = "none";

            //tempCount++;
        }
    }

}

//city filter
const cityFilter = () => {
    const city = document.getElementById("city-filter");

    let cityName = document.getElementById("city-filter").value.toLowerCase();
    let missions = document.getElementsByClassName("grid-mission");
    //alert(missions.Count());
    //alert(cityName);

    for (var i = 0; i <= missions.length; i++) {
        var missionCity = missions[i].getElementsByClassName("city-span")[0];
        //var missionCity = missions[i].querySelectorAll(".city-span");
        // alert("mission city=>>>>" + missionCity.innerHTML);
        if (missionCity.innerHTML.toLowerCase().indexOf(cityName) > -1) {
            missions[i].style.display = "";
            //alert("inside if");

        }
        else {
            missions[i].style.display = "none";
            // alert("inside else");
            //tempCount++;
        }
    }

}
//Skill filter
const skillFilter = () => {
    const skill = document.getElementById("skill-filter");

    let skillId = document.getElementById("skill-filter").value.toLowerCase();
    let missions = document.getElementsByClassName("grid-mission");
    //alert(missions.Count());
    //alert(skillId);

    for (var i = 0; i <= missions.length; i++) {
        var missionskillId = missions[i].getElementsByClassName("skill-id")[0];
        // alert(missionskillId.value);
        if (missionskillId.value == skillId) {
            missions[i].style.display = "";

        }
        else {
            missions[i].style.display = "none";

            //tempCount++;
        }
    }

}

//Serach Missions

const inputValue = document.getElementById("search-bar");
//inputValue.addEventListner("keyup", searchMissions);
const searchMissions = () => {

    let inputValue = document.getElementById("search-bar").value.toLowerCase();
    let missions = document.getElementsByClassName("grid-mission");
    let missionCount = @Model.MissionList.Count();
    // alert("hiii");
    //console.log("hello method called!!!");

    //alert(missionCount);
    var missionTitle;
    //let tempCount = 0;
    //let count;

    for (var i = 0; i <= missions.length; i++) {
        missionTitle = missions[i].getElementsByClassName("card-title")[0];
        if (missionTitle.innerHTML.toLowerCase().indexOf(inputValue) > -1)
            missions[i].style.display = "";

        else {
            missions[i].style.display = "none";

        }
    }


}

console.log(inputValue);

