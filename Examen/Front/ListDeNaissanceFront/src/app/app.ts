import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { WelcomeBar } from './components/welcome-bar/welcome-bar';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, WelcomeBar],
  template: `
    <app-welcome-bar></app-welcome-bar>
    <router-outlet></router-outlet>
  `,
  styleUrl: './app.css'
})
export class AppComponent {}