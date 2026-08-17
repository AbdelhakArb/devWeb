import { Component, inject, signal } from '@angular/core';
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
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  // Propriétés du formulaire
  email: string = '';
  motDePasse: string = '';
  userType: 'Parent' | 'Visiteur' = 'Visiteur';

  // Gestion d'état locale réactive
  readonly messageErreur = signal<string>('');
  readonly isSubmitting = signal<boolean>(false);

  onLogin(): void {
    if (!this.email || !this.motDePasse) {
      this.messageErreur.set('Veuillez remplir tous les champs.');
      return;
    }

    this.isSubmitting.set(true);
    this.messageErreur.set('');

    this.authService.connexion(this.email, this.motDePasse, this.userType).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        // Redirection conforme à app.routes.ts
        if (this.userType === 'Parent') {
          this.router.navigate(['/gestion-liste']);
        } else {
          this.router.navigate(['/visiteur']);
        }
      },
      error: (err: any) => {
        this.isSubmitting.set(false);
        this.messageErreur.set(
          err.error?.message || 'Identifiants invalides ou serveur indisponible.'
        );
      }
    });
  }
}