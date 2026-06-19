import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ListeNaissanceService } from '../../services/api/liste-naissance';
import { Article } from '../../services/api/models/article';
import { ListeDeNaissance } from '../../services/api/models/liste-naissance';

@Component({
  selector: 'app-visiteur-view',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './visiteur-view.html',
  styleUrl: './visiteur-view.css'
})
export class VisiteurViewComponent {
  idListeRecherche: string = '';
  listeTrouvee: ListeDeNaissance | null = null;
  articlesListe: Article[] = [];
  
  rechercheFaite: boolean = false;
  messageErreur: string = '';

  quantitesReservees: { [articleId: number]: number } = {};
  
  afficherModale: boolean = false;
  messagePourParents: string = '';
  reservationConfirmee: boolean = false;

  constructor(private listeService: ListeNaissanceService) {}

  rechercherListe() {
    this.messageErreur = '';
    this.listeTrouvee = null;
    this.rechercheFaite = false;
    this.articlesListe = [];

    const id = parseInt(this.idListeRecherche.trim(), 10);
    if (isNaN(id)) {
      this.messageErreur = "Veuillez entrer un identifiant de liste valide (chiffre).";
      return;
    }

    this.listeService.chargerListeParId(id).subscribe({
      next: (liste: ListeDeNaissance) => {
        this.listeTrouvee = { ...liste };
        this.rechercheFaite = true;
        
        this.listeService.chargerArticlesDeLaListe(id).subscribe({
          next: (articles: Article[]) => {
            this.articlesListe = articles;
            this.articlesListe.forEach(art => this.quantitesReservees[art.id] = 0);
          }
        });
      },
      error: (err: any) => {
        this.rechercheFaite = true;
        this.messageErreur = "Aucune liste de naissance ne correspond à cet identifiant.";
      }
    });
  }

  modifierQuantiteSouhaitee(articleId: number, changement: number, maxDisponible: number) {
    const qteActuelle = this.quantitesReservees[articleId] || 0;
    const nouvelleQte = qteActuelle + changement;

    if (nouvelleQte >= 0 && nouvelleQte <= maxDisponible) {
      this.quantitesReservees[articleId] = nouvelleQte;
    }
  }

  get totalArticlesSelectionnes(): number {
    return Object.values(this.quantitesReservees).reduce((sum, qte) => sum + qte, 0);
  }

  get totalMontantEstime(): number {
    return this.articlesListe.reduce((sum, art) => {
      const qte = this.quantitesReservees[art.id] || 0;
      return sum + (art.prix * qte);
    }, 0);
  }

  ouvrirConfirmation() {
    if (this.totalArticlesSelectionnes > 0) {
      this.afficherModale = true;
    }
  }

  fermerModale() {
    this.afficherModale = false;
    this.messagePourParents = '';
  }

  validerReservation() {
    if (!this.listeTrouvee?.listeDeNaissanceId) return;

    // On récupère le premier article sélectionné pour l'exemple d'envoi
    const articlesSelectionnes = this.articlesListe.filter(art => this.quantitesReservees[art.id] > 0);
    
    if (articlesSelectionnes.length === 0) return;

    // Boucle d'enregistrement simple
    articlesSelectionnes.forEach(art => {
      this.listeService.reserverArticle(this.listeTrouvee!.listeDeNaissanceId!, art.id).subscribe({
        next: () => {
          this.afficherModale = false;
          this.reservationConfirmee = true;
        },
        error: (err: any) => {
          console.error("Erreur lors de la réservation", err);
        }
      });
    });
  }

  recommencer() {
    this.listeTrouvee = null;
    this.articlesListe = [];
    this.rechercheFaite = false;
    this.quantitesReservees = {};
    this.reservationConfirmee = false;
    this.idListeRecherche = '';
  }
}