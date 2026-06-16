import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router'; // Pour la redirection
import { ListeNaissanceService } from '../../services/api/liste-naissance';
import { ListeDeNaissance } from '../../services/api/models/liste-naissance';

@Component({
  selector: 'app-creer-liste',
  imports: [CommonModule, FormsModule],
  templateUrl: './creer-liste.html',
  styleUrl: './creer-liste.css',
})
export class CreerListe implements OnInit {
  nomListe: string = '';
  dateAccouchement: string = '';
  lieu: string = '';

  messageSucces: string = '';
  messageErreur: string = '';
  
  // 1. Variable pour désactiver le bouton pendant le chargement
  isSubmitting: boolean = false; 

  // Simulation d'un état de connexion (à remplacer plus tard par ton service d'authentification)
  estConnecte: boolean = true; 

  constructor(
    private listeService: ListeNaissanceService,
    private router: Router // Injection du routeur
  ) {}

  ngOnInit() {
    // 3. Sécurité : Si l'utilisateur n'est pas connecté, on le jette (vers la page login par exemple)
    if (!this.estConnecte) {
      alert("Accès refusé ! Vous devez être connecté pour créer une liste.");
      this.router.navigate(['/login-page']); // Redirection vers ta page login existante
    }
  }

  onSubmit() {
    if (!this.nomListe.trim()) {
      this.messageErreur = "Le nom de la liste est obligatoire.";
      return;
    }

    // Désactive le bouton dès le clic
    this.isSubmitting = true;
    this.messageErreur = '';

    const nouvelleListe: ListeDeNaissance = {
      compteParentId: 1, 
      nomListeDeNaissance: this.nomListe,
      datePrevuPourAccouchement: this.dateAccouchement ? new Date(this.dateAccouchement) : undefined,
      lieuListe: this.lieu.trim() || undefined,
      statusListe: 'Active'
    };

    this.listeService.creerListe(nouvelleListe).subscribe({
      next: (response) => {
        this.messageSucces = "Liste créée ! Redirection vers la gestion de votre liste...";
        
        // 2. Attendre 2 petites secondes pour laisser l'utilisateur voir le succès, puis rediriger
        setTimeout(() => {
          this.router.navigate(['/gestion-liste']);
        }, 2000);
      },
      error: (err) => {
        this.messageErreur = err.error?.message || "Une erreur est survenue.";
        this.messageSucces = '';
        this.isSubmitting = false; // Réactive le bouton en cas d'échec pour qu'il puisse réessayer
      }
    });
  }
}