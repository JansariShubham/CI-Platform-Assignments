
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
                    <img class="cancle-image position-absolute" src='images/cancel1.png'
                        alt="">
                    </a> </div>`;
    }
}

function removeImg(index) {
    images.splice(index, 1);
    displayImages();
}


