import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ListeNaissanceService } from '../../services/api/liste-naissance';
import { ListeReservationService } from '../../services/liste-reservation';
import { PanierComponent } from '../../components/panier/panier';
import { PanierItem } from '../../services/api/models/panier';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-visiteur-view',
  standalone: true,
  imports: [CommonModule, FormsModule, PanierComponent],
  templateUrl: './visiteur-view.html',
  styleUrl: './visiteur-view.css'
})
export class VisiteurViewComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly listeService = inject(ListeNaissanceService);
    private readonly authService = inject(AuthService);
  readonly resService = inject(ListeReservationService);

  readonly idListeRecherche = signal<string>('');
  readonly rechercheFaite = signal<boolean>(false);
  readonly messageErreur = signal<string>('');
  readonly listeTrouvee = signal<any>(null);
  readonly listeIdActuelle = signal<number | null>(null);

  readonly afficherModale = signal<boolean>(false);
  readonly reservationConfirmee = signal<boolean>(false);
  
  quantitesReservees: { [articleId: number]: number } = {};
  messageTextSaisi: string = '';
  signatureMessageSaisie: string = '';

  get articlesListe(): any[] { return this.resService.articlesDeLaListe(); }
  get totalArticlesSelectionnes(): number { return Object.values(this.quantitesReservees).reduce((acc, val) => acc + val, 0); }

  get visiteurIdSaisi(): number {
  return this.authService.utilisateurConnecte()?.id ?? 0;
}

  get totalMontantEstime(): number {
    return this.articlesListe.reduce((total, item) => {
      const id = this.getId(item);
      return total + ((this.quantitesReservees[id] || 0) * this.getPrix(item));
    }, 0);
  }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.idListeRecherche.set(idParam);
      this.rechercherListeParId(Number(idParam));
    }
  }

  rechercherListe(): void {
    const id = Number(this.idListeRecherche());
    if (!id || isNaN(id)) {
      this.rechercheFaite.set(true);
      this.messageErreur.set("Veuillez entrer un identifiant valide.");
      return;
    }
    this.rechercherListeParId(id);
  }

  private rechercherListeParId(id: number): void {
    this.rechercheFaite.set(true);
    this.messageErreur.set('');
    this.listeService.chargerListeParId(id).subscribe({
      next: (liste: any) => {
        if (liste) {
          this.listeTrouvee.set(liste);
          const resolvedId = Number(liste.listeId ?? liste.ListeId ?? liste.id ?? liste.Id ?? id);
          this.listeIdActuelle.set(resolvedId);
          this.resService.enregistrerConsultation(resolvedId, this.visiteurIdSaisi).subscribe();

          if (!this.estCloturee()) {
            this.resService.chargerArticlesDeLaListe(resolvedId);
          }
        } else {
          this.messageErreur.set("Aucune liste trouvée.");
          this.listeTrouvee.set(null);
        }
      },
      error: () => {
        this.messageErreur.set("Erreur lors de la recherche.");
        this.listeTrouvee.set(null);
      }
    });
  }

  estCloturee(): boolean {
    const l = this.listeTrouvee();
    if (!l) return false;
    const statut = l.statusListe ?? l.StatusListe ?? l.statut ?? '';
    return statut.toString().toLowerCase().includes('cloture');
  }

  getStatutLibelle(l: any): string {
    return l?.statusListe ?? l?.StatusListe ?? l?.statut ?? 'Sauvegardee';
  }

  getId(item: any): number { return item.articleId ?? item.ArticleId ?? item.id ?? item.Id ?? 0; }
  getNom(item: any): string { return item.articleNom ?? item.ArticleNom ?? item.nom ?? 'Article'; }
  getPrix(item: any): number { return item.articlePrix ?? item.ArticlePrix ?? item.prix ?? 0; }
  
  getStock(item: any): number {
    const qtySouhaitee = item.qtySouhaitee ?? item.articleQty ?? item.quantiteSouhaitee ?? 0;
    const qtyReserve = item.qtyReserve ?? item.quantiteReservee ?? 0;
    const enCours = this.quantitesReservees[this.getId(item)] || 0;
    return Math.max(0, qtySouhaitee - qtyReserve - enCours);
  }

  getNomListe(l: any): string { return l?.nomListeDeNaissance ?? l?.NomListeDeNaissance ?? l?.titre ?? l?.nom ?? 'Liste'; }
  
  getListId(l: any): number { 
    return Number(l?.listeId ?? l?.ListeId ?? l?.id ?? l?.Id ?? this.listeIdActuelle() ?? 0); 
  }

  getDateEvenement(l: any): any { return l?.datePrevuForAccouchement ?? l?.DatePrevuForAccouchement ?? l?.dateEvenement ?? l?.date ?? new Date(); }

  modifierQuantiteSouhaitee(articleId: number, delta: number, stockMax: number): void {
    if (this.estCloturee()) return;
    const currentVal = this.quantitesReservees[articleId] || 0;
    const newVal = currentVal + delta;
    if (newVal >= 0 && newVal <= stockMax) {
      this.quantitesReservees[articleId] = newVal;
    }
  }

  panierItemsVisiteur(): PanierItem[] {
    const totalGlobal = this.totalMontantEstime;
    const currentListId = this.listeIdActuelle() || this.getListId(this.listeTrouvee());
    return Object.entries(this.quantitesReservees)
      .filter(([_, qty]) => qty > 0)
      .map(([id, qty]) => {
        const item = this.articlesListe.find(a => this.getId(a) === Number(id));
        return {
          listeDeNaissanceId: currentListId,
          articleId: Number(id),
          articleNom: item ? this.getNom(item) : 'Article',
          articlePrix: item ? this.getPrix(item) : 0,
          articleQty: qty,
          soustotalArticles: qty * (item ? this.getPrix(item) : 0),
          totalAPayer: totalGlobal
        };
      });
  }

  ouvrirConfirmation(): void { if (!this.estCloturee() && this.totalArticlesSelectionnes > 0) this.afficherModale.set(true); }
  fermerModale(): void { this.afficherModale.set(false); }

  validerReservation(payload: any[]): void {
    if (this.estCloturee()) return;
    console.log('[DEBUG] payload envoyé:', payload);
    this.resService.soumettreReservations(payload).subscribe({
      next: (success) => { 
        if (success) { 
          this.afficherModale.set(false); 
          this.reservationConfirmee.set(true); 
        } 
      }
    });
  }

  recommencer(): void {
    this.reservationConfirmee.set(false);
    this.quantitesReservees = {};
    this.messageTextSaisi = '';
    this.signatureMessageSaisie = '';
    const currentListId = this.listeIdActuelle();
    if (currentListId) this.resService.chargerArticlesDeLaListe(currentListId);
  }
}