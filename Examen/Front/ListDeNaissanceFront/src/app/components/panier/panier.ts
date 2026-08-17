import { Component, input, model, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PanierItem } from '../../services/api/models/panier';

@Component({
  selector: 'app-panier',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './panier.html',
  styleUrl: './panier.css',
})
export class PanierComponent {
  readonly items = input<PanierItem[]>([]);
  readonly isParent = input<boolean>(false);
  readonly visiteurId = input<number>(0);
  
  readonly message = model<string>('');
  readonly signature = model<string>('');

  readonly confirmer = output<any[]>();
  readonly annuler = output<void>();

  calculerTotal(): number {
    return this.items().reduce((acc, item) => {
      const prix = item.articlePrix || 0;
      const qty = item.articleQty || 1;
      return acc + (prix * qty);
    }, 0);
  }

  surConfirmer(): void {
    const payload = this.items().map(item => ({
      listeDeNaissanceId: item.listeDeNaissanceId || 0,
      articleId: item.articleId,
      qtySouhaitee: item.articleQty || 1,
      visiteurId: this.visiteurId(),
      messageText: this.message(),
      signatureMessage: this.signature()
    }));
    console.log('[DEBUG] items() dans panier au moment du clic:', this.items()); // ← ajoute ça
    console.log('[DEBUG] payload construit:', payload); // ← ajoute ça

    this.confirmer.emit(payload);
  }

  surAnnuler(): void {
    this.annuler.emit();
  }
}