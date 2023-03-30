
    var images = [];
    document.getElementById("drag-img").addEventListener("change", () => {
        var files = document.getElementById("drag-img").files;

        for (var i = 0; i < files.length; i++) {

            images.push(files[i]);
        }
        displayImages();

        //console.log(images);
    })
    var dragDiv = document.querySelector(".drag-drop-div");

    dragDiv.addEventListener("click", () => {
        document.getElementById("drag-img").click();
    })


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


function displayImages() {
    document.querySelector(".image-display-div").innerHTML = '';
    for (var i = 0; i < images.length; i++) {
        document.querySelector(".image-display-div").innerHTML += `<div class = 'position-relative'> <img class = 'drag-img' src= ${URL.createObjectURL(images[i])} alt="">
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
    var fileId = document.getElementById("drag-img");
    let myFileList = new DataTransfer();
    images.forEach(function (file) {
        myFileList.items.add(file);
        //console.log("hello" + file);
    });
    fileId.files = myFileList.files;
}

var submitForm = document.getElementById("formSubmit");
submitForm.addEventListener('submit', (e) => {
        e.preventDefault();
        setImageInput();
        $('#action').val(e.submitter.getAttribute("value"));
        var tinyTextArea = tinymce.get("tiny").getContent();
        var descSpan = document.getElementById("descSpan");

        if (tinyTextArea === "" || tinyTextArea === null) {
            descSpan.innerHTML = "Story description is required!";
            return;

        }

        if (images.length == 0 || images.length < 1) {
            var dragImgSpan = document.getElementById("dragImgSpan");
            dragImgSpan.innerHTML = "Minimum 1 Picture is Required!";
            return;
        }
    
        $('#action').val(e.submitter.getAttribute("value"));
        submitForm.submit();
    
    

})
/*$(document).ready(function () {
    // Code to be executed when the DOM is ready
    console.log("DOM is ready");

});*//*
var storyMedia = document.querySelector("#storyMedia");
var mediaPath = storyMedia.getAttribute("data-path");
console.log("path===>>> " + mediaPath);*/

var mediaPath = $("#storyMedia").data("path");
var mediaName = $("#storyMedia").data("name");
var mediaType = $("#storyMedia").data("type");

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