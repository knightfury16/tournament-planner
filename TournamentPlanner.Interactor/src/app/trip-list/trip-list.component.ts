import { AfterViewInit, Component, Input } from '@angular/core';
import { TrippinService } from '../trippin.service';
import { Observable, firstValueFrom } from 'rxjs';
import { Trip } from '../../../models/trippin/TrippinModel';
import { CommonModule } from '@angular/common';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Guid } from 'guid-typescript';

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
    budget: new FormControl(1000, [Validators.required]),
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

  async addTrip(){
    const newTrip: Trip = {
      TripId: Math.floor(Math.random() * 10000),
      ShareId: Guid.create().toString(),
      Name: this.tripForm.value.name ?? '',
      Budget: this.tripForm.value.budget ?? 0,
      Description: this.tripForm.value.description ?? '',
      Tags: (this.tripForm.value.tags ?? '').split(','),
      StartsAt: new Date(this.tripForm.value.startsAt ?? '').toISOString(),
      EndsAt: new Date(this.tripForm.value.endsAt ?? '').toISOString(),
    }
    await firstValueFrom(this.trip.addTrip(this.userName!, newTrip));
    this.ngAfterViewInit();
    this.tripForm.reset();
  }
}
