import { Component } from '@angular/core'; 
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ListeNaissanceService } from '../../services/api/liste-naissance';
import { ListeDeNaissance } from '../../services/api/models/liste-naissance';
import { AuthService } from '../../services/auth'; 

@Component({
  selector: 'app-creer-liste',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './creer-liste.html',
  styleUrl: './creer-liste.css',
})
export class CreerListe { 
  nomListe: string = '';
  dateAccouchement: string = '';
  lieu: string = '';

  messageSucces: string = '';
  messageErreur: string = '';
  isSubmitting: boolean = false; 

  constructor(
    private listeService: ListeNaissanceService,
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit() {
    if (!this.nomListe.trim()) {
      this.messageErreur = "Le nom de la liste est obligatoire.";
      return;
    }

    this.isSubmitting = true;
    this.messageErreur = '';

    const parentIdConnecte = this.authService.utilisateurConnecte()?.id;

    const nouvelleListe: ListeDeNaissance = {
      compteParentId: parentIdConnecte || 1,
      nomListeDeNaissance: this.nomListe,
      datePrevuPourAccouchement: this.dateAccouchement ? new Date(this.dateAccouchement) : undefined,
      lieuListe: this.lieu.trim() || undefined,
      statusListe: 'Active'
    };

    this.listeService.creerListe(nouvelleListe).subscribe({
      next: (response: any) => {
        this.messageSucces = "Liste créée ! Redirection vers la gestion de votre liste...";
        setTimeout(() => {
          this.router.navigate(['/gestion-liste']);
        }, 2000);
      },
      error: (err: any) => {
        this.messageErreur = err.error?.message || "Une erreur est survenue lors de la création.";
        this.isSubmitting = false;
      }
    });
  }
}