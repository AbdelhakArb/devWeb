import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth'; 

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent {
  public authService = inject(AuthService);
  private router = inject(Router);

  // Méthode de contrôle d'accès au clic sur les boutons protégés
  accederAction(route: string, roleRequis: 'Parent' | 'Visiteur') {
   if (!this.authService.estAuthentifie()) { // Remplacer par le nom exact présent dans ton AuthService
  this.router.navigate(['/login']);
  return;
}

    // Vérification du rôle via les Signals
    const estBonRole = roleRequis === 'Parent' ? this.authService.estParent() : this.authService.estVisiteur();

    if (estBonRole) {
      this.router.navigate([route]);
    } else {
      alert(`Accès refusé. Cette action est réservée aux ${roleRequis}s.`);
    }
  }
}