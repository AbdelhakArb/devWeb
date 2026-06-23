import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  email: string = '';
  motDePasse: string = '';
  userType: 'Parent' | 'Visiteur' = 'Visiteur'; // Valeur par défaut
  
  messageErreur: string = '';
  isSubmitting: boolean = false;

  constructor(private authService: AuthService, private router: Router) {}

  onLogin() {
    if (!this.email || !this.motDePasse) {
      this.messageErreur = "Veuillez remplir tous les champs.";
      return;
    }

    this.isSubmitting = true;
    this.messageErreur = '';

    // Appel du service via la méthode générique que nous avons créée
    this.authService.connexion(this.email, this.motDePasse, this.userType).subscribe({
      next: () => {
        // Redirection adaptée au profil
        if (this.userType === 'Parent') {
          this.router.navigate(['/gestion-liste']);
        } else {
          this.router.navigate(['/visiteur-view']);
        }
      },
      error: (err: any) => {
        this.isSubmitting = false;
        this.messageErreur = err.error?.message || "Identifiants invalides ou serveur indisponible.";
      }
    });
  }
}