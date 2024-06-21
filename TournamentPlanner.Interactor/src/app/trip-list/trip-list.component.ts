import { AfterViewInit, Component, Input } from '@angular/core';
import { TrippinService } from '../trippin.service';
import { Observable } from 'rxjs';
import { Trip } from '../../../models/trippin/TrippinModel';
import { CommonModule } from '@angular/common';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-trip-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './trip-list.component.html',
  styleUrl: './trip-list.component.scss',
})
export class TripListComponent implements AfterViewInit {
  @Input() public userName?: string;
  public data$?: Observable<{ value: Trip[] }>;

  public tripForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    budget: new FormControl('1000', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    tags: new FormControl(''),
    startsAt: new FormControl('', [Validators.required]),
    endsAt: new FormControl('', [Validators.required]),
  });

  constructor(private trip: TrippinService) {}
  ngAfterViewInit(): void {
    if (this.userName) {
      this.data$ = this.trip.getTrips(this.userName);
    }
  }

  addTrip(){

  }



}
