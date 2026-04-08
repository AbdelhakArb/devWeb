const input = document.getElementById('todoInput');
const btn = document.getElementById('addBtn');
const list = document.getElementById('todoList');

function addTask() {
  const taskText = input.value.trim();

  if (taskText !== "") {
    const li = document.createElement('li');

   li.innerHTML = `
      <input type="checkbox" class="todo-checkbox">
      <span class="task-text" style="flex-grow: 1;">${taskText}</span>
      <button class="delete-btn">🗑️</button>
    `;
    const checkbox = li.querySelector('.todo-checkbox');
    const textSpan = li.querySelector('.task-text');

    checkbox.addEventListener('change', function() {
      if (this.checked) {
        textSpan.style.textDecoration = "line-through";
        textSpan.style.textDecorationColor = "black";
        textSpan.style.color = "gray";
      } else {
        textSpan.style.textDecoration = "none";
        textSpan.style.color = "black";
      }
    });

    li.querySelector('.delete-btn').addEventListener('click', () => {
      li.remove();
    });
    list.appendChild(li);

    input.value = "";
    input.focus();
  }
}
btn.addEventListener('click', addTask);
input.addEventListener('keypress', (e) => {
  if (e.key === 'Enter') addTask();
});