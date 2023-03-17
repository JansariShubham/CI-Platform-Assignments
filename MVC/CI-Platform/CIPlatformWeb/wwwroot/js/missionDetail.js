

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
