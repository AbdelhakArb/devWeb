import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ListeNaissanceService } from '../../services/api/liste-naissance';

@Component({
  selector: 'app-creer-liste',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './creer-liste.html',
  styleUrl: './creer-liste.css',
})
export class CreerListeComponent implements OnInit {
  private readonly listeService = inject(ListeNaissanceService);
  private readonly router = inject(Router);

  readonly nomListe = signal<string>('');
  readonly dateAccouchement = signal<string>('');
  readonly lieu = signal<string>('');
  readonly messageErreur = signal<string>('');
  readonly messageSucces = signal<string>('');
  readonly isSubmitting = signal<boolean>(false);
  
  private articlesInitiaux: any[] = [];

  ngOnInit(): void {
    const currentState = history.state;

    if (currentState) {
      if (currentState['titreModele']) {
        this.nomListe.set(currentState['titreModele']);
      }
      if (currentState['descriptionModele'] && !this.lieu()) {
        this.lieu.set(currentState['descriptionModele']);
      }
      if (currentState['articlesInitiaux']) {
        this.articlesInitiaux = currentState['articlesInitiaux'];
      }
    }
  }

  onSubmit(): void {
    this.creerListe();
  }

  creerListe(): void {
    this.isSubmitting.set(true);
    this.messageErreur.set('');
    
    // Correction : Récupération dynamique de l'ID du parent connecté (ex: '25')
    const storedUserId = localStorage.getItem('userId') ?? localStorage.getItem('id') ?? localStorage.getItem('compteParentId');
    const parentId = Number(storedUserId ?? '25');

    const dto = {
      compteParentId: parentId,
      nomListeDeNaissance: this.nomListe(),
      datePrevuPourAccouchement: this.dateAccouchement(),
      lieuListe: this.lieu()
    };

    this.listeService.creerListe(dto).subscribe({
      next: (response: any) => {
        this.messageSucces.set('Liste de naissance créée avec succès !');
        const nouvelleListeId = response?.idListeDeNaissance ?? response?.id;
        
        if (this.articlesInitiaux.length > 0 && nouvelleListeId) {
          this.ajouterArticlesAuModele(nouvelleListeId, 0);
        } else {
          setTimeout(() => this.router.navigate(['/mes-listes']), 1000);
        }
      },
      error: () => {
        this.isSubmitting.set(false);
        this.messageErreur.set('Une erreur est survenue lors de la création.');
      }
    });
  }

  private ajouterArticlesAuModele(listeId: number, index: number): void {
    if (index >= this.articlesInitiaux.length) {
      this.isSubmitting.set(false);
      this.router.navigate(['/mes-listes']);
      return;
    }

    const article = this.articlesInitiaux[index];
    const articleId = article.articleId ?? article.id;
    const quantite = article.qtySouhaitee ?? article.quantite ?? 1;

    this.listeService.ajouterArticleDansListe(listeId, articleId, quantite).subscribe({
      next: () => {
        this.ajouterArticlesAuModele(listeId, index + 1);
      },
      error: () => {
        this.ajouterArticlesAuModele(listeId, index + 1);
      }
    });
  }
}