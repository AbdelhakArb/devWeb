import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router'; // BIEN AJOUTER RouterLink ICI !
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink], // BIEN L'AJOUTER DANS LES IMPORTS ICI !
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  email: string = '';
  motDePasse: string = '';
  
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

    this.authService.connexionParent(this.email, this.motDePasse).subscribe({
      next: () => {
        this.router.navigate(['/gestion-liste']);
      },
      error: (err: any) => {
        this.isSubmitting = false;
        this.messageErreur = err.error?.message || "Identifiants invalides ou serveur indisponible.";
      }
    });
  }
}