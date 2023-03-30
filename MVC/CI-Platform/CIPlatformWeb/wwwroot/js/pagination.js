
    //< !---------Pagination  ----------->
function getStory() {

    $.ajax({
        type: "GET",
        url: "/Users/Story/GetStories",
        data: {
            pageNum: pageNumber
        },
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: (result) => {
            $("#storyListingPartial").html(result);

        },
        error: (err) => {
            console.log("error...");
        }


    })
}


var storyCount = document.getElementById("storyCount").value;
var ulPagination = document.querySelector(".pagination");
//console.log("missions ===>" + missionCount);
var totalPages = Math.ceil(storyCount / 1);
if (storyCount < 1) {

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
    image.src = "/images/left.png";
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
    
    anchor.classList.add('page-link');
    anchor.classList.add('right-arrow');
    // anchor.dataset.id = pageNumber;
    //console.log(i);
    list.classList.add('cursor-pointer');
    list.append(anchor);
    let images = document.createElement('img');
    images.src = "/images/right-arrow1.png";
    anchor.append(images);
    ulPagination.appendChild(list);



}


var pages = document.querySelectorAll('.page-item');
pages.forEach((page) => {
    page.addEventListener("click", () => {
        pageNumber = page.dataset.id;
        getStory();
    })
})

var rightArrow = document.querySelector('.right-arrow');
rightArrow.addEventListener('click', () => {
    //console.log("rightarrowCalled!!!")
    if (pageNumber >= 2) {
        pageNumber = 1;
    }
    else {
        pageNumber++;
    }
    getStory();
    //console.log("pageNumber ===>>>" + pageNumber)

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
    getStory();
    //console.log("pageNumber ===>>>" + pageNumber)
    
})