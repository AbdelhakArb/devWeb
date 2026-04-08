let boxes = document.querySelectorAll('.color-box');
boxes.forEach(box => {  box.addEventListener('click', (e) => {
    let color = e.target.dataset.color;
    document.body.style.backgroundColor = color;
  });
});
