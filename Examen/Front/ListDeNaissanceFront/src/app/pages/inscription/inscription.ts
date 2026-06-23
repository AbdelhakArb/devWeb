import { Component, signal } from '@angular/core';
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
  typeUtilisateur = signal<'parent' | 'visiteur'>('parent');

  nom = signal<string>('');
  prenom = signal<string>('');
  email = signal<string>('');
  ville = signal<string>('');
  motDePasse = signal<string>('');
  confirmationMdp = signal<string>('');

  messageErreur = signal<string>('');
  messageSucces = signal<string>('');
  isSubmitting = signal<boolean>(false);

  constructor(private authService: AuthService, private router: Router) {}

  // Cette méthode remet en place le fonctionnement attendu par ton HTML
  majChamp(champ: 'nom' | 'prenom' | 'email' | 'ville' | 'motDePasse' | 'confirmationMdp', event: Event) {
    const valeur = (event.target as HTMLInputElement).value;
    // On accède au signal via 'this[champ]' et on met à jour sa valeur
    (this as any)[champ].set(valeur);
  }

  onInscription() {
    if (this.motDePasse() !== this.confirmationMdp()) {
      this.messageErreur.set("Les mots de passe ne correspondent pas.");
      return;
    }

    this.isSubmitting.set(true);
    this.messageErreur.set('');
    
    const mode = this.typeUtilisateur();
    const payload = mode === 'parent' ? {
      nomPremierParent: this.nom(),
      prenomPremierParent: this.prenom(),
      emailDeContact: this.email(),
      motDePasseCompte: this.motDePasse(),
      villeParent: this.ville() || null
    } : {
      visiteurNom: this.nom(),
      visiteurPrenom: this.prenom(),
      visiteurEmail: this.email(),
      visiteurMdp: this.motDePasse(),
      visiteurVille: this.ville() || null
    };

    this.authService.inscription(payload, mode).subscribe({
      next: (reponse: any) => {
        this.messageSucces.set(reponse.message || "Compte créé !");
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: (err: any) => {
        this.isSubmitting.set(false);
        this.messageErreur.set(err.error?.error || "Une erreur est survenue.");
      }
    });
  }
}