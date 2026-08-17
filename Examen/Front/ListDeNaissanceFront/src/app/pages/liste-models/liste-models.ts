import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ListeModelsService } from '../../services/liste-models';

@Component({
  selector: 'app-liste-models',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './liste-models.html',
  styleUrl: './liste-models.css',
})
export class ListeModelsComponent implements OnInit {
  private readonly modelsService = inject(ListeModelsService);
  private readonly router = inject(Router);

  readonly modeles = signal<any[]>([]);
  readonly modeleSelectionne = signal<any | null>(null);
  readonly articlesModele = signal<any[]>([]);
  readonly afficherModale = signal<boolean>(false);

  ngOnInit(): void {
    this.modelsService.obtenirTousLesModels().subscribe(data => {
      this.modeles.set(data);
    });
  }

  consulterModele(modele: any): void {
    this.modeleSelectionne.set(modele);
    this.articlesModele.set([]);
    const id = modele.modelListeId;
    this.modelsService.obtenirArticlesDuModel(id).subscribe(articles => {
      this.articlesModele.set(articles);
      this.afficherModale.set(true);
    });
  }

  fermerModale(): void {
    this.afficherModale.set(false);
    this.modeleSelectionne.set(null);
    this.articlesModele.set([]);
  }

  utiliserModele(): void {
    const modeleCourant = this.modeleSelectionne();
    const articlesCourants = this.articlesModele();

    this.router.navigate(['/creer-liste'], {
      state: { 
        titreModele: modeleCourant?.modelListeNom, 
        descriptionModele: modeleCourant?.modelListeDesc,
        articlesInitiaux: articlesCourants 
      }
    });
  }
}