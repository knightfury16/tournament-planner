import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService, UserInfo } from '../../Shared/auth.service';
import { DomainRole } from '../tp-model/TpModel';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    RouterModule,
    CommonModule,
    MatMenuModule
  ],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss',
})
export class NavBarComponent {
  public ApplicationTitle: string = "Tournament Planner"
  public userRole: string | null = null; // Define userRole variable

  constructor(private authService: AuthService) {
    // Example: Set userRole based on some logic
    // this.userRole = 'admin'; // Uncomment to simulate an admin user
    // this.userRole = 'player'; // Uncomment to simulate a player user
    // this.userRole = null; // Uncomment to simulate a guest user
  }

  public getCurrentUser(): UserInfo | null {
    return this.authService.currentUser();
  }

  public getCurrentUserRole(): string | undefined {
    return this.authService.currentUser()?.role;
  }

  public isCurrentUserAdmin(): boolean {
    return this.getCurrentUserRole() === DomainRole.Admin.toString();
  }

  public isCurrentUserPlayer(): boolean {
    return this.getCurrentUserRole() === DomainRole.Player.toString();
  }
  public async signOut() {
    await this.authService.singOut();
  }
}
