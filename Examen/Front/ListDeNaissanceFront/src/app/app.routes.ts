import { Routes, Router } from '@angular/router';
import { inject } from '@angular/core'; 
import { AuthService } from './services/auth';

import { HomeComponent } from './pages/home/home';
import { CreerListe } from './pages/creer-liste/creer-liste';
import { GestionListe } from './pages/gestion-liste/gestion-liste';
import { LoginComponent } from './pages/login/login';
import { InscriptionComponent } from './pages/inscription/inscription'; // Import de l'inscription
import { VisiteurViewComponent } from './pages/visiteur-view/visiteur-view';
import { PaiementComponent } from './pages/paiemment/paiement';
import { ListeModels } from './pages/liste-models/liste-models';

const authGuard = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.estAuthentifie()) {
    return true; 
  } else {
    return router.parseUrl('/login'); 
  }
};

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'inscription', component: InscriptionComponent },
  { path: 'visiteur', component: VisiteurViewComponent },
  
  // Routes protégées
  { path: 'creer-liste', component: CreerListe, canActivate: [authGuard] },
  { path: 'gestion-liste', component: GestionListe, canActivate: [authGuard] },
  
  // Pense à ajouter cette route si tu veux que le bouton "Modèles" fonctionne
  { path: 'modeles', component: ListeModels }, 
  
  { path: '**', redirectTo: '' }
];