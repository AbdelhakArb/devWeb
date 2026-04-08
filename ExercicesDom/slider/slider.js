let slidleft = document.getElementById("prev");
let slidright = document.getElementById("next");
let img = document.getElementById("img");   
let images = ["./elephant.jpg", "./lion.jpg","./giraffe.webp","./perroquet.jpg","./vache.webp","./bison.webp","./tigre.webp","./hibou.webp"];
let currentIndex = 0;
slidleft.addEventListener("click", function() {
    currentIndex--; 
    if (currentIndex < 0) {
        currentIndex = images.length - 1;
    }
    img.innerHTML = `<img src="${images[currentIndex]}" alt="image" style="width: 400px; height: 300px;">`;
});
slidright.addEventListener("click", function() {
    currentIndex++; 
    if (currentIndex >= images.length) {
        currentIndex = 0;
    }
    img.innerHTML = `<img src="${images[currentIndex]}" alt="image" style="width: 400px; height: 300px;">`;
});
