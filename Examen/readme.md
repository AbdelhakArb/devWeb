# Projet - Liste de Naissance

Application web de gestion et de consultation de listes de naissance, développée dans le cadre de l'examen. Le projet respecte les principes de la **Clean Architecture** côté backend et utilise **Angular** avec une gestion d'état par services côté frontend.

---

## 🛠️ Prérequis techniques

Avant de lancer le projet, assurez-vous d'avoir installé les versions suivantes sur votre machine :

* **.NET SDK** : Version 8.0 (ou supérieure)
* **Node.js** : Version 18.x ou supérieure (avec npm)
* **Angular CLI** : Version 18.x (ou supérieure)
* **SGBD** : MySQL (via XAMPP, WampServer ou un serveur MySQL autonome)

---

## 📂 Installation et Configuration

### 1. Base de données (MySQL)

1. Lancez votre serveur MySQL.
2. Créez une nouvelle base de données (par exemple nommée `liste_de_naissance`).
3. Exécutez les scripts SQL fournis dans le dossier du projet pour initialiser les tables et insérer les données de test.

### 2. Configuration de la chaîne de connexion (Backend)

1. Ouvrez le projet backend dans votre IDE (Visual Studio ou VS Code).
2. Localisez le fichier de configuration `appsettings.json` (ou `appsettings.Development.json`) dans le projet **API**.
3. Modifiez la chaîne de connexion `DefaultConnection` pour pointer vers votre base de données MySQL locale :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=liste_de_naissance;Uid=root;Pwd=;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Microsoft.AspNetCore": "Warning",
      "Microsoft.AspNetCore.Hosting": "Information"
    }
  },
  "AllowedHosts": "*"
}

### 3. lancement du projet

- Ouvrez un terminal et positionnez-vous dans le dossier contenant le projet API :

Bash
cd backend/API
Restaurez les dépendances et lancez l'application :

Bash
dotnet restore
dotnet run
- L'API démarrera généralement sur https://localhost:5001 ou http://localhost:5200 (vérifiez la sortie de la console pour l'URL exacte de Swagger).

- Lancer le Frontend (Application Angular)
  - Ouvrez un nouveau terminal et positionnez-vous dans le dossier du frontend :

Bash
cd frontend
Installez les dépendances Node.js :

Bash
npm install
Lancez l'application en mode développement :

Bash
ng serve
Ouvrez votre navigateur et accédez à l'adresse : http://localhost:4200

🔑 Comptes de test
Si l'application nécessite une authentification ou l'utilisation de profils spécifiques pour tester les fonctionnalités (Parents / Visiteurs) :

Compte Parent (Gestion de liste) :

Email / Identifiant : parent.test@example.com (ou selon vos seeds SQL)

Rôle : Création et modification de la liste de naissance.

Visiteur (Consultation et Réservation) :

Utilisation libre via les liens de partage ou la sélection de listes publiques.

