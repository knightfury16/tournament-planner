import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, startWith, switchMap } from 'rxjs';
import { TrippinService } from '../trippin.service';

@Component({
  selector: 'app-airports-list',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './airports-list.component.html',
  styleUrl: './airports-list.component.scss'
})
export class AirportsListComponent {

  nameInput = new FormControl();

  name = this.nameInput.valueChanges;

  public data$ = this.nameInput.valueChanges.pipe(
    startWith(''),
    debounceTime(1000),
    distinctUntilChanged(),
    switchMap((name) => this.trippin.getAirports(name))
  );

  ngOnInit() {}

  constructor(private trippin: TrippinService) {}

}
