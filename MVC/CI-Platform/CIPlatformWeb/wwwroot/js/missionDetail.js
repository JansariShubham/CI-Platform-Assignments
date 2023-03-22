

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
if (userId != null && userId != "") {
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
                    console.log("Removed");
                    console.log(isClicked)
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
                    console.log("added");
                    console.log(isClicked);
                },
                error: function (err) {
                    console.log("error");
                }


            })

        }
    })
}






//var ratingsBtnClicked = true; 

$(document).ready(() => {
    
    getRelatedMission();

    let starInput = $("#star-input-id");
   // console.log(starInput);
    addStar(starInput, userId, missionId);
    
    getComments();
    


});


function getRelatedMission() {

    let missionId = document.getElementById("missionId").value

    $.ajax({
        type: "GET",
        url: '/Users/MissionDetail/getRelatedMission',
        data: { missionId: missionId },
        success: (data) => {
            $("#realtedMission").html(data);
            
        },
        error: (err) => {
            console.log("error");
        }
    })
}




    function addStar(starInput, userId, missionId) {
        console.log(starInput);
        $("#rating .rating-stars").on("click", function (event) {
            var starRating = Math.ceil(+starInput.val());
            console.log("Hiiii==>>>>" +starRating);
            $.ajax({
                type: "POST",
                url: '/Users/MissionDetail/AddRatings',
                data: { userId: userId, missionId: missionId, rating: starRating },
                success: function (data) {
                    ratingsBtnClicked == false;

                }
            });
        });
    }



var commentMsg = document.getElementById("commentMsg").value;
console.log(commentMsg);
var commentsBtn = document.getElementById("commentBtn");
//console.log("btn")

function enableDisableBtn() {
   // console.log("hello inside fn");
    if (document.getElementById("commentMsg").value.length) {
        
        commentsBtn.disabled = false;
    }
    else {
        //console.log("In hiii");
        commentsBtn.disabled = true;
    }
    
}
commentsBtn.addEventListener("click", () => {
    var commentMsg = document.getElementById("commentMsg").value;
    $.ajax({
        type: "POST",
        url: '/Users/MissionDetail/AddComments',
        data: { userId: userId, missionId: missionId, commentText: commentMsg },
        success:(data) => {
           // console.log("Comments Addded");
            alert("Comment Added Successfully");
            $("#partialComment").html(data)
           document.getElementById("commentMsg").value = '';
    },
        error : (err) => {
            console.log("Error in adding comment");
            }

        })
})

function getComments() {
$.ajax({
    type: "GET",
    url: '/Users/MissionDetail/getAllComments',
    data: {missionId: missionId},
    success: (data) => {
        $("#partialComment").html(data)

    },
    error: (err) => {
        console.log("Error in adding comment");
    }

})

}

var recommendBtn = document.getElementById("recommendedBtn");
recommendBtn.addEventListener("click", () => {
    $.ajax({
        type: "GET",
        url: '/Users/MissionDetail/getAllUsers',
        data: { userId: userId, missionId: missionId },
        success: (data) => {
            $('#recommendedPartial').html(data);
            $('#recommendedModal').modal('show');
           
            

            var sendRecommendationBtn = document.getElementById("sendRecommendationBtn");
            sendRecommendationBtn.addEventListener('click', () => {
                getCheckedUsersId();
                $('#recommendedModal').modal('hide');
                $.ajax({
                    type: "POST",
                    url: '/Users/MissionDetail/AddUsersToMissionInvite',
                    data: { usersIdList: usersId, missionId: missionId, currentUserId: userId },
                    success: (data) => {
                        

                    },
                    error: (err) => {

                    }

                })


            })

        },
        error: (err) => {
            
        }

    })
})


var usersId = [];
function getCheckedUsersId() {
    var checkedUser = 
        Array.from(document.querySelectorAll("#userChkBox:checked")).forEach(val => {

            var selectedUserId = $(val).val();
        usersId.push(selectedUserId);
        //console.log(userId);
        })
    
    
}

