
var userId = document.getElementById("userId").value;

var goalModalBtn = document.getElementById("goalModal");
goalModalBtn.addEventListener('click', () => {
    $.ajax({
        url: "/Users/Home/GetGoalMission",
        type: "GET",

        data: { userId:userId },
        success: (result) => {
            $('##goalMissionModal').modal('show');
            console.log('success');
        },
        error: (err) => {
            console.log('error');
        }
    })

})

var timeModalBtn = document.getElementById("timeModal");
timeModalBtn.addEventListener('click', () => {
    $.ajax({
        url: "/Users/Home/GetTimeMission",
        type: "GET",

        data: { userId: userId },
        success: (result) => {
            $('#timeMissionModal').modal('show');
            console.log('success');
        },
        error: (err) => {
            console.log('error');

        }
    })

    })