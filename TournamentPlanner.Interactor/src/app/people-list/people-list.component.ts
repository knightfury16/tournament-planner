import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { TrippinService } from '../trippin.service';

@Component({
  selector: 'app-people-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './people-list.component.html',
  styleUrl: './people-list.component.scss',
})
export class PeopleListComponent {
  nameInput = new FormControl();

  name = this.nameInput.valueChanges;

  public input$ = this.nameInput.valueChanges.pipe(
    debounceTime(1000),
    distinctUntilChanged(),
    switchMap((name) => this.trippin.getPeople(name))
  );

  ngOnInit() {}

  constructor(private trippin: TrippinService) {}
}
