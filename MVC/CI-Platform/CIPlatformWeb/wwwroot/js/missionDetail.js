

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

if (userId != null && userId != "") {
    var addToFavourite = document.getElementById("addToFavourite");

    //console.log("Outside..." + isClicked);

    //var isClicked = false;
    var isFavourite = document.getElementById("isFavourate").value; 
    addToFavourite.addEventListener("click", () => {
    
        if (isFavourite == 1) {


            $.ajax({
                url: "/Users/MissionDetail/RemoveFromFavourite",
                type: "POST",
                data: favMissionObj,
                success: function (result) {

                    favSpan.textContent = "Add to Favourite";
                    favImg.src = "/images/heart1.png";
                    //addToFavourite.disabled = true;
                   // isClicked = false;
                    console.log("Removed from fav");
                    isFavourite = 0;
                    
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
                   // isClicked = true;
                    console.log("added to fav");
                    isFavourite = 1;
                   
                },
                error: function (err) {
                    console.log("error");
                }


            })

        }
    })
}






var ratingsBtnClicked = true; 

$(document).ready(() => {
    
    getRelatedMission();

    let starInput = $("#star-input-id");
   // console.log(starInput);
    addStar(starInput, userId, missionId);

    displayRatings();
    
    getComments();

    getRecentVolunteers();


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

function displayRatings() {
    $.ajax({
        type: "GET",
        url: '/Users/MissionDetail/getRatings',
        data: {missionId: missionId},
        success: function (data) {
            
            console.log("ratingsss....");
            $("#ratingPartial").html(data);

        },
        error: (err) => {
            console.log("error in rating");
        }
    });
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
                    console.log("rating sufccess");
                    $("#ratingPartial").html(data);
                }
            });
        });
    }

if (userId != null && userId != "") {
    var commentMsg = document.getElementById("commentMsg").value;
    console.log(commentMsg);
    var commentsBtn = document.getElementById("commentBtn");

    console.log("btn")

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
            success: (data) => {
                // console.log("Comments Addded");
                alert("Comment Added Successfully");
                $("#partialComment").html(data)
                document.getElementById("commentMsg").value = '';
            },
            error: (err) => {
                console.log("Error in adding comment");
            }

        })
    })

}

//

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
if (userId != null && userId != "") {
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
}


var usersId = [];
function getCheckedUsersId() {
    var checkedUser = 
        Array.from(document.querySelectorAll("#userChkBox:checked")).forEach(val => {

            var selectedUserId = $(val).val();
        usersId.push(selectedUserId);
        //console.log(userId);
        })
    
    
}


function getRecentVolunteers() {
    
    let currentPage = 1;
        const recentVolunteers = document.querySelectorAll("[data-recent]");
    const totalVolunteers = recentVolunteers.length;
    if (totalVolunteers > 0) {
       const totalPages = Math.ceil(totalVolunteers / 2);
        //console.log(recentVolunteers.length);
        
        const rightPage = document.querySelector("#rightPage");
        const leftPage = document.querySelector("#leftPage");
        setPage(recentVolunteers);
        $(rightPage).click(() => {
            if (currentPage == totalPages) return;
            currentPage++;
            setPage(recentVolunteers);
        })
        $(leftPage).click(() => {
            if (currentPage == 1) return;
            currentPage--;
            setPage(recentVolunteers)
        })
    }
    function setPage(recentVolunteers) {
        let count = 0;
        Array.from($(recentVolunteers)).forEach((vol) => {
            const i = $(vol).data('recent');
            if (i >= (currentPage - 1)*  2 && i < currentPage * 2) {
            $(vol).removeClass("d-none");
            count++;
        } else {
            $(vol).addClass("d-none");
        }
    })
    const pageRange = document.querySelector("#pageRange");
    const start = (currentPage - 1) * 2 + 1;
    pageRange.textContent = `${start} - ${start + count - 1} of ${totalVolunteers} recent volunteers`
}
    /*var itemsPerPage = 1;
    var pageNumber = 1;
    const startIndex = (pageNumber - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;

    // Display the items in the current page
    for (let i = startIndex; i < endIndex && i < totalItems; i++) {
        dataContainer.children[i].style.display = "block";
    }*/
}

$('#applyBtn').click(() => {

    var userId = $('#userId').val();
    var missionId = $('#missionId').val();
    //alert(userId + missionId);
    $.ajax({
        type: "POST",
        url: '/Users/MissionDetail/AddMissionApplication',
        data: { missionId: missionId, userId: userId },
        success: (data) => {
            window.location.replace("/Users/MissionDetail/Index/5");
        },
        error: (err) => {

        }

    })
})

