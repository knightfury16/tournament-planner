import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTabChangeEvent, MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { CashRegisterComponent } from "../cash-register/cash-register.component";
import { TournamentListComponent } from "../tournament-list/tournament-list.component";

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [MatCardModule, MatIconModule, RouterModule, MatTabsModule, CashRegisterComponent, TournamentListComponent],
  templateUrl: './test.component.html',
  styleUrl: './test.component.scss'
})
export class TestComponent {

  public mytabIndex = 0;
  public fromTabChange = 0;
  public tabChange($event: MatTabChangeEvent) {
    this.fromTabChange = $event.index;
  }

}
  