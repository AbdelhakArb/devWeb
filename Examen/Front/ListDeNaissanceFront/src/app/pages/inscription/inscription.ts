import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-inscription',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './inscription.html',
  styleUrl: '../login/login.css'
})
export class InscriptionComponent {
  typeUtilisateur = signal<'parent' | 'visiteur'>('parent');

  // Champs du formulaire (Signals locaux)
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

  majChamp(champ: 'nom' | 'prenom' | 'email' | 'ville' | 'motDePasse' | 'confirmationMdp', event: Event) {
    this[champ].set((event.target as HTMLInputElement).value);
  }

  onInscription() {
    if (!this.nom() || !this.prenom() || !this.email() || !this.motDePasse() || !this.confirmationMdp()) {
      this.messageErreur.set("Veuillez remplir les champs obligatoires.");
      return;
    }

    if (this.motDePasse() !== this.confirmationMdp()) {
      this.messageErreur.set("Les mots de passe ne correspondent pas.");
      return;
    }

    this.isSubmitting.set(true);
    this.messageErreur.set('');
    this.messageSucces.set('');

    let payload: any = {};
    const mode = this.typeUtilisateur();

    // Mapping strict avec les objets de ton Backend C#
    if (mode === 'parent') {
      payload = {
        nomPremierParent: this.nom(),
        prenomPremierParent: this.prenom(),
        emailDeContact: this.email(),
        motDePasseCompte: this.motDePasse(),
        villeParent: this.ville() || null
      };
    } else {
      payload = {
        visiteurNom: this.nom(),
        visiteurPrenom: this.prenom(),
        visiteurEmail: this.email(),
        visiteurMdp: this.motDePasse(),
        visiteurVille: this.ville() || null
      };
    }

    this.authService.inscription(payload, mode).subscribe({
      next: (reponse: any) => {
        this.messageSucces.set(reponse.message || "Compte créé avec succès ! Redirection...");
        setTimeout(() => {
          this.isSubmitting.set(false);
          this.router.navigate(['/']); // Redirection vers le Hub d'accueil
        }, 2000);
      },
      error: (err: any) => {
        this.isSubmitting.set(false);
        this.messageErreur.set(err.error?.error || "Une erreur est survenue lors de l'enregistrement.");
        console.error("Détails du rejet de validation C# :", err);
      }
    });
  }
}