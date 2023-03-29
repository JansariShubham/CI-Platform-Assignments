var storyId = document.getElementById("storyId").value;
var userId = document.getElementById("userId").value;
var missionId = document.getElementById("missionId").value;

if (userId != null && userId != "") {
    var recommendBtn = document.getElementById("recommendedBtn");
    recommendBtn.addEventListener("click", () => {

       // console.log("In ajx");
        $.ajax({
            url: '/Users/MissionDetail/GetAllUsers',
            type: "GET",
            data: { userId: userId, storyId: storyId },
            success: (data) => {
                $('#recommendedPartial').html(data);
                $('#recommendedModal').modal('show');

                var sendRecommendationBtn = document.getElementById("sendRecommendationBtn");
                sendRecommendationBtn.addEventListener('click', () => {
                    getCheckedUsersId();
                    $('#recommendedModal').modal('hide');
                    $.ajax({
                        type: "POST",
                        url: '/Users/Story/AddUsersToStoryInvite',
                        data: { usersIdList: usersId, storyId: storyId, currentUserId: userId },
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