const searchBar = document.getElementById('searchInput');
const listItems = document.querySelectorAll('#itemList li');

searchBar.addEventListener('input', function(e) {
    const searchTerm = e.target.value.toLowerCase();

    listItems.forEach(function(item) {
        const itemText = item.textContent.toLowerCase();
        if (itemText.includes(searchTerm)) {
            item.style.display = '';
        } else {
            item.style.display = 'none';
        }
    });
}); 