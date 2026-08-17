import { Component, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-inscription',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './inscription.html',
  styleUrl: '../login/login.css'
})
export class InscriptionComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly typeUtilisateur = signal<'parent' | 'visiteur'>('parent');
  
  nom = '';
  prenom = '';
  email = '';
  adresse = '';
  cp = '';
  ville = '';
  pays = '';
  motDePasse = '';
  confirmationMdp = '';

  readonly messageErreur = signal<string>('');
  readonly messageSucces = signal<string>('');
  readonly isSubmitting = signal<boolean>(false);

  onInscription(): void {
    if (this.motDePasse !== this.confirmationMdp) {
      this.messageErreur.set("Les mots de passe ne correspondent pas.");
      return;
    }

    this.isSubmitting.set(true);
    this.messageErreur.set('');
    this.messageSucces.set('');
    
    const mode = this.typeUtilisateur();
    
    const payload = {
      nom: this.nom,
      prenom: this.prenom,
      email: this.email,
      motDePasse: this.motDePasse,
      adresse: this.adresse || null,
      cp: this.cp || null,
      ville: this.ville || null,
      pays: this.pays || null
    };

    this.authService.inscrireEtConnecter(payload, mode).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.messageSucces.set("Inscription et connexion réussies !");
        setTimeout(() => this.router.navigate(['/']), 1500);
      },
      error: (err: any) => {
        this.isSubmitting.set(false);
        this.messageErreur.set(err.error?.error || err.error?.message || "Erreur lors de l'inscription.");
      }
    });
  }
}