-- #############################################################################
-- # SCRIPT SQL FINAL : GESTION DES LISTES DE NAISSANCES
-- #############################################################################

-- 1. BASE DE DONNÉES
-- ===============================================

CREATE DATABASE IF NOT EXISTS gestiondeslistesdenaissances CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE gestiondeslistesdenaissances;

-- 2. TABLES 
-- =======================================================================

CREATE TABLE COMPTEPARENT (
    compteParentId INT AUTO_INCREMENT PRIMARY KEY,
    nomPremierParent VARCHAR(100) NOT NULL,
    prenomPremierParent VARCHAR(100) NOT NULL,
    nomSecondParent VARCHAR(100),
    prenomSecondParent VARCHAR(100),
    motDePasseCompte CHAR(60) NOT NULL, 
    emailDeContact VARCHAR(255) NOT NULL UNIQUE,
    adresseParent VARCHAR(255),
    cpParent VARCHAR(10),
    villeParent VARCHAR(100),
    paysParent VARCHAR(100)
);

CREATE TABLE VISITEUR (
    visiteurId INT AUTO_INCREMENT PRIMARY KEY,
    visiteurNom VARCHAR(100) NOT NULL,
    visiteurPrenom VARCHAR(100) NOT NULL,
    visiteurEmail VARCHAR(255) NOT NULL UNIQUE,
    visiteurMdp CHAR(60) NOT NULL,
    visiteurAdresse VARCHAR(255),
    visiteurCP VARCHAR(10),
    visiteurVille VARCHAR(100),
    visiteurPays VARCHAR(100)
);

-- Table des categories
CREATE TABLE CATEGORIE (
    categorieId SMALLINT AUTO_INCREMENT PRIMARY KEY,
    categorieNom VARCHAR(100) NOT NULL UNIQUE,
    categorieDesc VARCHAR(255)
);

-- Table des modeles listes de naissance
CREATE TABLE MODELDELISTEDENAISSANCE (
    modelListeId INT AUTO_INCREMENT PRIMARY KEY,
    modelListeNom VARCHAR(255) NOT NULL,
    modelListeDesc TEXT
);

CREATE TABLE ARTICLE (
    articleId INT AUTO_INCREMENT PRIMARY KEY,
    articleNom VARCHAR(255) NOT NULL,
    articleDesc TEXT,
    articleQty INT,
    articlePrix DECIMAL(8, 2)
);

CREATE TABLE LISTEDENAISSANCE (
    listeDeNaissanceId INT AUTO_INCREMENT PRIMARY KEY,
    compteParentId INT NOT NULL,
    nomListeDeNaissance VARCHAR(255) NOT NULL,
    dateCreationListe DATE NOT NULL,
    datePrevuPourAccouchement DATE NOT NULL,
    statusListe VARCHAR(20) NOT NULL,
    lieuListe VARCHAR(255),
    FOREIGN KEY (compteParentId) REFERENCES COMPTEPARENT(compteParentId)
);

CREATE TABLE MESSAGES (
    messageId INT AUTO_INCREMENT PRIMARY KEY,
    listeDeNaissanceId INT NOT NULL,
    visiteurId INT NOT NULL, 
    messageText TEXT NOT NULL,
    signatureMessage VARCHAR(100),
    dateMessage DATETIME NOT NULL,
    FOREIGN KEY (listeDeNaissanceId) REFERENCES LISTEDENAISSANCE(listeDeNaissanceId),
    FOREIGN KEY (visiteurId) REFERENCES VISITEUR(visiteurId)
);


CREATE TABLE PRESENCEARTICLEDANSLISTE (
    presenceArticleDansListeId INT AUTO_INCREMENT NOT NULL,
    listeDeNaissanceId INT NOT NULL,
    articleId INT NOT NULL,
    qtySouhaitee INT NOT NULL,
    PRIMARY KEY (presenceArticleDansListeId),
    UNIQUE KEY (listeDeNaissanceId, articleId), 
    FOREIGN KEY (listeDeNaissanceId) REFERENCES LISTEDENAISSANCE(listeDeNaissanceId),
    FOREIGN KEY (articleId) REFERENCES ARTICLE(articleId)
);

CREATE TABLE RESERVATION (
    reservationId INT AUTO_INCREMENT NOT NULL,
    visiteurId INT NOT NULL, 
    presenceArticleDansListeId INT NOT NULL,
    qtyReserve INT NOT NULL,
    dateReservation DATE,
    PRIMARY KEY (reservationId),
    FOREIGN KEY (presenceArticleDansListeId) REFERENCES PRESENCEARTICLEDANSLISTE(presenceArticleDansListeId) ON DELETE RESTRICT,
    FOREIGN KEY (visiteurId) REFERENCES VISITEUR(visiteurId)
);


CREATE TABLE CATEGORIEARTICLE (
    categorieId SMALLINT NOT NULL,
    articleId INT NOT NULL,
    PRIMARY KEY (categorieId, articleId),
    FOREIGN KEY (categorieId) REFERENCES CATEGORIE(categorieId),
    FOREIGN KEY (articleId) REFERENCES ARTICLE(articleId)
);


CREATE TABLE CONSULTATION (
    listeDeNaissanceId INT NOT NULL,
    visiteurId INT NOT NULL,
    dateConsultation DATETIME NOT NULL,
    PRIMARY KEY (listeDeNaissanceId, visiteurId),
    FOREIGN KEY (listeDeNaissanceId) REFERENCES LISTEDENAISSANCE(listeDeNaissanceId),
    FOREIGN KEY (visiteurId) REFERENCES VISITEUR(visiteurId)
);

CREATE TABLE PRESENCEARTICLEDANSMODEL (
    modelListeId INT NOT NULL,
    articleId INT NOT NULL,
    PRIMARY KEY (modelListeId, articleId),
    FOREIGN KEY (modelListeId) REFERENCES MODELDELISTEDENAISSANCE(modelListeId),
    FOREIGN KEY (articleId) REFERENCES ARTICLE(articleId)
);

CREATE TABLE CONSULTER_CHOISIR (
    compteParentId INT NOT NULL,
    modelListeId INT NOT NULL,
    PRIMARY KEY (compteParentId, modelListeId),
    FOREIGN KEY (compteParentId) REFERENCES COMPTEPARENT(compteParentId),
    FOREIGN KEY (modelListeId) REFERENCES MODELDELISTEDENAISSANCE(modelListeId)
);