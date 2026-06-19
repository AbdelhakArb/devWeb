import { Component } from '@angular/core'; 
import { CommonModule } from '@angular/common';
import { Article } from '../../services/api/models/article';
import { ListeDeNaissance } from '../../services/api/models/liste-naissance';
import { ListeNaissanceService } from '../../services/api/liste-naissance'; 
import { AuthService } from '../../services/auth'; 
import { Router } from '@angular/router';

@Component({
  selector: 'app-gestion-liste',
  standalone: true,
  imports: [CommonModule],
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
  listeIdActuelle: number | null = null;
  listeCloturee: boolean = false;
  afficherMessageConfirmation: boolean = false;

  constructor(
    private listeService: ListeNaissanceService,
    private authService: AuthService, 
    private router: Router
  ) {
    // Chargement immédiat dans le constructeur
    const parentId = this.authService.utilisateurConnecte()?.id;
    if (parentId) {
      this.listeService.obtenirListesParParent(parentId).subscribe({
        next: (listes: ListeDeNaissance[]) => {
          if (listes && listes.length > 0) {
            this.listeIdActuelle = listes[0].listeDeNaissanceId || null;
            this.listeCloturee = listes[0].statusListe === 'Cloturee';
            
            if (this.listeIdActuelle) {
              this.chargerMaListe();
            }
          }
        },
        error: (err: any) => console.error("Erreur lors de la récupération des listes :", err)
      });
    }
  }

  chargerMaListe() {
    if (!this.listeIdActuelle) return;

    this.listeService.chargerArticlesDeLaListe(this.listeIdActuelle).subscribe({
      next: (articlesDB: Article[]) => {
        this.maListeArticles = articlesDB;
      },
      error: (err: any) => console.error("Erreur lors du chargement des articles :", err)
    });
  }

  ajouterArticle(article: Article) {
    if (this.listeCloturee) return;
    if (!this.listeIdActuelle) return;

    const articleExistant = this.maListeArticles.find(item => item.id === article.id);
    if (articleExistant) {
      articleExistant.articleQty = (articleExistant.articleQty || 0) + 1;
    } else {
      this.maListeArticles.push({ ...article, articleQty: 1 });
    }

    const quantiteAEnvoyer: number = (articleExistant ? articleExistant.articleQty : 1) || 1;
    
    this.listeService.ajouterArticleDansListe(this.listeIdActuelle!, article.id!, quantiteAEnvoyer).subscribe({
      next: () => console.log('Article synchronisé en DB !'),
      error: (err: any) => console.error('Erreur de synchro DB :', err)
    });
  }

  retirerArticle(idArticle: number) {
    if (this.listeCloturee) return;

    const itemAModifier = this.maListeArticles.find(item => item.id === idArticle);
    if (itemAModifier && itemAModifier.articleQty && itemAModifier.articleQty > 1) {
      itemAModifier.articleQty--;
    } else {
      this.maListeArticles = this.maListeArticles.filter(item => item.id !== idArticle);
    }
  }

  cloturerLaListe() {
    this.listeCloturee = true;
    this.afficherMessageConfirmation = true;
  }

  acheterLeReste() {
    this.router.navigate(['/paiement']);
  }

  get totalPrix(): number {
    return this.maListeArticles.reduce((total, item) => total + (item.prix * (item.articleQty || 1)), 0);
  }

  get totalQuantite(): number {
    return this.maListeArticles.reduce((total, item) => total + (item.articleQty || 0), 0);
  }

  get resteAPayer(): number {
    return this.maListeArticles.reduce((total, item) => total + (item.prix * (item.articleQty || 1)), 0);
  }
}