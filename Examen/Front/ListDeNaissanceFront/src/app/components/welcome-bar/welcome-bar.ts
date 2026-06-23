import { Component, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-welcome-bar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './welcome-bar.html',
  styleUrls: ['./welcome-bar.css']
})
export class WelcomeBar {
  constructor(public authService: AuthService, private router: Router) {}
  
  utilisateur = computed(() => this.authService.utilisateurConnecte());

  deconnexion() {
    this.authService.deconnexion();
    this.router.navigate(['/']);
  }
}