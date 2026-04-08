let productsGrid = document.getElementById("productsGrid");
let productInfo = document.getElementById("productInfo");
let productCard = document.getElementById("productCard");
let skip = 0;
const limit = 12;
let isLoading = false;
const loadMoreTrigger = document.getElementById("loadMoreTrigger");
getProductDetails(0);
const observer = new IntersectionObserver((entries) => {
    if (entries[0].isIntersecting && !isLoading) {
        isLoading = true;
        skip += limit;
        getProductDetails(skip);
    }
}, {
    threshold: 0.1 
});
observer.observe(loadMoreTrigger);

function getProductDetails(skip) {
isLoading = true;
fetch(`https://dummyjson.com/products?limit=${limit}&skip=${skip}`, {
    method: 'GET',
    headers: { 'Content-Type': 'application/json'},
})
.then(res => {if(!res.ok){ 
        return res.json().then(errData => {
                const errorCustom = new Error(errData.message || "Erreur inconnue");
                errorCustom.status = res.status; 
                throw errorCustom; 
            });
    } return res.json();})
.then(data => {showProductList(data.products, skip);isLoading = false;})
.catch(err =>{ errorHandler(err); isLoading = false;});
}


function showProductList(productList, skip) {
    if (skip === 0) {
        productInfo.innerHTML = '';
    }   
    productList.forEach(product => {
        const item = document.createElement('div');
        item.className = "product-item";
        item.innerHTML = `<table>
            <tr>
                <td><img src="${product.thumbnail}" alt="${product.title}" width="150"></td>
                <td colspan="2"><h3>${product.title}</h3></td>
                <td>Prix: $${product.price}</td>
            </tr>
        </table>
        `;

        item.addEventListener("click", function() {
            showProductCard(product.id);
        });
        productInfo.appendChild(item);
    });
}


function showProductCard(productId){
fetch(`https://dummyjson.com/products/${productId}`, {
    method: 'GET',
    headers: { 'Content-Type': 'application/json' },
})
.then(res => {if(!res.ok){ 
        return res.json().then(errData => {
                const errorCustom = new Error(errData.message || "Erreur inconnue");
                errorCustom.status = res.status; 
                throw errorCustom; 
            });
    } return res.json();})
.then(data => {

    productCard.innerHTML = `
        <span id="closeX" style="position:absolute; top:10px; right:15px; cursor:pointer; font-weight:bold; font-size:20px;">&times;</span>
        <img src="${data.thumbnail}" alt="${data.title}" width="300">
        <h2>${data.title}</h2>
        <p>${data.description}</p>
        <p>Prix: $${data.price}</p>
    `;
    productCard.style.display = "flex";
    document.getElementById("closeX").onclick = () => {
            productCard.style.display = "none";
        };

})
.catch(err => errorHandler(err));
}


document.addEventListener("click", (event) => {
    // Si la carte est ouverte ET que le clic n'est ni sur la carte, ni sur un produit de la liste
    if (productCard.style.display === "flex" && 
        !productCard.contains(event.target) && 
        !event.target.closest(".product-item")) {
        
        productCard.style.display = "none";
    }
});


function errorHandler(error) {
    let errorP = document.getElementById("errorMessage");
    let message = '';
    switch (error.status) {
        case 400: message = 'Requête invalide.'; break;
        case 404: message = 'Produit introuvable.'; break;
        case 500: message = 'Erreur serveur.'; break;
        default: message = 'Une erreur est survenue (Code: ' + error.status + ')';
    }  
    productsGrid.style.display = "none";
    errorP.innerHTML = `<p style="color: red;">${message}</p>`;
}
