import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { FooterComponent } from "./footer/footer.component";
import { SpinnerComponent } from "./spinner/spinner.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NavBarComponent, RouterOutlet, RouterModule, FooterComponent, SpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'TournamentPlanner';
}
