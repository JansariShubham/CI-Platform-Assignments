//Get Cities By Country
$(document).ready(function () {
    var filterMissionsCount = document.getElementById("missionCountLanding").value;
    var missionCounts = document.getElementById("filterMissions");
    missionCounts.textContent = filterMissionsCount;
    indexPagination();
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
                //$("#city-filter").append($('<option></option>').val('city').html("City"));
                //$.each(result, function (i, city) {
                //    $("#city-filter").append($('<option></option>').val(city.cityId).html(city.name));
                //    //alert(city.name);
                //});
                const cityFilter = document.querySelector("#city-filter");
                cityFilter.innerHTML = `<option value="1" selected disabled>City</option>`;
                result.forEach((c) => {
                    cityFilter.innerHTML += `
                    <option value=${c.cityId}>${c.name}</option>
                        `
                })
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
var pageNumber = 1;


var searchText = document.getElementById("search-bar");
searchText.addEventListener('input', () => {
    search = searchText.value;
    pageNumber = 1;
    getFilterData();
})

var cityValue = document.getElementById("city-filter");
cityValue.addEventListener('change', () => {
    city.push(+cityValue.value);
    pageNumber = 1;
    getFilterData();
    var id = cityValue.value;
    var item = cityValue.options[cityValue.selectedIndex].text;
    var type = "city";
    // console.log("id=>" + id +"item=>" + item);
    addFilterToHtmlList(id, item, type)
})

var countryValue = document.getElementById("country-filter");
countryValue.addEventListener('change', () => {
    country.push(+countryValue.value);
    //console.log(country);
    pageNumber = 1;
    getFilterData();

    var id = countryValue.value;
    var item = countryValue.options[countryValue.selectedIndex].text;
    var type = "country";
    addFilterToHtmlList(id, item, type)

})

var themeValue = document.getElementById("theme-filter");
themeValue.addEventListener('change', () => {
    theme.push(+themeValue.value);
    pageNumber = 1;
    getFilterData();

    var id = themeValue.value;
    var item = themeValue.options[themeValue.selectedIndex].text;
    var type = "theme";
    addFilterToHtmlList(id, item, type)
})


var skillValue = document.getElementById("skill-filter");
skillValue.addEventListener('change', () => {
    skill.push(+skillValue.value);
    pageNumber = 1;
    getFilterData();


    var id = skillValue.value;
    var item = skillValue.options[skillValue.selectedIndex].text;
    var type = "skill";
    addFilterToHtmlList(id, item, type)

})



var exploreDiv = document.getElementById("exploreDiv");
exploreDiv.classList.remove("d-none");

$("#exploreDropdown li").click((e) => {
    
    var id = e.currentTarget.dataset.id;
  
})






function getFilterData() {

    const obj = {
        searchText: search,
        cityList: city,
        countryList: country,
        themeList: theme,
        skillList: skill,
        sortingList: sortByValue,
        pageNum: pageNumber,
        userId: userId

    };
    console.log(obj);
    $.ajax({
        type: "POST",
        url: "/Users/Home/GetFilterData",
        data: obj,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: (result) => {
            $("#partial").html(result);
            indexPagination();
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
            filterMissions();
        },
        error: (err) => {
            console.log("error...");
        }


    })

}
userFilterList = document.getElementById("userFilterList");
filterOptionArea = document.getElementById("filterOptionArea");

function addFilterToHtmlList(id, item, type) {
    filterOptionArea.classList.remove('d-none');
    let li = document.createElement('li');
    li.classList.add(`gap-2`);
    li.classList.add(`rounded-5`);
    li.classList.add(`d-flex`);
    li.classList.add(`filter-option-value`);
    li.dataset.id = id;
    li.dataset.type = type;

    let image = document.createElement('img');
    image.src = "./images/cancel1.png";
    image.alt = "Cancel";
    image.classList.add('cancle-img');
    image.classList.add('cursor-pointer');


    li.textContent = item.trim();
    li.append(image);

    image.addEventListener('click', () => {
        let id = image.parentElement.dataset.id;
        let type = image.parentElement.dataset.type;

        if (type === "country")
            removeElement(id, country);
        else if (type === "city")
            removeElement(id, city);
        else if (type === "theme")
            removeElement(id, theme);
        else
            removeElement(id, skill);
        image.parentElement.remove();
        getFilterData();
        // if (userFilterList.childElementCount === 0) {  }
    });
    userFilterList.appendChild(li);
}

function removeElement(id, list) { list.splice(list.indexOf(id), 1) }


var clearAllFilter = document.querySelector(".clear-btn");
clearAllFilter.addEventListener("click", () => {
    filterOptionArea.classList.add('d-none');
    country.splice(0, country.length);
    city.splice(0, city.length);
    theme.splice(0, city.length);
    skill.splice(0, city.length);
    getFilterData();
    userFilterList.innerHTML = "";
})

var sortByFilter = document.getElementById("sortByFilter");
var sortByValue;
sortByFilter.addEventListener("change", () => {
    sortByValue = sortByFilter.value;
    getFilterData();
    //console.log(sortByFilter.value);
})



var ulPagination = document.querySelector(".pagination");
function indexPagination() {
    var missionCount = document.getElementById("missionCount").value;
    ulPagination.innerHTML = "";
    //console.log("missions ===>" + missionCount);

    var totalPages = Math.ceil(missionCount / 4);
    if (missionCount < 4) {
        return;
    }
    else {
        let li = document.createElement('li');
        li.classList.add('page-item1');
        let a = document.createElement('a');
        a.classList.add('page-link');
        a.classList.add('left-arrow');
        // a.dataset.id = pageNumber;
        li.classList.add('cursor-pointer');
        li.append(a);
        let image = document.createElement('img');
        image.src = "./images/left.png";
        a.append(image);
        ulPagination.appendChild(li);

        var i;
        for (i = 1; i < totalPages + 1; i++) {

            let li = document.createElement('li');
            li.classList.add('page-item');
            let a = document.createElement('a');
            a.classList.add('page-link');
            li.classList.add('cursor-pointer');
            li.append(a);
            li.dataset.id = i;
            a.textContent = i;
            ulPagination.appendChild(li);
        }


        let list = document.createElement('li');
        list.classList.add('page-item1');
        let anchor = document.createElement('a');
        //anchor.classList.add('page-link-side-arrows');
        anchor.classList.add('page-link');
        anchor.classList.add('right-arrow');
        // anchor.dataset.id = pageNumber;
        //console.log(i);
        list.classList.add('cursor-pointer');
        list.append(anchor);
        let images = document.createElement('img');
        images.src = "./images/right-arrow1.png";
        anchor.append(images);
        ulPagination.appendChild(list);



        var rightArrow = document.querySelector('.right-arrow');
        rightArrow.addEventListener('click', () => {
            //console.log("rightarrowCalled!!!")
            if (pageNumber >= 3) {
                pageNumber = 1;
            }
            else {
                pageNumber++;
            }
            //console.log("pageNumber ===>>>" + pageNumber)
            getFilterData();
        })
        var leftArrow = document.querySelector('.left-arrow');
        leftArrow.addEventListener('click', () => {
            //console.log("rightarrowCalled!!!")
            if (pageNumber <= 1) {
                pageNumber = 1;
            }
            else {

                pageNumber--;
            }
            //console.log("pageNumber ===>>>" + pageNumber)
            getFilterData();
        })
    }


    var pages = document.querySelectorAll('.page-item');
    pages.forEach((page) => {
        page.addEventListener("click", () => {
            pageNumber = page.dataset.id;
            getFilterData();
        })
    })
}


function filterMissions() {

    var filterMissionsCount = document.getElementById("missionCount").value;
    // console.log("filter missions: " + filterMissionsCount)
    var missionCounts = document.getElementById("filterMissions");
    missionCounts.textContent = filterMissionsCount;
    //console.log("nmew missions: " + missionCounts.value);
}

var userId = document.getElementById("userId").value;



function addRemoveFavourate(missionId) {
    userId = document.getElementById("userId").value;
    if (userId != null && userId != "") {



        $.ajax({
            url: "/Users/MissionDetail/AddOrRemoveFavourite",
            type: "POST",
            data: { userId: userId, missionId: missionId },
            success: function (result) {
                isFavourite = 0;
                getFilterData();


            },
            error: function (err) {
                console.log("error");
            }


        });
    }
}

