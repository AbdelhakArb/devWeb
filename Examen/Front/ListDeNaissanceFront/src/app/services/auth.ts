import { Injectable, signal, computed, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'http://localhost:5141/api/comptes';
  private http = inject(HttpClient);
  private router = inject(Router);
  private utilisateurSignal = signal<any | null>(null);

  public utilisateurConnecte = computed(() => this.utilisateurSignal());
  public estAuthentifie = computed(() => this.utilisateurSignal() !== null);

  public estParent = computed(() => {
    const user = this.utilisateurSignal();
    if (!user) return false;
    const role = (user.role || user.Role || user.type || user.Type || '').toLowerCase();
    return role === 'parent' || user.prenomPremierParent !== undefined || user.nomPremierParent !== undefined;
  });

  public estVisiteur = computed(() => {
    const user = this.utilisateurSignal();
    if (!user) return false;
    const role = (user.role || user.Role || user.type || user.Type || '').toLowerCase();
    return role === 'visiteur' || user.visiteurPrenom !== undefined || user.visiteurNom !== undefined;
  });

  constructor() {
    const userStocke = localStorage.getItem('babyList_user');
    if (userStocke) {
      try { 
        this.utilisateurSignal.set(JSON.parse(userStocke)); 
      }
      catch (e) { 
        localStorage.removeItem('babyList_user'); 
      }
    }
  }

  connexion(email: string, motDePasse: string, type: 'Parent' | 'Visiteur' | 'parent' | 'visiteur'): Observable<any> {
    const typeNormalise = type.charAt(0).toUpperCase() + type.slice(1).toLowerCase();
    const endpoint = typeNormalise === 'Parent' ? 'connexion-parent' : 'connexion-visiteur';
    return this.http.post(`${this.baseUrl}/${endpoint}`, { email, motDePasse }).pipe(
      tap((reponse: any) => this.gererConnexionReussie(reponse, typeNormalise))
    );
  }

  inscrireEtConnecter(donnees: any, type: 'Parent' | 'Visiteur' | 'parent' | 'visiteur'): Observable<any> {
    const typeNormalise = type.charAt(0).toUpperCase() + type.slice(1).toLowerCase();
    const endpointInscription = typeNormalise === 'Parent' ? 'inscription-parent' : 'inscription-visiteur';
    const endpointConnexion = typeNormalise === 'Parent' ? 'connexion-parent' : 'connexion-visiteur';

    return this.http.post(`${this.baseUrl}/${endpointInscription}`, donnees).pipe(
      switchMap(() => 
        this.http.post(`${this.baseUrl}/${endpointConnexion}`, { 
          email: donnees.email, 
          motDePasse: donnees.motDePasse 
        })
      ),
      tap((reponse: any) => this.gererConnexionReussie(reponse, typeNormalise))
    );
  }

  private gererConnexionReussie(reponse: any, typeForce?: string) {
    if (reponse) {
      const u = reponse.utilisateur || reponse;
      
      const roleBrut = u.role || u.Role || u.type || u.Type || typeForce;
      let roleFinal = 'Visiteur';
      
      if ((roleBrut && roleBrut.toLowerCase() === 'parent') || u.prenomPremierParent || u.nomPremierParent || u.compteParentId) {
        roleFinal = 'Parent';
      }

      // Mapping rigoureux basé sur tes règles backend (Dapper)
      let idUtilisateur = null;
      let prenomFinal = 'Utilisateur';
      let nomFinal = '';

      if (roleFinal === 'Parent') {
        idUtilisateur = u.idParent || u.IdParent || u.compteParentId || u.CompteParentId || u.id || u.Id;
        prenomFinal = u.prenomPremierParent || u.PrenomPremierParent || u.prenom || 'Parent';
        nomFinal = u.nomPremierParent || u.NomPremierParent || u.nom || '';
      } else {
        idUtilisateur = u.visiteurId || u.VisiteurId || u.id || u.Id;
        prenomFinal = u.visiteurPrenom || u.VisiteurPrenom || u.prenom || 'Visiteur';
        nomFinal = u.visiteurNom || u.VisiteurNom || u.nom || '';
      }

      const utilisateurNormalise = {
        id: idUtilisateur,
        prenom: prenomFinal,
        nom: nomFinal,
        role: roleFinal
      };

      this.utilisateurSignal.set(utilisateurNormalise);
      localStorage.setItem('babyList_user', JSON.stringify(utilisateurNormalise));
    }
  }

  deconnexion(): void {
    this.utilisateurSignal.set(null);
    localStorage.removeItem('babyList_user');
    this.router.navigate(['/']);
  }
}