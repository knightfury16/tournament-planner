import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-trip-list',
  standalone: true,
  imports: [],
  templateUrl: './trip-list.component.html',
  styleUrl: './trip-list.component.scss',
})
export class TripListComponent {
  @Input() public userName?: string;
}
