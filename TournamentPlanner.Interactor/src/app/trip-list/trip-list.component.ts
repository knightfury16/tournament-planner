import { AfterViewInit, Component, Input } from '@angular/core';
import { TrippinService } from '../trippin.service';
import { Observable } from 'rxjs';
import { Trip } from '../../../models/trippin/TrippinModel';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-trip-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './trip-list.component.html',
  styleUrl: './trip-list.component.scss',
})
export class TripListComponent implements AfterViewInit {
  @Input() public userName?: string;
  public data$?: Observable<{ value: Trip[] }>;

  constructor(private trip: TrippinService) {}
  ngAfterViewInit(): void {
    if (this.userName) {
      this.data$ = this.trip.getTrips(this.userName);
    }
  }
}
