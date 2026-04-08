let box = document.getElementById('myBox');
let posX = 50;
let posY = 50;
const speed = 10;

document.addEventListener('keydown', (event) => {
    switch (event.key) {
        case 'ArrowUp':
            posY -= speed;
            break;
        case 'ArrowDown':
            posY += speed; 
            break;
        case 'ArrowLeft':
            posX -= speed;
            break; 
        case 'ArrowRight': posX += speed;
            break;
    }
        box.style.top = posY + 'px';
         box.style.left = posX + 'px';
});