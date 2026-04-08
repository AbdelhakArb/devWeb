let authForm = document.getElementById('authForm');
let userInfoDiv = document.getElementById('userInfo');

 authForm.addEventListener('submit', function (event) {
    event.preventDefault();
let userInfo={};
    userInfo.username = document.getElementById('username').value;
    userInfo.password = document.getElementById('password').value;

authenticateUser(userInfo);
 });

function showUserCard(userData) {
        if(authForm) authForm.style.display = 'none';
        userInfoDiv.innerHTML = `
            <h2>Informations de l'utilisateur :</h2>
            <img src="${userData.image}" alt="User Image" width="100">
            <p><strong>Nom :</strong> ${userData.firstName} ${userData.lastName}</p>
            <p><strong>Email :</strong> ${userData.email}</p>
        `;
}


function errorHandler(error) {
    //const status = error.status || 'Réseau'; 
    let message = '';
    
    switch (error.status) {
        case 400: message = 'Identifiants requis.'; break;
        case 401: message = 'Identifiants incorrects ou session expirée.'; break;
        case 404: message = 'Serveur introuvable.'; break;
        case 500: message = 'Erreur serveur.'; break;
        case 'Réseau': message = 'Impossible de contacter le serveur. Vérifiez votre connexion.'; break;
        default: message = 'Une erreur est survenue (Code: ' + status + ')';
    }
    userInfoDiv.innerHTML = `<p style="color: red;">${message}</p>`;
}

function getUserInfo(token) {
    fetch('https://dummyjson.com/auth/me', {
            method: 'GET',
            headers: { 
                'Authorization': `Bearer ${token}`
            }
        })
    .then(res =>{if(!res.ok){ throw res; } return res.json();})
    .then(userData => { showUserCard(userData); })
    .catch(err => errorHandler(err));

    }

function authenticateUser(userInfo) {
   
    fetch('https://dummyjson.com/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            username: userInfo.username,
            password: userInfo.password,
            expiresInMins: 30, 
        })
    })
    .then(res => {if(!res.ok){ 
        return res.json().then(errData => {
                const errorCustom = new Error(errData.message || "Erreur inconnue");
                errorCustom.status = res.status; 
                throw errorCustom; 
            });
    } return res.json();})
    .then(loginData => {getUserInfo(loginData.accessToken);})
    .catch(err => errorHandler(err));
}

