import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // Adresse IP et port exacts issus de ton Minimal API C#
  private baseUrl = 'http://localhost:5141/api/comptes';

  // Gestion de l'état global avec les Signals Angular (Exigence professeur)
  private utilisateurSignal = signal<any | null>(null);

  public utilisateurConnecte = computed(() => this.utilisateurSignal());
  public estAuthentifie = computed(() => this.utilisateurSignal() !== null);

  constructor(private http: HttpClient) {
    // Restauration automatique de la session au démarrage
    const userStocke = localStorage.getItem('babyList_user');
    if (userStocke) {
      try {
        this.utilisateurSignal.set(JSON.parse(userStocke));
      } catch (e) {
        localStorage.removeItem('babyList_user');
      }
    }
  }

  /**
   * Envoie les données vers le bon endpoint selon le type sélectionné
   */
  inscription(donnees: any, type: 'parent' | 'visiteur'): Observable<any> {
    const routeFinale = type === 'parent' ? 'inscription-parent' : 'inscription-visiteur';
    return this.http.post(`${this.baseUrl}/${routeFinale}`, donnees).pipe(
      tap((reponseServer: any) => {
        // Si le serveur te renvoie l'objet utilisateur créé, on le stocke
        if (reponseServer && reponseServer.utilisateur) {
          this.utilisateurSignal.set(reponseServer.utilisateur);
          localStorage.setItem('babyList_user', JSON.stringify(reponseServer.utilisateur));
        }
      })
    );
  }

  /**
   * Connexion Parent
   */
  connexionParent(email: string, motDePasse: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/connexion-parent`, { email, motDePasse }).pipe(
      tap((reponseServer: any) => {
        if (reponseServer && reponseServer.utilisateur) {
          this.utilisateurSignal.set(reponseServer.utilisateur);
          localStorage.setItem('babyList_user', JSON.stringify(reponseServer.utilisateur));
        }
      })
    );
  }

  /**
   * Connexion Visiteur
   */
  connexionVisiteur(email: string, motDePasse: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/connexion-visiteur`, { email, motDePasse }).pipe(
      tap((reponseServer: any) => {
        if (reponseServer && reponseServer.utilisateur) {
          this.utilisateurSignal.set(reponseServer.utilisateur);
          localStorage.setItem('babyList_user', JSON.stringify(reponseServer.utilisateur));
        }
      })
    );
  }


  deconnexion(): void {
    this.utilisateurSignal.set(null);
    localStorage.removeItem('babyList_user');
  }
}