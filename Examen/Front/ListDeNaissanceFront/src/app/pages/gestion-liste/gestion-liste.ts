import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Article } from '../../services/api/models/article';
import { ListeNaissanceService } from '../../services/api/liste-naissance'; 
import { ListeReservationService } from '../../services/liste-reservation';
import { ArticleService } from '../../services/article';
import { CatalogueComponent } from '../catalogue/catalogue';
import { PanierComponent } from '../../components/panier/panier';
import { PanierItem } from '../../services/api/models/panier';

@Component({
  selector: 'app-gestion-liste',
  standalone: true,
  imports: [CommonModule, CatalogueComponent, PanierComponent],
  templateUrl: './gestion-liste.html',
  styleUrl: './gestion-liste.css'
})
export class GestionListe implements OnInit {
  private readonly listeService = inject(ListeNaissanceService);
  private readonly route = inject(ActivatedRoute);
  readonly resService = inject(ListeReservationService);
  readonly articleService = inject(ArticleService);

  readonly listeCloturee = signal<boolean>(false);
  readonly listeIdActuelle = signal<number | null>(null);
  readonly afficherModaleCloture = signal<boolean>(false);

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id') || this.route.parent?.snapshot.paramMap.get('id');
    
    if (idParam) {
      const listeId = Number(idParam);
      this.listeIdActuelle.set(listeId);
      this.resService.chargerArticlesDeLaListe(listeId);
    }
  }
 



  ajouterArticle(article: Article): void {
    if (this.listeCloturee() || !this.listeIdActuelle()) return;
    
    const articleId = article.articleId ?? article.id;
    if (!articleId) return;

    this.listeService.ajouterArticleDansListe(this.listeIdActuelle()!, articleId, 1).subscribe({
      next: () => {
        this.resService.chargerArticlesDeLaListe(this.listeIdActuelle()!);
      },
      error: (err: any) => console.error('Erreur ajout article :', err)
    });
  }

  modifierQuantite(item: any, delta: number): void {
    const listeId = this.listeIdActuelle();
    const articleId = item.articleId ?? item.id;
    if (!listeId || !articleId) return;

    const action$ = delta > 0 
      ? this.listeService.incrementerQuantite(listeId, articleId)
      : this.listeService.decrementerQuantite(listeId, articleId);

    action$.subscribe({
      next: () => {
        this.resService.chargerArticlesDeLaListe(listeId);
        if (delta > 0) {
          this.articleService.diminuerStock(articleId);
        } else {
          this.articleService.augmenterStock(articleId);
        }
      },
      error: (err: any) => console.error("Erreur modification quantité :", err)
    });
  }

  panierItems(): PanierItem[] {
    const items = this.resService.articlesDeLaListe();
    
    const mappedItems = items.map(item => {
      const prix = Number(item.articlePrix) || 0;
      const qty = Number(item.articleQty) || 1;
      return {
        articleId: item.articleId ?? item.id ?? 0,
        articleNom: item.articleNom ?? '',
        articlePrix: prix,
        articleQty: qty,
        soustotalArticles: prix * qty,
        totalAPayer: 0
      };
    });

    const total = mappedItems.reduce((acc, i) => acc + i.soustotalArticles, 0);
    return mappedItems.map(i => ({ ...i, totalAPayer: total }));
  }

  ouvrirCloture(): void {
    this.afficherModaleCloture.set(true);
  }

  fermerCloture(): void {
    this.afficherModaleCloture.set(false);
  }

  validerCloture(): void {
    const listeId = this.listeIdActuelle();
    if (!listeId) return;

    this.resService.cloturerListe(listeId, 'Cloturee').subscribe({
      next: () => {
        this.afficherModaleCloture.set(false);
        this.listeCloturee.set(true);
        this.resService.chargerArticlesDeLaListe(listeId);
      },
      error: (err) => console.error("Erreur clôture :", err)
    });
  }
}