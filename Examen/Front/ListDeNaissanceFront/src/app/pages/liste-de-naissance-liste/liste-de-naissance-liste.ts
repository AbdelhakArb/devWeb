import { Component, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-liste-de-naissance-liste',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './liste-de-naissance-liste.html',
  styleUrl: './liste-de-naissance-liste.css'
})
export class ListeDeNaissanceListComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  readonly listes = signal<any[]>([]);
  readonly isLoading = signal<boolean>(true);
  readonly errorMessage = signal<string>('');

  ngOnInit(): void {
    const user: any = this.authService.utilisateurConnecte();
    const parentId = user?.id || user?.compteParentId || user?.idCompteParent;

    if (!user || !parentId) {
      this.errorMessage.set("Utilisateur non connecté ou ID introuvable.");
      this.isLoading.set(false);
      return;
    }

    this.http.get<any[]>(`http://localhost:5141/api/listenaissance/parent/${parentId}`).subscribe({
      next: (data) => {
        this.listes.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error("Erreur lors de la récupération des listes :", err);
        this.errorMessage.set("Impossible de charger vos listes de naissance.");
        this.isLoading.set(false);
      }
    });
  }

  selectionnerListe(listeId: number): void {
    const liste = this.listes().find(l => l.listeDeNaissanceId === listeId);
    if (liste && liste.statusListe === 'Cloturee') {
      return;
    }
    this.router.navigate(['/gestion-liste', listeId]);
  }
}