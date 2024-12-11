import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, RouterModule, CommonModule],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss'
})
export class NavBarComponent {
  public ApplicationTitle: string = "Tournament Planner"
  public userRole: string | null = null; // Define userRole variable

  constructor() {
    // Example: Set userRole based on some logic
    // this.userRole = 'admin'; // Uncomment to simulate an admin user
    // this.userRole = 'player'; // Uncomment to simulate a player user
    // this.userRole = null; // Uncomment to simulate a guest user
  }

}
