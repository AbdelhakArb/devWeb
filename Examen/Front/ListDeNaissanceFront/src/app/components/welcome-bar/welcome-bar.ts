import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-welcome-bar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './welcome-bar.html',
  styleUrls: ['./welcome-bar.css']
})
export class WelcomeBarComponent {
  public authService = inject(AuthService);
}