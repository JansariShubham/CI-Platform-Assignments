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
