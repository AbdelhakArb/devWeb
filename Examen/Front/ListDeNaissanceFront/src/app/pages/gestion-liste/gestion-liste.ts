import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; // 1. Vérifie bien cet import
import { Article } from '../../services/api/models/article';

@Component({
  selector: 'app-gestion-liste',
  standalone: true,
  imports: [CommonModule], // 2. RE-VÉRIFIE ABSOLUMENT QUE COMMONMODULE EST BIEN ICI !
  templateUrl: './gestion-liste.html',
  styleUrl: './gestion-liste.css'
})
export class GestionListe {
  
  catalogueArticles: Article[] = [
    { id: 1, nom: 'Poussette Trio Évolutive', prix: 349.99, categorie: 'Transport' },
    { id: 2, nom: 'Lit à barreaux en bois', prix: 149.50, categorie: 'Chambre' },
    { id: 3, nom: 'Biberon anti-colique 260ml', prix: 12.90, categorie: 'Repas' },
    { id: 4, nom: 'Babyphone Caméra HD', prix: 89.99, categorie: 'Sécurité' },
    { id: 5, nom: 'Tapis d’éveil sensoriel', prix: 45.00, categorie: 'Jouets' }
  ];

  maListeArticles: Article[] = [];

  ajouterArticle(article: Article) {
    const articleExistant = this.maListeArticles.find(item => item.id === article.id);

    if (articleExistant) {
      articleExistant.articleQty = (articleExistant.articleQty || 0) + 1;
    } else {
      this.maListeArticles.push({ ...article, articleQty: 1 });
    }
  }

  retirerArticle(idArticle: number) {
    const articleExistant = this.maListeArticles.find(item => item.id === idArticle);

    if (articleExistant && articleExistant.articleQty && articleExistant.articleQty > 1) {
      articleExistant.articleQty--;
    } else {
      this.maListeArticles = this.maListeArticles.filter(item => item.id !== idArticle);
    }
  }

  get totalPrix(): number {
    return this.maListeArticles.reduce((total, item) => total + (item.prix * (item.articleQty || 1)), 0);
  }

  get totalQuantite(): number {
    return this.maListeArticles.reduce((total, item) => total + (item.articleQty || 0), 0);
  }
}