//user change password

var passwordForm = document.getElementById("editPasswordForm");
  var userId = document.getElementById("userId").value;

passwordForm.addEventListener("submit", (e) => {
    e.preventDefault();
   
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
            console.log("result =>>>" +result);
            if (result != "success") {
                
                document.getElementById("oldPasswordSpan").textContent = "Please Enter Correct Password";
            }
            else {
                document.getElementById("oldPassword").value = ""
                document.getElementById("oldPasswordSpan").textContent = ""
                document.getElementById("newPassword").value = "";
                document.getElementById("confirmPassword").value = "";
                $('#passwordModal').modal('hide');
                

            }


        },
        error: function (err) {
            console.log("error in updating password");
        }


    });
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





