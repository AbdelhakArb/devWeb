import { Routes, Router, CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from './services/auth';
import { CreerListeComponent } from './pages/creer-liste/creer-liste';
import { HomeComponent } from './pages/home/home';
import { GestionListe } from './pages/gestion-liste/gestion-liste';
import { LoginComponent } from './pages/login/login';
import { InscriptionComponent } from './pages/inscription/inscription';
import { VisiteurViewComponent } from './pages/visiteur-view/visiteur-view';
import { ListeModelsComponent } from './pages/liste-models/liste-models';
import { ListeDeNaissanceListComponent } from './pages/liste-de-naissance-liste/liste-de-naissance-liste';

const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.estAuthentifie()) {
    return true;
  }
  return router.parseUrl('/login');
};

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'inscription', component: InscriptionComponent },
  { path: 'visiteur', component: VisiteurViewComponent },
  { path: 'modeles', component: ListeModelsComponent },


  // Routes protégées par le Guard
  { path: 'creer-liste', component: CreerListeComponent, canActivate: [authGuard] },
  { path: 'mes-listes', component: ListeDeNaissanceListComponent, canActivate: [authGuard] },
  { path: 'gestion-liste/:id', component: GestionListe, canActivate: [authGuard] },
  { path: 'gestion-liste', redirectTo: 'mes-listes', pathMatch: 'full' },

  { path: '**', redirectTo: '' }
];