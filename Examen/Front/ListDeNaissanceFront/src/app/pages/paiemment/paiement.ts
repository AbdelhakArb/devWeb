import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-paiement',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="paiement-construction-container">
      <div class="icon-construction">🚧</div>
      <h2>Page de Paiement</h2>
      <p class="alert warning">Cette page est actuellement en cours de construction.</p>
      <p class="info">Le module de passerelle bancaire (Stripe/PayPal) sera intégré lors de la prochaine version.</p>
      <button routerLink="/gestion-liste" class="btn-retour">Retour à la gestion</button>
    </div>
  `,
  styles: [`
    .paiement-construction-container {
      max-width: 500px;
      margin: 60px auto;
      padding: 40px;
      text-align: center;
      background: #fff;
      border-radius: 8px;
      box-shadow: 0 4px 15px rgba(0,0,0,0.1);
      font-family: sans-serif;
    }
    .icon-construction { font-size: 4rem; margin-bottom: 20px; }
    .warning { background: #fff3cd; color: #856404; padding: 12px; border-radius: 4px; margin: 20px 0; }
    .info { color: #666; margin-bottom: 30px; }
    .btn-retour { background: #3498db; color: white; border: none; padding: 10px 20px; border-radius: 4px; cursor: pointer; font-weight: bold; }
  `]
})
export class PaiementComponent {}