import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { SignInComponent } from './features/auth/signin/signin.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/spacex',
    pathMatch: 'full'
  },
  {
    path: 'signin',
    component: SignInComponent
  },
  {
    path: 'signup',
    loadComponent: () =>
      import('./features/auth/signup/signup.component').then(m => m.SignUpComponent)
  },
  {
    path: 'spacex',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/spacex/spacex.component').then(m => m.SpaceXComponent)
  },
  {
    path: '**',
    redirectTo: '/spacex'
  }
];
