import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-spacex',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './spacex.component.html',
  styleUrl: './spacex.component.scss'
})
export class SpaceXComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  public signOut(): void {
    this.authService.signOut();
    this.router.navigate(['/signin']);
  }
}
