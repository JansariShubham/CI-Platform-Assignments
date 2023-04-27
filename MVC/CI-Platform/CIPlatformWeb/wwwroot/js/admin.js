document.getElementById("sidebarBtn").addEventListener('click', () => {
            document.getElementById("sidebar").classList.toggle("d-none");

        })
        document.getElementById("closeSidebar").addEventListener('click', () => {
            document.getElementById("sidebar").classList.add("d-none");
        })


userAjaxCall();

function userAjaxCall() {
    $.ajax({
        type: "GET",
        url: '/Admin/Dashboard/getAllUsers',

        success: function (data) {

            console.log("success in getting users data");
            $("#adminPartial").html(data);
            showAddUserModal();
            editUserData();
            deleteUser();
            updateSidebarHeight();
            searchOperation();

            /*var userBtn = $('#userBtn');
            userBtn.addEventListener('click', () => {
                userBtn.style.backgroundColor = "red";
            })*/

            



        },
        error: (err) => {
            console.log("error in  getting users data");
        }
    });
}

function showAddUserModal() {
    
    var addUserBtn = document.getElementById("addUser");
    addUserBtn.addEventListener('click', () => {

        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getAddUserModal',

            success: function (data) {

                console.log("success in getting users modal");
                $("#addUserModalPartial").html(data);
                $('#addUserModal').modal('show');
                addUserData();
            },
            error: (err) => {
                console.log("error in  getting users modal");
            }
        });
       
    })
}

function addUserData() {
    var addUserForm = document.getElementById("addUserForm");
    addUserForm.addEventListener('submit', (e) => {
        e.preventDefault();
        if ($('#addUserForm').valid()) {

            var firstName = $('#firstName').val();
            var lastName = $('#lastName').val();
            var email = $("#email").val();
            var empId = $("#empId").val();
            var department = $("#department").val();
            var password = $("#password").val();

            console.log(firstName + lastName + email + empId + department);
            $('#addUserModal').modal('hide');
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/AddUser',
                data: {firstName : firstName, lastName : lastName, email : email , empId : empId, department : department, password: password},
                success: function (data) {
                    console.log("success adding user")
                    $("#adminPartial").html(data);
                    Swal.fire(
                        'User Added Successfully!',
                        'You clicked the button!',
                        'success'
                    )

                    showAddUserModal();
                    editUserData();
                    deleteUser();
                    searchOperation();

                },
                error: (err) => {
                    console.log("error in  getting users modal");
                }
            });


       }
        else {
            return;
        }
    })
}

function editUserData() {
    var editUserBtn = document.querySelectorAll(".edit-user");
    editUserBtn.forEach((user) => {
        user.addEventListener('click', () => {
            var userId = user.getAttribute("data-userid");
            //alert(userId);
            $.ajax({
                type: "GET",
                url: '/Admin/Dashboard/getUserDataByUserId',
                data: { userId: userId },
                success: function (data) {

                    //console.log("success in getting users modal");
                    $("#editUserModalPartial").html(data);
                    $('#editUserModal').modal('show');
                    editUser(userId);
                   
                },
                error: (err) => {
                    console.log("error in  getting users modal");
                }
            });
        })
    })
}

function editUser(userId) {
    var editUserForm = document.getElementById("editUserForm");
    editUserForm.addEventListener('submit', (e) => {
        e.preventDefault();
        if ($('#editUserForm').valid()) {

            var firstName = $('#firstName').val();
            var lastName = $('#lastName').val();
            var email = $("#email").val();
            var empId = $("#empId").val();
            var department = $("#department").val();
            var password = $("#password").val();
            
            console.log(firstName + lastName + email + empId + department);
            $('#editUserModal').modal('hide');
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/editUser',
                data: { firstName: firstName, lastName: lastName, email: email, empId: empId, department: department, password: password, userId: userId },
                success: function (data) {
                    console.log("success edit user")
                    $("#adminPartial").html(data);
                    editUserData();
                    deleteUser();
                    showAddUserModal();
                    searchOperation();
                    Swal.fire(
                        'User Updated Successfully!',
                        'You clicked the button!',
                        'success'
                    )

                },
                error: (err) => {
                    console.log("error in  getting users modal");
                }
            });


        }
        else {
            return;
        }
    })
}

function deleteUser() {
    var deleteBtn = document.querySelectorAll(".delete-user");
    deleteBtn.forEach((delBtn) => {
        delBtn.addEventListener('click', () => {
            var userId = delBtn.getAttribute("data-userid");
            var status = delBtn.getAttribute("data-status");
            if (status == 0) {
                Swal.fire({
                    title: "Are you sure?",
                    text: "",
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
                                type: "POST",
                                url: '/Admin/Dashboard/changeUserStatus',
                                data: { userId: userId, status: status },
                                success: function (data) {

                                    $("#adminpartial").html(data);
                                    deleteUser();
                                    editUserData();
                                    showAddUserModal();
                                    searchOperation();
                                    userAjaxCall();
                                },
                                error: (err) => {
                                    console.log("error in  getting users modal");
                                }
                            });
                        }
                    });
            }
                        
            else {
                $.ajax({
                    type: "POST",
                    url: '/Admin/Dashboard/changeUserStatus',
                    data: { userId: userId, status: status },
                    success: function (data) {

                        $("#adminPartial").html(data);
                        deleteUser();
                        editUserData();
                        showAddUserModal();
                        searchOperation();
                        userAjaxCall();
                    },
                    error: (err) => {
                        console.log("error in  getting users modal");
                    }
                });
            }
            
        })
      })
}


var sidebar = document.getElementById("sidebar");
var rightDiv = document.getElementById("rightDiv");


const resizeObserver = new ResizeObserver(entries => {
    for (let entry of entries) {
        updateSidebarHeight();
    }
});

// Start observing the element
resizeObserver.observe(rightDiv);

function updateSidebarHeight() {
    // Get the height of the content
    const contentHeight = rightDiv.offsetHeight;
   // console.log(vhToPixels(100), contentHeight);
    let height = (vhToPixels(100) > contentHeight) ? vhToPixels(100) : contentHeight;
    sidebar.style.height = `${height}px`;
}

function vhToPixels(vh) {
    return Math.round(window.innerHeight / (100 / vh));
}
var searchText;

function searchOperation() {
    var search = document.getElementById("searchInput");
    search.addEventListener('input', () => {
         searchText = search.value;
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getSearchedUsers',
            data: { searchText: searchText },
            success: function (data) {

                $("#adminPartial").html(data);
                deleteUser();
                editUserData();
                searchOperation();
                showAddUserModal();
                search = document.getElementById("searchInput");
                search.focus();
                search.value = searchText;
            },
            error: (err) => {
                console.log("error in  getting users modal");
            }
        });
       
    })
}


var cmsPageBtn = document.getElementById('cmsPageBtn');
cmsPageBtn.addEventListener('click', () => {

    cmsAjax();
})

function cmsAjax() {
    tinymce.remove('#tiny');
    $.ajax({
        type: "GET",
        url: '/Admin/Dashboard/getCmsDetails',

        success: function (data) {

            console.log("success in getting cms");
            $("#adminPartial").html(data);

            addCms();
            deleteCms();
            editCms();
            searchCms();

        },
        error: (err) => {
            console.log("error in  getting cms data");
        }
    });
}
var userBtn = document.getElementById("userBtn");
userBtn.addEventListener('click', () => {
    
    userAjaxCall();
})




function addCms() {
    var addCmsBtn = document.getElementById('addCmsBtn');
    addCmsBtn.addEventListener('click', () => {
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/cmsAddPage',

            success: function (data) {

                console.log("success in getting adding cms");
                $("#adminPartial").html(data);
                $.getScript('/js/tinymce.js');
                
                addCmsData();

            },
            error: (err) => {
                console.log("error in  getting cms adding data");
            }
        });
    })
}

function addCmsData() {
    var addCmsForm = document.getElementById('addCmsForm');
    addCmsForm.addEventListener('submit', (e) => {
        e.preventDefault();

        var tinyTextArea = tinymce.get("tiny").getContent();
        var title = document.getElementById("title").value;
        var slug = document.getElementById("slug").value;
        //var status = document.getElementById("status").value;
        let status = $("#status option:selected").val();
        
        if (tinyTextArea === "" || tinyTextArea === null) {
            descSpan.innerHTML = "CMS description is required!";
            return;

        }

        const cmsPageDetails = {
            desc: tinyTextArea,
            title: title,
            slug: slug,
            status : status
        };


        if ($('#addCmsForm').valid()) {
            
            //var obj = $('#addCmsForm').serialize();
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/AddCmsDetails',
                data: cmsPageDetails,
                success: function (data) {

                    console.log("success in adding cms");
                    //$("#adminPartial").html(data);

                    
                    cmsAjax();

                    Swal.fire(
                        'CMS Page Added Successfully!',
                        'You clicked the button!',
                        'success'
                    )
                    

                },
                error: (err) => {
                    console.log("error in  getting cms data");
                }
            });
            
        }
        else {
            return;
        }
    })
}

function deleteCms() {

    var deleteCms = document.querySelectorAll(".delete-cms");

    deleteCms.forEach((deleteBtn) => {
        deleteBtn.addEventListener('click', () => {
            var cmsId = deleteBtn.getAttribute("data-cmsid");
            var status = deleteBtn.getAttribute("data-status");
           
            if (status == "False") {
               
                status = 0;
            }
            else {
              
                status = 1;
            }
            //alert(status);
            //alert(cmsId);
            if (status == 1) {
                
                $.ajax({
                    type: "POST",
                    url: '/Admin/Dashboard/changeCmsStatus',
                    data: { cmsId: cmsId, status: status },
                    success: function (data) {

                        $("#adminPartial").html(data);
                        cmsAjax();

                    },
                    error: (err) => {
                        console.log("error in  getting cms modal");
                    }
                });
            }
            else {
                
                Swal.fire({
                    title: "Are you sure?",
                    text: "",
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
                                type: "POST",
                                url: '/Admin/Dashboard/changeCmsStatus',
                                data: { cmsId: cmsId, status: status },
                                success: function (data) {

                                    $("#adminPartial").html(data);
                                    cmsAjax();

                                },
                                error: (err) => {
                                    console.log("error in  getting cms modal");
                                }
                            });
                        }
                    });

                

            }

        })
    })
}



function editCms() {

    var editCms = document.querySelectorAll(".edit-cms");
    editCms.forEach((editBtn) => {
        var cmsId = editBtn.getAttribute("data-cmsid");

        /*var cmsStatus = editBtn.getAttribute("data-status");*/
        editBtn.addEventListener('click', () => {
         
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getCmsEditDetails',
            data: { cmsId: cmsId },
            success: function (data) {

                $("#adminPartial").html(data);
                $.getScript('/js/tinymce.js');

                editCmsDetails(cmsId);

            },
            error: (err) => {
                console.log("error in  getting cms modal");
            }
        });

    })
        })

}

function editCmsDetails(cmsId) {
    var cmsEditForm = document.getElementById("editCmsForm");
    cmsEditForm.addEventListener("submit", (e) => {
        e.preventDefault();
        if (tinyTextArea === "" || tinyTextArea === null) {
            descSpan.innerHTML = "CMS description is required!";
            return;
        }

        if ($('#editCmsForm').valid()) {
            var tinyTextArea = tinymce.get("tiny").getContent();
            var title = document.getElementById("title").value;
            var slug = document.getElementById("slug").value;

            let status = $("#status option:selected").val();

            

            const cmsPageDetails = {
                desc: tinyTextArea,
                title: title,
                slug: slug,
                status: status,
                cmsId: cmsId
            };

            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/editCmsDetails',
                data: cmsPageDetails,
                success: function (data) {

                    console.log("success in adding cms");
                    //$("#adminPartial").html(data);


                    cmsAjax();

                    Swal.fire(
                        'CMS Page Added Successfully!',
                        'You clicked the button!',
                        'success'
                    )


                },
                error: (err) => {
                    console.log("error in editing cms data");
                }
            });

        }
        else {
            return;
        }
    })
}



function searchCms() {
    var search = document.getElementById("searchInput");
    search.addEventListener('input', () => {
        searchText = search.value;
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getSearchedCms',
            data: { searchText: searchText },
            success: function (data) {

                $("#adminPartial").html(data);
                
                search = document.getElementById("searchInput");
                search.focus();
                search.value = searchText;
                searchCms();
                addCms();
                editCms();
                deleteCms();
            },
            error: (err) => {
                console.log("error in  getting users modal");
            }
        });

    })
}


var missionThemeBtn = document.getElementById("missionThemeBtn");
missionThemeBtn.addEventListener("click", () => {
    missionThemeAjax();
   
})

function missionThemeAjax() {
    $.ajax({
        type: "GET",
        url: '/Admin/Dashboard/getMissionThemeList',

        success: function (data) {

            $("#adminPartial").html(data);
            getAddThemeDetailsForm();
            deleteTheme();
            editTheme();
            searchMissionThemes();


        },
        error: (err) => {
            console.log("error in  getting theme list");
        }
    });
}

function getAddThemeDetailsForm() {
    var addThemeBtn = document.getElementById("addThemeBtn");
    addThemeBtn.addEventListener('click', () => {
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getAddThemeDetails',

            success: function (data) {

                $("#adminPartial").html(data);
                addMissionTheme();


            },
            error: (err) => {
                console.log("error in  getting theme list");
            }
        });
    })
}
function addMissionTheme() {
    var addMissionThemeForm = document.getElementById("addMissionThemeForm");

    addMissionThemeForm.addEventListener('submit', (e) => {
        e.preventDefault();
        if ($('#addMissionThemeForm').valid()) {
            var themeObj = $('#addMissionThemeForm').serialize();
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/AddMissionTheme',
                data: themeObj,
                success: function (data) {
                    missionThemeAjax();


                },
                error: (err) => {
                    console.log("error in  getting theme list");
                }
            });

        }
        else {
            return;
        }
    })
}

function deleteTheme() {
    var deleteThemeBtn = document.querySelectorAll(".delete-cms");
    deleteThemeBtn.forEach((deleteTheme) => {
        var missionThemeId = deleteTheme.getAttribute("data-themeid");
        var missionThemeStatus = deleteTheme.getAttribute("data-status");

        deleteTheme.addEventListener('click', () => {
            // alert(missionThemeStatus + " " + missionThemeId)
            if (missionThemeStatus == 1) {
               
                Swal.fire({
                    title: "Are you sure?",
                    text: "",
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
                                type: "POST",
                                url: '/Admin/Dashboard/ChangeMissionThemeStatus',
                                data: {
                                    missionThemeId: missionThemeId,
                                    missionThemeStatus: missionThemeStatus
                                },
                                success: function (data) {
                                    missionThemeAjax();


                                },
                                error: (err) => {
                                    console.log("error in  getting theme list");
                                }
                            });
                        }
                    });
            }
            else {
                $.ajax({
                    type: "POST",
                    url: '/Admin/Dashboard/ChangeMissionThemeStatus',
                    data: {
                        missionThemeId: missionThemeId,
                        missionThemeStatus: missionThemeStatus
                    },
                    success: function (data) {
                        missionThemeAjax();


                    },
                    error: (err) => {
                        console.log("error in  getting theme list");
                    }
                });
            }
           
        })

    })
}

function editTheme() {
    var editCms = document.querySelectorAll(".edit-cms");
    editCms.forEach((editTheme) => {
        var themeId = editTheme.getAttribute("data-themeid");
        editTheme.addEventListener('click', () => {
            $.ajax({
                type: "GET",
                url: '/Admin/Dashboard/GetThemeEditForm',
                data: {themeId : themeId},
                success: function (data) {
                    $('#adminPartial').html(data);
                    editMissionTheme(themeId);


                },
                error: (err) => {
                    console.log("error in  getting theme list");
                }
            });
        })
    })
}

function editMissionTheme(themeId) {
    var editThemeForm = document.getElementById("editMissionThemeForm");
    editThemeForm.addEventListener('submit', (e) => {
        e.preventDefault();
        if ($('#editMissionThemeForm').valid()) {
           // var themeObj = $('#editMissionThemeForm').serialize();
            //console.log(themeObj);

            var title = $('#title').val();
            var status = $('#status').val();
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/EditThemeData',
                data: {
                    title: title,status: status, themeId: themeId
                },
                success: function (data) {

                    missionThemeAjax();


                },
                error: (err) => {
                    console.log("error in  getting theme list");
                }
            });

        }
        else {
            return;
        }
    })
}



function searchMissionThemes() {
    var search = document.getElementById("searchInput");
    search.addEventListener('input', () => {
        searchText = search.value;
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getSearchedThemes',
            data: { searchText: searchText },
            success: function (data) {

                $("#adminPartial").html(data);
                searchMissionThemes();
                search = document.getElementById("searchInput");
                search.focus();
                search.value = searchText;
                getAddThemeDetailsForm();
                editTheme();
                deleteTheme();

            },
            error: (err) => {
                console.log("error in  getting searched theme list");
            }
        });

    })
}


var missionSkillBtn = document.getElementById("missionSkillBtn");
missionSkillBtn.addEventListener('click', () => {
    getSkillList();
    
})

function getSkillList() {
    $.ajax({
        type: "GET",
        url: '/Admin/Dashboard/GetSkillList',

        success: function (data) {

            $('#adminPartial').html(data);
            getAddSkillForm();
            deleteSkill();
            getEditSkillForm();
            searchSkill();
        },
        error: (err) => {
            console.log("error in  getting skill list");
        }
    });
}

function getAddSkillForm() {
    $('#addSkillBtn').click(() => {
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/GetAddSkillForm',

            success: function (data) {

                $('#adminPartial').html(data);
                addSkill();

            },
            error: (err) => {
                console.log("error in  getting skill form");
            }
        });
    })
    
}

function addSkill() {
    $('#addSkillForm').submit((e) => {
        e.preventDefault();
        if ($('#addSkillForm').valid()) {
            var skillName = $('#addSkillForm').serialize();
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/AddSkill',
                data: skillName,
                success: function (data) {
                    getSkillList();

                },
                error: (err) => {
                    console.log("error in  getting skill form");
                }
            });
        }
        else {
            return;
        }
    })
}
function deleteSkill() {
    var deleteSkill = document.querySelectorAll(".delete-skill");
    deleteSkill.forEach((deleteBtn) => {
        var skillId = deleteBtn.getAttribute("data-skillid");
        var status = deleteBtn.getAttribute("data-status");
        deleteBtn.addEventListener('click', () => {
            if (status == "True") {
                Swal.fire({
                    title: "Are you sure?",
                    text: "",
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
                                type: "POST",
                                url: '/Admin/Dashboard/ChangeSkillStatus',
                                data: { status: status, skillId: skillId },
                                success: function (data) {
                                    $('#adminPartial').html(data);

                                    getSkillList();


                                },
                                error: (err) => {
                                    console.log("error in skill delete");
                                }
                            });
                        }
                    });

            }
            else {
                $.ajax({
                    type: "POST",
                    url: '/Admin/Dashboard/ChangeSkillStatus',
                    data: { status: status, skillId: skillId },
                    success: function (data) {
                        $('#adminPartial').html(data);

                        getSkillList();


                    },
                    error: (err) => {
                        console.log("error in skill delete");
                    }
                });
            }
        })

    })
}

function getEditSkillForm() {
    var editSkill = document.querySelectorAll(".edit-skill");
    editSkill.forEach((editSkillBtn) => {
        var skillId = editSkillBtn.getAttribute("data-skillid");
        editSkillBtn.addEventListener("click", () => {
            $.ajax({
                type: "GET",
                url: '/Admin/Dashboard/GetSkillEditForm',
                data: {skillId: skillId},
                success: function (data) {
                    $('#adminPartial').html(data);
                    editSkillData(skillId);
                    //getSkillList();


                },
                error: (err) => {
                    console.log("error in skill delete");
                }
            });
        })
    })
} 

function editSkillData(skillId) {
   
    $('#editSkillForm').submit((e) => {
        e.preventDefault();
        if ($('#editSkillForm').valid()) {
            var skillName = $('#skillName').val();
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/EditSkill',
                data: { skillId: skillId , skillName: skillName},
                success: function (data) {
                    //$('#adminPartial').html(data);
                   
                    getSkillList();

                    Swal.fire(
                        'CMS Page Added Successfully!',
                        'You clicked the button!',
                        'success'
                    )
                },
                error: (err) => {
                    console.log("error in skill delete");
                }
            });
        }
        else {
            return;
        }
    })
}

function searchSkill() {
    var search = document.getElementById("searchInput");
    search.addEventListener('input', () => {
        searchText = search.value;
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getSearchedSkills',
            data: { searchText: searchText },
            success: function (data) {

                $("#adminPartial").html(data);
                deleteSkill();
                getEditSkillForm();
                searchSkill();
                getAddSkillForm();
                search = document.getElementById("searchInput");
                search.focus();
                search.value = searchText;
            },
            error: (err) => {
                console.log("error in  getting users modal");
            }
        });
    });
    }


$("#missionApplicationBtn").click(() => {
    missionAppAjax();

})

function missionAppAjax() {
    $.ajax({
        type: "GET",
        url: '/Admin/Dashboard/MissionApplicationList',
       
        success: function (data) {
            $('#adminPartial').html(data);
            approveStatus();
            declineStatus();
            searchMissions();
        },
        error: (err) => {
            console.log("error in getting missionapp list");
        }
    });
}

function approveStatus() {
    var approvedBtn = document.querySelectorAll(".approved-btn");
    approvedBtn.forEach((approveButton) => {
        var missionAppId = approveButton.getAttribute("data-applicationid");
        approveButton.addEventListener('click', () => {
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/ApproveStatus',
                data: { missionAppId: missionAppId },
                success: function (data) {
                    missionAppAjax();
                   

                },
                error: (err) => {
                    console.log("error in getting missionapp list");
                }
            });

        })
    })
}


function declineStatus() {
    var declineBtn = document.querySelectorAll(".decline-btn");
    declineBtn.forEach((declineButton) => {
        var missionAppId = declineButton.getAttribute("data-applicationid");
        declineButton.addEventListener('click', () => {
            Swal.fire({
                title: "Are you sure?",
                text: "",
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
                            type: "POST",
                            url: '/Admin/Dashboard/DeclineStatus',
                            data: { missionAppId: missionAppId },
                            success: function (data) {
                                missionAppAjax();


                            },
                            error: (err) => {
                                console.log("error in getting missionapp list");
                            }
                        });  
                    }
                });

           

        })
    })
}


function searchMissions() {
    var search = document.getElementById("searchInput");
    search.addEventListener('input', () => {
        searchText = search.value;
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getSearchedMissions',
            data: { searchText: searchText },
            success: function (data) {

                $("#adminPartial").html(data);
                approveStatus();
                declineStatus();
                searchMissions();
                search = document.getElementById("searchInput");
                search.focus();
                search.value = searchText;
            },
            error: (err) => {
                console.log("error in  getting users modal");
            }
        });
    });
}


$('#storyBtn').click( () => {
    storyAjax();
})

function storyAjax() {
    $.ajax({
        type: "GET",
        url: '/Admin/Dashboard/getStoryList',
       
        success: function (data) {
            $('#adminPartial').html(data);
            searchStories();
            approveStory();
            restoreStory();
            declineStory();
            deleteStory();
        },
        error: (err) => {
            console.log("error in getting missionapp list");
        }
    });
}

function searchStories() {
    var search = document.getElementById("searchInput");
    search.addEventListener('input', () => {
        searchText = search.value;
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getSearchedStories',
            data: { searchText: searchText },
            success: function (data) {

                $("#adminPartial").html(data);
               
                searchStories();
                approveStory();
                declineStory();
                restoreStory();
                deleteStory();
                search = document.getElementById("searchInput");
                search.focus();
                search.value = searchText;
            },
            error: (err) => {
                console.log("error in  getting users modal");
            }
        });
    });
}

function approveStory() {
    var approvedBtn = document.querySelectorAll(".approved-btn");
    approvedBtn.forEach((approveButton) => {
        var storyId = approveButton.getAttribute("data-storyid");
        approveButton.addEventListener('click', () => {
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/ApproveStory',
                data: { storyId : storyId },
                success: function (data) {
                    storyAjax();


                },
                error: (err) => {
                    console.log("error in getting missionapp list");
                }
            });

        })
    })
}


function restoreStory() {
 
    var restoreStory = document.querySelectorAll(".restore-btn");
    restoreStory.forEach((restoreBtn) => {
        //alert("hello")
        var storyId = restoreBtn.getAttribute("data-storyid");
        
        restoreBtn.addEventListener('click', () => {
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/ApproveStory',
                data: { storyId: storyId },
                success: function (data) {
                    storyAjax();


                },
                error: (err) => {
                    console.log("error in getting missionapp list");
                }
            });
        })
    })
}
function declineStory() {
    var declineStories = document.querySelectorAll(".decline-btn");
    declineStories.forEach((declineBtn) => {
        var storyId = declineBtn.getAttribute("data-storyid");
        
        declineBtn.addEventListener('click', () => {
    
            Swal.fire({
                title: "Are you sure?",
                text: "You want to decline",
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
                            type: "POST",
                            url: '/Admin/Dashboard/DeclineStory',
                            data: { storyId: storyId },
                            success: function (data) {
                                storyAjax();


                            },
                            error: (err) => {
                                console.log("error in getting missionapp list");
                            }
                        });
                    }
                });


           
        })
    })
}

function deleteStory() {
    var deleteStory = document.querySelectorAll(".delete-story");
    deleteStory.forEach((deleteBtn) => {
        
        var storyId = deleteBtn.getAttribute("data-storyid");
        deleteBtn.addEventListener('click', () => {
            Swal.fire({
                title: "Are you sure?",
                text: "You want to decline",
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
                            type: "POST",
                            url: '/Admin/Dashboard/DeleteStory',
                            data: { storyId: storyId },
                            success: function (data) {
                                storyAjax();


                            },
                            error: (err) => {
                                console.log("error in getting missionapp list");
                            }
                        });
                    }
                });
        });
       
    })
}


$('#bannerBtn').click(() => {
    
    bannerAjax();
})

function bannerAjax() {
    $.ajax({
        type: "GET",
        url: '/Admin/Dashboard/getBannerList',

        success: function (data) {
            $('#adminPartial').html(data);
            getAddBannerForm();
            deleteBanner();
            getEditBannerForm();
            
        },
        error: (err) => {
            console.log("error in getting banner list");
        }
    });
}

function getAddBannerForm() {
    //alert("hello");
    $('#addBannerBtn').click(() => {
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getBannerAddForm',

            success: function (data) {
                $('#adminPartial').html(data);
                previewImage();
                addBanner();
            },
            error: (err) => {
                console.log("error in getting banner form");
            }
        });
    })
}

function previewImage() {
   
    var bannerImageInput = document.getElementById("bannerImage");
    var previewImg = document.getElementById("preview");
    //var bannerImageInput = $('#bannerImage');

    $('#bannerImage').change(() => {

        var bannerImage = bannerImageInput.files[0];
        //console.log(bannerImage);

        previewImg.src = URL.createObjectURL(bannerImage);
    })

}

function addBanner() {
    $('#addBannerForm').submit((e) => {
        e.preventDefault();
        var bannerImageInput = document.getElementById("bannerImage");
        if (bannerImageInput.files.length == 0) {
            $('#imageError').text("Image is Required!");
            return;
        }
        if ($('#addBannerForm').valid()) {
           
            var bannerImage = bannerImageInput.files[0];
            //console.log(bannerImage);
            var formData = new FormData($('#addBannerForm')[0]);
            formData.append('image', bannerImage);
            console.log(formData)
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/AddBannerDetails',
                data: formData,
                contentType: false,
                processData: false,
                success: (data) => {
                    bannerAjax();
                    Swal.fire(
                        'Banner Added Successfully!',
                        'You clicked the button!',
                        'success'
                    )


                },
                error: (err) => {
                    console.log("error in adding ");
                }
            })
        }
        else {
            return;
        }
    })
}

function deleteBanner() {
    var deleteBanner = document.querySelectorAll(".delete-banner");
    deleteBanner.forEach((deleteBtn) => {
        var bannerId = deleteBtn.getAttribute("data-bannerid");
        var status = deleteBtn.getAttribute("data-status");
       
        deleteBtn.addEventListener('click', () => {
            if (status == "True") {
                Swal.fire({
                    title: "Are you sure?",
                    text: "You want to decline",
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
                                type: "POST",
                                url: '/Admin/Dashboard/ChangeBannerStatus',
                                data: { bannerId: bannerId, status: status },
                                success: function (data) {
                                    bannerAjax();
                                },
                                error: (err) => {
                                    console.log("error in getting banner form");
                                }
                            });
                            
                        }
                    });
            }
            else {


                $.ajax({
                    type: "POST",
                    url: '/Admin/Dashboard/ChangeBannerStatus',
                    data: { bannerId: bannerId, status: status },
                    success: function (data) {
                        bannerAjax();
                    },
                    error: (err) => {
                        console.log("error in getting banner form");
                    }
                });
            }
        })
    })
}

function getEditBannerForm() {
    var editBanner = document.querySelectorAll(".edit-banner");
    editBanner.forEach((editBtn) => {
        var bannerId = editBtn.getAttribute("data-bannerid");
        editBtn.addEventListener('click', () => {
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/getEditBannerForm',
                data: { bannerId: bannerId },
                success: function (data) {
                   
                    $("#adminPartial").html(data);
                    getBannerImage();
                    previewImage();
                    editBannerDetails(bannerId);
                },
                error: (err) => {
                    console.log("error in getting banner form");
                }
            });
        })
    })
}

function getBannerImage() {
    var existingImg = $('#bannerImg');
    var fileName = existingImg.data("name");
    var type = existingImg.data("type");

    fetch(existingImg.val())
        .then(response => response.arrayBuffer())
        .then(buffer => {
            const myFile = new File([buffer], fileName, { type: `image/${type}` });

            let myFileList = new DataTransfer();
            myFileList.items.add(myFile);
            document.querySelector('#bannerImage').files = myFileList.files;

        });
       
}


function editBannerDetails(bannerId) {

    $('#bannerForm').submit((e) => {
        e.preventDefault();
        var bannerImageInput = document.getElementById("bannerImage");
        if (bannerImageInput.files.length == 0) {
            $('#imageError').text("Image is Required!");
            return;
        }
        if ($('#bannerForm').valid()) {

            var bannerImage = bannerImageInput.files[0];
            console.log( bannerImage);
            
            var formData = new FormData($('#bannerForm')[0]);
            formData.append('image', bannerImage);
            formData.append('bannerId', bannerId);
            //console.log(formData)
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/EditBannerDetails',
                data: formData,
                contentType: false,
                processData: false,
                success: (data) => {
                    bannerAjax();
                    Swal.fire(
                        'Banner Updated Successfully!',
                        'You clicked the button!',
                        'success'
                    )


                },
                error: (err) => {
                    console.log("error in updating ");
                }
            })
        }
        else {
            return;
        }
    })

}


$('#missionBtn').click(() => {
    missionAjax();
})

function missionAjax() {
    tinymce.remove('#tiny');
    $.ajax({
        type: "GET",
        url: '/Admin/Dashboard/getMissionList',

        success: function (data) {
            $('#adminPartial').html(data);
            
            searchMissions();
            deleteMission();
            getAddTimeMissionForm();
            getAddGoalMissionForm();
            getEditMissionForm();
        },
        error: (err) => {
            console.log("error in getting mission list");
        }
    });
}

function searchMissions() {
    var search = document.getElementById("searchInput");
    search.addEventListener('input', () => {
        searchText = search.value;
        $.ajax({
            type: "GET",
            url: '/Admin/Dashboard/getSearchedMission',
            data: { searchText: searchText },
            success: function (data) {

                $("#adminPartial").html(data);
                
                search = document.getElementById("searchInput");
                search.focus();
                search.value = searchText;
                searchMissions();
                deleteMission();
            },
            error: (err) => {
                console.log("error in  getting users modal");
            }
        });

    })
}


function deleteMission() {
    
    var deleteMission = document.querySelectorAll(".delete-mission");
    deleteMission.forEach((deleteBtn) => {
        var missionId = deleteBtn.getAttribute("data-missionid");
        var status = deleteBtn.getAttribute("data-status");
        deleteBtn.addEventListener('click', () => {
            
            if (status == "True") {
                Swal.fire({
                    title: "Are you sure?",
                    text: "You want to In active this mission?",
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
                                type: "POST",
                                url: '/Admin/Dashboard/ChangeMissionStatus',
                                data: { missionId: missionId, status: status },
                                success: function (data) {
                                    missionAjax();
                                },
                                error: (err) => {
                                    console.log("error in getting banner form");
                                }
                            });
                        }
                    });
            }
            else {
                $.ajax({
                    type: "POST",
                    url: '/Admin/Dashboard/ChangeMissionStatus',
                    data: { missionId: missionId, status: status },
                    success: function (data) {
                        missionAjax();
                    },
                    error: (err) => {
                        console.log("error in getting banner form");
                    }
                });

                
            }
        })
    })
}


function getAddTimeMissionForm() {
    $('#addTimeBtn').click(() => {
        $.ajax({
            type: "POST",
            url: '/Admin/Dashboard/getAddTimeMissionForm',
            
            success: function (data) {
                $('#adminPartial').html(data);
                $.getScript('/js/tinymce.js');
                console.log("succ");
                getCitiesByCountry();
                fileClick();
                displayMissionImages();
                previewDocuments();
                addTimeMission();
            },
            error: (err) => {
                console.log("error in getting banner form");
            }
        });
    })
}


function getAddGoalMissionForm() {
    $('#addGoalBtn').click(() => {
        $.ajax({
            type: "POST",
            url: '/Admin/Dashboard/getAddGoalMissionForm',

            success: function (data) {
                $('#adminPartial').html(data);
                $.getScript('/js/tinymce.js');
                getCitiesByCountry();
                console.log("succ");
                fileClick();
                displayMissionImages();
                previewDocuments();
                addGoalMission();
            },
            error: (err) => {
                console.log("error in getting banner form");
            }
        });
    })
}


function fileClick() {
    var fileInput = document.getElementById("ImagesInput");
    document.querySelector(".dragarea").addEventListener('click', () => {
        fileInput.click();
    })
}



//Get Cities By Country
function getCitiesByCountry() {

    $("#countryDropdown").change(function () {
        var country = $(this).val();
        $.ajax({
            url: "/Users/Home/GetCitiesByCountry",
            type: "GET",
            dataType: "json",
            data: { country: country },
            success: function (result) {

                var cityDropdown = $("#cityDropdown");
                cityDropdown.empty();
                
                const cityFilter = document.querySelector("#cityDropdown");
                cityFilter.innerHTML = `<option value="1" selected disabled>City</option>`;
                result.forEach((c) => {
                    cityFilter.innerHTML += `
                    <option value=${c.cityId}>${c.name}</option>
                        `
                })
            }
        });
    });
}

var images = [];


function displayMissionImages() {
    document.getElementById("ImagesInput").addEventListener("change", () => {
        var files = document.getElementById("ImagesInput").files;

        for (var i = 0; i < files.length; i++) {

            images.push(files[i]);
        }
        displayImages();

        //console.log(images);
    })
    var dragDiv = document.querySelector(".dragarea");


    dragDiv.addEventListener("dragover", (e) => {
        e.preventDefault();

    })

    dragDiv.addEventListener("drop", (e) => {
        e.preventDefault();

        var image = e.dataTransfer.files;

        for (var i = 0; i < image.length; i++) {

            images.push(image[i]);
        }
        displayImages();

    })


    dragDiv.addEventListener("dragenter", (e) => {
        e.preventDefault();
    })
}

function displayImages() {
    document.querySelector(".selected-images").innerHTML = '';
    for (var i = 0; i < images.length; i++) {
        document.querySelector(".selected-images").innerHTML += `<div class = 'position-relative'> <img class = 'drag-img' src= ${URL.createObjectURL(images[i])} alt="">
                    <a class = "btn delete-btn border-0" onclick = removeImg(${i})>
                    <img class="cancle-image position-absolute" src='/images/cancel1.png'
                        alt="">
                    </a> </div>`;
    }
}

function removeImg(index) {
    images.splice(index, 1);
    displayImages();
}

function setImageInput() {
    var fileId = document.getElementById("ImagesInput");
    let myFileList = new DataTransfer();
    images.forEach(function (file) {
        myFileList.items.add(file);
        //console.log("hello" + file);
    });
    fileId.files = myFileList.files;
}


function setDocInput() {
    var fileId = document.getElementById("DocumentsInput");
    let myFileList = new DataTransfer();
    documents.forEach(function (file) {
        myFileList.items.add(file);
        //console.log("hello" + file);
    });
    fileId.files = myFileList.files;
}
function previewDocuments() {
    
    var documentsInput = document.getElementById("DocumentsInput");
    var selectedDocuments = document.querySelector('.selected-documents');
    documentsInput.addEventListener("change", () => {
        
        selectedDocuments.innerHTML = '';
        const documents = documentsInput.files;
        for (let i = 0; i < documentsInput.files.length; i++) {
            selectedDocuments.innerHTML += `<a target="_blank" href="${URL.createObjectURL(documents[i])}"
                       class="btn border border-dark rounded-pill p-2 d-flex align-items-center gap-2 text-15">${documents[i].name}</a>`
        }
    })
}


function addTimeMission() {
    $('#MissionForm').submit((e) => {
        e.preventDefault();
        setImageInput();
        var descSpan = document.getElementById("descriptionError");
        var tinyTextArea = tinymce.get("tiny").getContent();
        if (tinyTextArea === "" || tinyTextArea === null) {
            descSpan.innerHTML = "Please Enter Mission description!";
            return;

        }

        if (images.length == 0 || images.length < 1) {
            var dragImgSpan = document.getElementById("mediaError");
            dragImgSpan.innerHTML = "Minimum 1 Picture is Required!";
            return;
        }
        var skill = $("#MissionSkills").find(':selected').length;
        //alert(skill)
        if (skill === 0 || skill === null) {
            $('#skillError').text("Minimum 1 Skill is Required!");
            return;

        }
        if ($('#MissionForm').valid()) {
            var formData = new FormData($('#MissionForm')[0]);
            formData.set('Description', tinyTextArea);
            console.log(formData);
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/AddTimeMission',
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    missionAjax();
                    Swal.fire(
                        'Mission Addded Successfully!',
                        'You clicked the button!',
                        'success'
                    )
                },
                error: (err) => {
                    console.log("error in getting banner form");
                }
            });

        }
        else {
            return;
        }
    })
}

function addGoalMission() {
    $('#MissionForm').submit((e) => {
        e.preventDefault();
        setImageInput();
        var descSpan = document.getElementById("descriptionError");
        var tinyTextArea = tinymce.get("tiny").getContent();
        if (tinyTextArea === "" || tinyTextArea === null) {
            descSpan.innerHTML = "Please Enter Mission description!";
            return;

        }

        if (images.length == 0 || images.length < 1) {
            var dragImgSpan = document.getElementById("mediaError");
            dragImgSpan.innerHTML = "Minimum 1 Picture is Required!";
            return;
        }
        var skill = $("#MissionSkills").find(':selected').length;
        //alert(skill)
        if (skill === 0 || skill === null) {
            $('#skillError').text("Minimum 1 Skill is Required!");
            return;

        }
        if ($('#MissionForm').valid()) {
            var formData = new FormData($('#MissionForm')[0]);
            formData.set('Description', tinyTextArea);
            console.log(formData);
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/AddGoalMission',
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    missionAjax();
                    Swal.fire(
                        'Mission Added Successfully!',
                        'You clicked the button!',
                        'success'
                    )
                },
                error: (err) => {
                    console.log("error in getting banner form");
                }
            });

        }
        else {
            return;
        }
    })
}


function getEditMissionForm() {
    var editMission = document.querySelectorAll(".edit-mission");
    editMission.forEach((editBtn) => {
        var missionId = editBtn.getAttribute("data-missionid");
        var missionType = editBtn.getAttribute("data-missiontype");
        
        editBtn.addEventListener('click', () => {
            //alert(missionId + missionType);
            if (missionType == "True") {
                $.ajax({
                    type: "GET",
                    url: '/Admin/Dashboard/GetTimeMissionEditForm',
                    data: { missionId: missionId},
                    success: function (data) {

                        $('#adminPartial').html(data);
                        $.getScript('/js/tinymce.js');
                        fileClick();
                        previewMedia();
                        displayMissionImages();
                        previewDocuments();
                        fetchAndCreateFiles();
                        setDocInput();
                        editTimemissionDetails(missionId);
                        getCitiesByCountry();
                       
                    },
                    error: (err) => {
                        console.log("error in getting banner form");
                    }
                });
            }
            else {
                $.ajax({
                    type: "GET",
                    url: '/Admin/Dashboard/GetGoalMissionEditForm',
                    data: { missionId: missionId },
                    success: function (data) {
                        $('#adminPartial').html(data);
                        $.getScript('/js/tinymce.js');
                        fileClick();
                        previewMedia();
                        displayMissionImages();
                        previewDocuments();
                        //previewDoc();
                        fetchAndCreateFiles();
                        setDocInput();
                        editGoalMissionDetails(missionId);
                        getCitiesByCountry();
                    },
                    error: (err) => {
                        console.log("error in getting banner form");
                    }
                });
            }
        })
    })
}


function editTimemissionDetails(missionId) {
    $('#MissionForm').submit((e) => {
        e.preventDefault();
        setImageInput();
        var descSpan = document.getElementById("descriptionError");
        var tinyTextArea = tinymce.get("tiny").getContent();
        if (tinyTextArea === "" || tinyTextArea === null) {
            descSpan.innerHTML = "Please Enter Mission description!";
            return;

        }

        if (images.length == 0 || images.length < 1) {
            var dragImgSpan = document.getElementById("mediaError");
            dragImgSpan.innerHTML = "Minimum 1 Picture is Required!";
            return;
        }
        var skill = $("#MissionSkills").find(':selected').length;
        //alert(skill)
        if (skill === 0 || skill === null) {
            $('#skillError').text("Minimum 1 Skill is Required!");
            return;

        }
        if ($('#MissionForm').valid()) {
            var formData = new FormData($('#MissionForm')[0]);
            formData.set('Description', tinyTextArea);
            formData.set('MissionId', missionId);
            console.log(formData);
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/EditTimeMissionDetails',
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    missionAjax();
                    Swal.fire(
                        'Mission Updated Successfully!',
                        'You clicked the button!',
                        'success'
                    )
                },
                error: (err) => {
                    console.log("error in getting banner form");
                }
            });

        }
        else {
            return;
        }
    })
}


function editGoalMissionDetails(missionId) {
    $('#MissionForm').submit((e) => {
        e.preventDefault();
        setImageInput();
        var descSpan = document.getElementById("descriptionError");
        var tinyTextArea = tinymce.get("tiny").getContent();
        if (tinyTextArea === "" || tinyTextArea === null) {
            descSpan.innerHTML = "Please Enter Mission description!";
            return;

        }

        if (images.length == 0 || images.length < 1) {
            var dragImgSpan = document.getElementById("mediaError");
            dragImgSpan.innerHTML = "Minimum 1 Picture is Required!";
            return;
        }
        var skill = $("#MissionSkills").find(':selected').length;
        //alert(skill)
        if (skill === 0 || skill === null) {
            $('#skillError').text("Minimum 1 Skill is Required!");
            return;

        }
        if ($('#MissionForm').valid()) {
            var formData = new FormData($('#MissionForm')[0]);
            formData.set('Description', tinyTextArea);
            formData.set('MissionId', missionId);
            console.log(formData);
            $.ajax({
                type: "POST",
                url: '/Admin/Dashboard/EditGoalMissionDetails',
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    missionAjax();
                    Swal.fire(
                        'Mission Updated Successfully!',
                        'You clicked the button!',
                        'success'
                    )
                },
                error: (err) => {
                    console.log("error in getting banner form");
                }
            });

        }
        else {
            return;
        }
    })
}

function previewMedia() {
    images = [];
    var missionMedia = $('#missionMedia');
    

    var mediaName = missionMedia.data('name');
    var mediaType = missionMedia.data('type');
    var mediaPath = missionMedia.data('path');

    console.log("path===>>> " + mediaPath + mediaName + mediaType);


    Promise.all(Array.from(document.querySelectorAll('[data-path]')).map((image, index) => {
        const fileName = image.value;
        const url = $(image).data("path") + $(image).data("name") + $(image).data("type");
        const type = $(image).data("type");
        return fetch(url)
            .then(response => response.arrayBuffer())
            .then(buffer => {
                const myFile = new File([buffer], $(image).data("name") + $(image).data("type"), { type: `image/${type.slice(1)}` });
                images.push(myFile);
            });
    }))
        .then(() => {
            displayImages();

        })
        .catch(error => {
            console.error(error);
        });
}
var documents = [];
async function fetchAndCreateFiles() {
    documents = [];
    const docImages = Array.from(document.querySelectorAll('#missionDoc'));
    for (const image of docImages) {
        const fileName = $(image).data("docname") + $(image).data("doctype");
        const url = $(image).data("docpath") + $(image).data("docname") + $(image).data("doctype");
        const type = $(image).data("doctype");
        //const title = $(image).data("title");

        const response = await fetch(url);
        const buffer = await response.arrayBuffer();
        const myFile = new File([buffer], fileName, { type: `image/${type.slice(1)}` });
        documents.push(myFile);
        //titles.push(title);
    }
    setDocInput();
    var selectedDocuments = document.querySelector('.selected-documents');
    selectedDocuments.innerHTML = '';
    for (let i = 0; i < documents.length; i++) {
        selectedDocuments.innerHTML += `<a target="_blank" href="${URL.createObjectURL(documents[i])}"
                class="btn border border-dark rounded-pill p-2 d-flex align-items-center gap-2 text-15">${documents[i].name}</a>`
    }
}


