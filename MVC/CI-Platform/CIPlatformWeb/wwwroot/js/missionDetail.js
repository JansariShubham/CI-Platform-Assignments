

const missionBtn = document.querySelector(".mission-btn");
const orgBtn = document.querySelector(".org-btn");
const commentBtn = document.querySelector(".comment-btn");


const missionDiv = document.querySelector(".mission-div");
const orgDiv = document.querySelector(".org-div");
const commentDiv = document.querySelector(".comment-div");

missionBtn.addEventListener("click", () => {
    orgDiv.classList.add("d-none");
    commentDiv.classList.add("d-none");
    missionDiv.classList.remove("d-none");
    missionBtn.classList.add("active-tab");
    orgBtn.classList.remove("active-tab");
    commentBtn.classList.remove("active-tab");
});


orgBtn.addEventListener("click", () => {
    missionDiv.classList.add("d-none");
    commentDiv.classList.add("d-none");
    orgDiv.classList.remove("d-none");
    orgBtn.classList.add("active-tab");
    missionBtn.classList.remove("active-tab");
    commentBtn.classList.remove("active-tab");
});


commentBtn.addEventListener("click", () => {
    missionDiv.classList.add("d-none");
    orgDiv.classList.add("d-none");
    commentDiv.classList.remove("d-none");
    commentBtn.classList.add("active-tab");
    missionBtn.classList.remove("active-tab");
    orgBtn.classList.remove("active-tab");
});

var userId = document.getElementById("userId").value;
var missionId = document.getElementById("missionId").value;

//console.log(favImg);
var favSpan = document.querySelector(".fav-span");
var favImg = document.querySelector(".fav-img");

var favMissionObj = {
    userId: userId,
    missionId: missionId
}
//console.log(favMissionObj);

var isClicked = false;
var addToFavourite = document.getElementById("addToFavourite");

//console.log("Outside..." + isClicked);

addToFavourite.addEventListener("click", () => {
    if (isClicked == true) {


        $.ajax({
            url: "/Users/MissionDetail/RemoveFromFavourite",
            type: "POST",
            data: favMissionObj,
            success: function (result) {

                favSpan.textContent = "Add to Favourite";
                favImg.src = "/images/heart1.png";
                //addToFavourite.disabled = true;
                isClicked = false;
            },
            error: function (err) {
                console.log("error");
            }


        })
    }
    else {
        $.ajax({
            url: "/Users/MissionDetail/AddToFavourite",
            type: "POST",
            data: favMissionObj,
            success: function (result) {

                favSpan.textContent = "Remove From Favourite";
                favImg.src = "/images/fill-heart.png";
                //addToFavourite.disabled = true;
                isClicked = true;
                console.log(isClicked);
            },
            error: function (err) {
                console.log("error");
            }


        })

    }
})



$(document).ready(() => {

    let userId = document.getElementById("userId").value;
    let missionId = document.getElementById("missionId").value
    let starInput = $("#star-input-id");
    console.log(starInput);
    addStar(starInput,userId,missionId);

});
function addStar(starInput, userId, missionId) {
    console.log(starInput);
    $("#rating .rating-stars").on("click", function (event) {
        var starRating = Math.ceil(+starInput.val());
        //console.log("Hiiii==>>>>" +starRating);
        $.ajax({
            type: "POST",
            url: '/Users/MissionDetail/AddRatings',
            data: { userId: userId, missionId: missionId, rating: starRating },
            success: function (data) {
            }
        });
    });
}

