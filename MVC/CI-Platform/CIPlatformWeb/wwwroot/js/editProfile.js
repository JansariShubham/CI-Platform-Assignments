//user change password

var passwordForm = document.getElementById("editPasswordForm");
  var userId = document.getElementById("userId").value;

passwordForm.addEventListener("submit", (e) => {
    e.preventDefault();

    if ($("#editPasswordForm").valid()) {

        var oldPassword = document.getElementById("oldPassword").value;
        var newPassword = document.getElementById("newPassword").value;
        var confirmPassword = document.getElementById("confirmPassword").value;

        if (newPassword != confirmPassword) {
            return;
        }

        $.ajax({
            url: "/Users/Home/ChangePassword",
            type: "POST",
            data: { userId: userId, oldPassword: oldPassword, newPassword: newPassword },
            success: function (result) {
                console.log("result =>>>" + result);
                if (result != "success") {

                    document.getElementById("oldPasswordSpan").textContent = "Please Enter Correct Password";
                }
                else {
                    document.getElementById("oldPassword").value = ""
                    document.getElementById("oldPasswordSpan").textContent = ""
                    document.getElementById("newPassword").value = "";
                    document.getElementById("confirmPassword").value = "";
                    $('#passwordModal').modal('hide');
                    Swal.fire(
                        'Password Updated Successfully!',
                        'You clicked the button!',
                        'success'
                    )

                }


            },
            error: function (err) {
                console.log("error in updating password");
            }


        });
    }
    else {
        return;
    }
})


document.getElementById("userImgDiv").addEventListener("click", () => {
    document.getElementById("userInputImg").click();


})
var userImg = document.getElementById("userImg");
var profile = [];
var userImgInput = document.getElementById("userInputImg");
userImgInput.addEventListener("change", () => {
    var userProfile = userImgInput.files[0];

    userImg.src = URL.createObjectURL(userProfile);
    //console.log("url==>>" + profileURL);


    var formData = new FormData();

    //console.log(userProfile + userId);
    formData.append('profile', userProfile);
    formData.append('userId', userId);
    console.log("formdata==>>>" + formData);

    $.ajax({
        type: "POST",
        url: '/Users/Home/UpdateUserProfile',
        data:  formData ,
        contentType: false,
        processData : false,
        success: (data) => {
            console.log("INNNNN");
            Swal.fire(
                'Profile Photo Updated Successfully!',
                'You clicked the button!',
                'success'
            )
            

        },
        error: (err) => {
            console.log("error in updating profile");
        }
    })
})

//Get Cities By Country
$(document).ready(function () {

    
    $("#country-filter").change(function () {
        
        var country = $(this).val();
        $.ajax({
            url: "/Users/Home/GetCitiesByCountry",
            type: "GET",
            
            data: { country: country },
            success: function (result) {

                var cityDropdown = $("#city-filter");
                cityDropdown.empty();
                
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

$(document).ready(() => {
    renderSkills();
})

var leftDiv;
var rightDiv;

var leftArrow;
var rightArrow;



var selectSkillsArr = [];
leftDiv = $("#leftDivSelect");


rightDiv = $("#rightDivSelect");

leftArrow = $("#leftArrow");
rightArrow = $("#rightArrow");



    //alert(leftDiv + rightDiv + leftArrow + rightArrow)


rightArrowClickFun();
leftArrowClickFun();

function rightArrowClickFun() {
    rightArrow.on('click', () => {
        let selectedSkills = $("#leftDivSelect option:selected");
    
        if (selectedSkills.length < 0) {
            Swal.fire(
                'Skill is empty!',
                'You clicked the button!',
                'error'
            )
        }
        else {
            addAndPreviewSelectedSkills(selectedSkills);
        }

    })
}
function addAndPreviewSelectedSkills(selectedSkills) {
    for (let skill of selectedSkills) {
       // console.log(skill);
        var currSkillId = +skill.value;
        if (selectSkillsArr.indexOf(currSkillId) === -1) {
            selectSkillsArr.push(currSkillId);

            previewSkills(skill);
        }
    }

}

function previewSkills(skill) {
    //alert("hello")

    rightDiv.append($(skill).clone());
}

function leftArrowClickFun() {
    
    leftArrow.on('click', () => {
        let selectedSkills = $("#rightDivSelect option:selected");
       // console.log(selectSkillsArr);
        //console.log(selectedSkills);
        if (selectedSkills.length < 0) {
            Swal.fire(
                'Skill is empty!',
                'You clicked the button!',
                'error'
            )
        }
        else {
            removeSkillsFromRightDiv(selectedSkills);
        }

    })
}

function removeSkillsFromRightDiv(selectedSkills) {
    for (let skill of selectedSkills) {
        // console.log("remove");
        // console.log(skill);
        var currSkillId = +skill.value;
      //  console.log(currSkillId)

        console.log(selectSkillsArr)
        var indexOfCurrSkillArr = selectSkillsArr.indexOf(currSkillId);
        console.log(indexOfCurrSkillArr);
        if (indexOfCurrSkillArr > -1) {
            selectSkillsArr.splice(indexOfCurrSkillArr, 1);
           // console.log(selectSkillsArr)
            $(skill).remove();

            document.querySelector(`#leftDivSelect option[value = "${currSkillId}"]`).selected = false;

            
        }
        
    }


}


function renderSkills() {

    let hiddenSkills = document.querySelectorAll(".hidden-skills");
    
    //console.log("Hekkki" );
   // console.log(hiddenSkills);

    if (hiddenSkills.length > 0) {
        for (let skill of hiddenSkills) {
            
            
   // console.log(skill);
            let currSkillId = +skill.value;
            selectSkillsArr.push(currSkillId);
         //   console.log(selectSkillsArr);
            document.querySelector(`#leftDivSelect option[value = "${currSkillId}"]`).selected = true;

            

        }
        let selectedUserSkill = $("#leftDivSelect option:selected");
        for (var userSkill of selectedUserSkill) {
         //   console.log(userSkill);
            previewSkills(userSkill);
            renderNameOfSkillInTextArea(userSkill.textContent);
        }
    }
}

let skillTextArea = document.getElementById("skillTextArea");

function renderNameOfSkillInTextArea(skillName) {
    skillTextArea.value += skillName + "\n";
   // console.log("skillTextArea ====>>> " + skillTextArea);
    

}

$('#saveSkillBtn').on('click', () => {
    let selectedSkills = $('#rightDivSelect option');
    skillTextArea.value = "";
    for (let skill of selectedSkills) {
        renderNameOfSkillInTextArea(skill.textContent);

    }
    $('#skillModal').modal('hide');
    console.log(selectSkillsArr);

})

var profileForm = document.getElementById("userProfileForm");

profileForm.addEventListener('submit', (e) => {
    e.preventDefault();
    if ($('#userProfileForm').valid()) {


        let rightDivSelectedSkills = $('#rightDivSelect option');
        let selectedSkillTextArea = skillTextArea.value.trim().split("\n");

        for (let i = 0; i < selectedSkillTextArea.length; i++) {
            for (let j = 0; j < rightDivSelectedSkills.length; j++) {
                let skillTextName = selectedSkillTextArea[i];
                let skillName = rightDivSelectedSkills[j].getAttribute('data-skillname');
                if (skillTextName == skillName) {
                    document.querySelector(`#rightDivSelect option[data-skillname = "${skillName}"]`).selected = true;
                }
            }
        }
        

        profileForm.submit();


    }
    else {
        return;
    }
})

