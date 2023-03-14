//Get Cities By Country
$(document).ready(function () {
    $("#country-filter").change(function () {
        var country = $(this).val();
        console.log(country);
        $.ajax({
            url: "/Users/Home/GetCitiesByCountry",
            type: "GET",
            dataType: "json",
            data: { country: country },
            success: function (result) {

                var cityDropdown = $("#city-filter");
                cityDropdown.empty();

                $.each(result, function (i, city) {
                    $("#city-filter").append($('<option></option>').val(city.cityid).html(city.name));
                    //alert(city.name);
                });
            }
        });
    });
});


var city = [];
var country = [];
var theme = [];
var skill = [];
var search;
var sort;


var searchText = document.getElementById("search-bar");
searchText.addEventListener('input', () => {
        search = searchText.value;
})

var cityValue = document.getElementById("city-filter");
cityValue.addEventListener('change', () => {
    city.push(+cityValue.value);
    getFilterData();
})

var countryValue = document.getElementById("country-filter");
countryValue.addEventListener('change', () => {
    country.push(+countryValue.value);
    console.log(country);
    getFilterData();
})

var themeValue = document.getElementById("theme-filter");
themeValue.addEventListener('change', () => {
    theme.push(+themeValue.value);
    getFilterData();
})


var skillValue = document.getElementById("skill-filter");
skillValue.addEventListener('change', () => {
    skill.push(+skillValue.value);
    getFilterData();
})




function getFilterData() {

    const obj = {
        searchText: search,
        cityList: city,
        countryList: country,
        themeList: theme,
        skillList: skill
    };
    console.log(obj);
    $.ajax({
        type: "POST",
        url: "/Users/Home/GetFilterData",
        data: obj,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: (result) => {
            console.log(result);
            $("#partial").html(result);

        },
        error: (err) => {
            console.log("error...");
        }


    })






}