
var userId = document.getElementById("userId").value;
function goalTimeSheetAjax() {
    var userId = document.getElementById("userId").value;
    $.ajax({
        url: "/Users/Home/GetGoalMission",
        type: "GET",

        data: { userId: userId },
        success: (result) => {
            $('#goalModalPartial').html(result);
            $('#goalMissionModal').modal('show');
            addGoalTimeSheet();
            //dateValidation();

            console.log('success');
        },
        error: (err) => {
            console.log('error');
        }
    })

}

function hourTimeSheetAjax() {
    var userId = document.getElementById("userId").value;
    $.ajax({
        url: "/Users/Home/GetTimeMission",
        type: "GET",

        data: { userId: userId },
        success: (result) => {
            $('#hourModalPartial').html(result);
            $('#timeMissionModal').modal('show');
            addHourTimeSheet()
            //dateValidation();
            console.log('success');
        },
        error: (err) => {
            console.log('error');

        }
    })
}
$(document).ready(() => {
    goalModal();
    timeModal();
    deleteTimeSheetData();
    getEditGoalModal();
    getEditHourModal();
    
    
})
function goalModal() {
    var goalModalBtn = document.getElementById("goalModal");
    goalModalBtn.addEventListener('click', () => {
        goalTimeSheetAjax();
    })
}

function timeModal() {
    var timeModalBtn = document.getElementById("timeModal");
    timeModalBtn.addEventListener('click', () => {
        hourTimeSheetAjax();

    })
}

function isDateInRange(startDate, endDate, checkDate) {
    return moment(checkDate).isBetween(startDate, endDate, null, '[]');
}

function dateValidation() {
    const mission = document.getElementById("mission");
    mission.addEventListener('change', (e) => {
        const selectedMission = e.target.selectedOptions[0];
        const startDate = moment(selectedMission.dataset.startdate, 'DD-MM-YYYY HH:mm:ss');
        const endDate = moment(selectedMission.dataset.enddate, 'DD-MM-YYYY HH:mm:ss');
        console.log(startDate + " " + endDate);

        const checkDate = document.getElementById("dateVol");
        checkDate.addEventListener('change', (e) => {
            const selectedDate = e.target.value;
            console.log(selectedDate);
            const isValidDate = moment(selectedDate, 'YYYY-MM-DD', true).isValid();

            if (isValidDate && isDateInRange(startDate, endDate, selectedDate)) {
                console.log('The date is in range!');
                document.getElementById("dateError").textContent = ''; // Clear the error message
                 
            } else {
                document.getElementById("dateError").textContent = `Please select a date between ${startDate.format('DD-MM-YYYY')} date and ${endDate.format('DD-MM-YYYY')}.`;
                return;
            }
        });
    });
}

// Call the function to start the validation

function addHourTimeSheet() {
    var timeForm = document.getElementById("hourTimeSheetForm");
    timeForm.addEventListener("submit", (e) => {
        e.preventDefault();
        dateValidation();
        if ($("#hourTimeSheetForm").valid()) {



            var userId = document.getElementById("userId").value;
            var hours = document.getElementById("hour").value;
            var minutes = document.getElementById("minute").value;
            var message = document.getElementById("notes").value;
            var date = document.getElementById("dateVol").value;

            var mission = $('#mission :selected').val();
            

            $.ajax({
                url: '/Users/Home/AddHourTimeSheet',
                type: "POST",
                data: { userId: userId, hours: hours, message: message, minutes: minutes, date: date, missionId:mission },
                success: (result) => {
                   // console.log("success in addinng hour timesheet data")
                    $('#volTimeSheetPartial').html(result);
                    $("#timeMissionModal").modal('hide');
                    Swal.fire(
                        'Data Added Successfully!',
                        'You clicked the button!',
                        'success'
                    )
                    goalModal();
                    timeModal();
                    deleteTimeSheetData();
                    getEditGoalModal();
                    getEditHourModal();


                },
                error: (err) => {
                   // console.log("error in hour timesheet");
                    console.log(err);
                }
            })
        }
        else {
            return;
        }


    })
}


function addGoalTimeSheet() {
    var goalForm = document.getElementById("goalTimeSheetForm");
   goalForm.addEventListener("submit", (e) => {
        e.preventDefault();
        if ($("#goalTimeSheetForm").valid()) {

            var userId = document.getElementById("userId").value;
            var action = document.getElementById("action").value;
            
            var message = document.getElementById("message").value;
            var date = document.getElementById("date").value;

            var mission = $('#mission :selected').val();

           // alert("goaalll" + userId + action + message + date + mission);

            $.ajax({
                url: '/Users/Home/AddGoalTimeSheet',
                type: "POST",
                data: { userId: userId, message: message, action:action,  date: date, missionId: mission },
                success: (result) => {
                   // console.log("success in addinng goal timesheet data")
                    $('#volTimeSheetPartial').html(result);
                    $("#goalMissionModal").modal('hide');
                    
                    Swal.fire(
                        'Data Added Successfully!',
                        'You clicked the button!',
                        'success'
                    )
                    goalModal();
                    timeModal();
                    deleteTimeSheetData();
                    getEditGoalModal();
                    getEditHourModal();


                },
                error: (err) => {
                   // console.log("error in goal timesheet");
                    console.log(err);
                }
            })
        }
        else {
            return;
        }


    })
}
function deleteTimeSheetData() {
    var deleteTimeSheet = document.querySelectorAll(".delete-img");
    deleteTimeSheet.forEach((timeSheet) => {
        timeSheet.addEventListener("click", () => {
            var timeSheetId = timeSheet.getAttribute('data-timesheetid');

            Swal.fire({
                title: "Are you sure?",
                text: "Once deleted, you will not be able to recover this data",
                icon: "warning",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes",
                cancelButtonText: "Cancle",
                closeOnConfirm: false,
                closeOnCancel: false
            })
                .then((response) => {
                    if (response.isConfirmed) {

                        $.ajax({
                            url: '/Users/Home/deleteTimeSheet',
                            type: "POST",
                            data: { userId: userId, timeSheetId: timeSheetId },
                            success: (result) => {

                                $('#volTimeSheetPartial').html(result);
                                Swal.fire(
                                    'Data Deleted Successfully!',
                                    'You clicked the button!',
                                    'success'
                                )

                                goalModal();
                                timeModal();
                                deleteTimeSheetData();
                                getEditGoalModal();
                                getEditHourModal();



                            },
                            error: (err) => {

                                console.log(err);
                            }
                        })

                    }
                });


            //console.log("heeyyyy " + timeSheetId);

        })
    })
}

function getEditGoalModal() {

    var editGoalTimeSheetBtn = document.querySelectorAll(".edit-img");
    editGoalTimeSheetBtn.forEach((editTimeSheet) => {
        editTimeSheet.addEventListener("click", () => {
            var timeSheetId = editTimeSheet.getAttribute("data-timesheetid");

            $.ajax({
                url: '/Users/Home/getGoalTimeSheetData',
                type: "GET",
                data: { userId: userId, timeSheetId: timeSheetId },
                success: (result) => {

                    $('#editGoalModal').html(result);
                    $('#goalEditModal').modal('show');
                    updateGoalTimeSheet(timeSheetId);



                },
                error: (err) => {

                    console.log(err);
                }
            })
        })
    })

}

function getEditHourModal() {

    var editHourTimeSheetBtn = document.querySelectorAll(".edit1-img");
    editHourTimeSheetBtn.forEach((editTimeSheet) => {
        editTimeSheet.addEventListener("click", () => {
            var timeSheetId = editTimeSheet.getAttribute("data-timesheetid");

            $.ajax({
                url: '/Users/Home/getHourTimeSheetData',
                type: "GET",
                data: { userId: userId, timeSheetId: timeSheetId },
                success: (result) => {

                    $('#editTimeModal').html(result);
                    $('#timeEditModal').modal('show');
                    updateHourTimeSheet(timeSheetId);

                },
                error: (err) => {

                    console.log(err);
                }
            })
        })
    })
}

function updateHourTimeSheet(timeSheetId) {
    var timeForm = document.getElementById("hourTimeSheetEditForm");
    timeForm.addEventListener("submit", (e) => {
        e.preventDefault();
        if ($("#hourTimeSheetEditForm").valid()) {

            var userId = document.getElementById("userId").value;
            var hours = document.getElementById("hours").value;
            var minutes = document.getElementById("minutes").value;
            var message = document.getElementById("message").value;
            var date = document.getElementById("date").value;

            var mission = $('#mission :selected').val();
            //alert(timeSheetId);
            //alert(userId + hours + minutes + message + date + mission);

            $.ajax({
                url: '/Users/Home/UpdateHourTimeSheet',
                type: "POST",
                data: { userId: userId, hours: hours, message: message, minutes: minutes, date: date, missionId: mission, timeSheetId: timeSheetId },
                success: (result) => {
                    console.log("success in updating hour timesheet data")
                    $("#timeEditModal").modal('hide');
                    $('#volTimeSheetPartial').html(result);
                    Swal.fire(
                        'Data Updated Successfully!',
                        'You clicked the button!',
                        'success'
                    )
                    goalModal();
                    timeModal();
                    deleteTimeSheetData();
                    getEditGoalModal();
                    getEditHourModal();
                },
                error: (err) => {
                    console.log("error in hour timesheet");
                    console.log(err);
                }
            })
        }
        else {
            return;
        }


    })
}


function updateGoalTimeSheet(timeSheetId) {
    var timeForm = document.getElementById("goalTimeSheetEditForm");
    timeForm.addEventListener("submit", (e) => {
        e.preventDefault();
        if ($("#goalTimeSheetEditForm").valid()) {

            var userId = document.getElementById("userId").value;
            var action = document.getElementById("action").value;

            var message = document.getElementById("message").value;
            var date = document.getElementById("date").value;

            var mission = $('#mission :selected').val();

            $.ajax({
                url: '/Users/Home/UpdateGoalTimeSheet',
                type: "POST",
                data: { userId: userId, message: message, action: action, date: date, missionId: mission, timeSheetId: timeSheetId },
                success: (result) => {
                    console.log("success in addinng goal timesheet data")
                    $("#goalEditModal").modal('hide');
                    $('#volTimeSheetPartial').html(result);
                    Swal.fire(
                        'Data Updated Successfully!',
                        'You clicked the button!',
                        'success'
                    )
                    goalModal();
                    timeModal();
                    deleteTimeSheetData();
                    getEditGoalModal();
                    getEditHourModal();

                },
                error: (err) => {
                    console.log("error in goal timesheet");
                    console.log(err);
                }
            })
        }
        else {
            return;
        }


    })
}